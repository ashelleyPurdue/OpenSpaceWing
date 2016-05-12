using UnityEngine;
using System.Collections;

public class PlayerGunBehaviour : MonoBehaviour
{
    //Constants
    public const float MAX_DIST = 500;
    public const float SCREEN_SPHERECAST_RADIUS = 0.15f;
    public const float BULLET_SPHERECAST_RADIUS = 0.2f;

    public const float BULLET_SPEED = 500f;
    public const float FIRE_RATE = 10;       //Bullets per second

    //Config toggles
    public static bool useTargettedBullets = false;


    //Properties
    public Vector3 AimPoint
    {
        get { return aimPoint; }
    }

    public Vector3 AimPointNormal
    {
        get { return aimPointNormal; }
    }

    public Transform TargettedObject
    {
        get { return targettedObject; }
    }


    //Private fields
    private Vector3 gunPos = Vector3.zero;  //Position of the gun in local space
    private GameObject bulletOriginal;
    private DamageSource damageSrc;

    private Vector3 aimPoint = Vector3.zero;
    private Vector3 aimPointNormal = Vector3.forward;
    private Transform targettedObject = null;

    private bool distanceLocked = false;
    private float lockedDistance = MAX_DIST;

    private float repeatFireTimer = 10;


    //Events
    void Awake()
    {
        bulletOriginal = Resources.Load<GameObject>("testBullet_prefab");
        damageSrc = GetComponent<DamageSource>();
    }

    void Update()
    {
        //Update the aim point
        UpdateAimPoint();

        //Look at aim point
        transform.LookAt(aimPoint);

        //Shoot a lazer where the player is aiming
        if (Input.GetButton("Fire1"))
        {
            //Fire if timer is up
            if (repeatFireTimer <= 0)
            {
                Fire();
                repeatFireTimer = 1f / FIRE_RATE;
            }

            //Increment the timer
            repeatFireTimer -= Time.deltaTime;
        }
        else
        {
            //Clear the timer
            repeatFireTimer = 0;
        }
    }


    //Misc methods

    private void Fire()
    {
        //Fires the bullet

        //Find the direction the bullet should travel in
        Vector3 bulletDir = (aimPoint - transform.TransformPoint(gunPos)).normalized;

        //Do a raycast so we can get the healthpoints of the object being fired at.
        HealthPoints targetHP = null;
        
        Ray ray = new Ray(transform.position, (aimPoint - transform.position).normalized);
        RaycastHit[] hits = Physics.SphereCastAll(ray, BULLET_SPHERECAST_RADIUS, Vector3.Distance(transform.position, aimPoint));

        float closestDist = float.MaxValue;
        foreach (RaycastHit hit in hits)
        {
            if (hit.distance < closestDist)
            {
                targetHP = hit.transform.GetComponent<HealthPoints>();
                closestDist = hit.distance;
            }
        }

        //Create the bullet and fire it.
        Rigidbody bulletRigidbody;
        if (useTargettedBullets)
        {
            TargettedBulletBehaviour bullet = TargettedBulletBehaviour.Create(transform.position, targetHP, damageSrc, closestDist / BULLET_SPEED);
            bulletRigidbody = bullet.myRigidbody;
        }
        else
        {
            //Create the bullet
            GameObject bullet = Instantiate(Resources.Load<GameObject>("testBullet_prefab"));
            bulletRigidbody = bullet.GetComponent<Rigidbody>();
            bullet.transform.position = transform.position;

            //Configure the damage source
            DamageSource bulletSrc = bullet.GetComponent<DamageSource>();
            bulletSrc.damageAmount = damageSrc.damageAmount;
            bulletSrc.tags = damageSrc.tags;
            bulletSrc.useDefaultHitDetection = true;
        }

        bulletRigidbody.AddForce(bulletDir * BULLET_SPEED, ForceMode.VelocityChange);
    }

    private void UpdateAimPoint()
    {
        //Returns the point in world space that the player is aiming at.

        //If the player is holding the "lock distance" button, use the locked distance.
        //TODO: Rewrite this with distance locking in mind.
        float dist = MAX_DIST;
        if (distanceLocked)
        {
            dist = lockedDistance;
        }

        //Transform the mouse position to a viewport point
        Vector3 mousePos = Vector3.zero;
        mousePos.x = Mathf.InverseLerp(0, Screen.width, Input.mousePosition.x);
        mousePos.y = Mathf.InverseLerp(0, Screen.height, Input.mousePosition.y);

        //Do a spherecast.
        Ray ray = Camera.main.ViewportPointToRay(mousePos);
        RaycastHit[] hits = Physics.SphereCastAll(ray, SCREEN_SPHERECAST_RADIUS, MAX_DIST);

        //If no hits were found, just set it to the maximum distance.  Else, use the closest point
        float closestDist = dist;

        if (hits.Length == 0)
        {
            aimPoint = ray.GetPoint(dist);
            aimPointNormal = Vector3.forward;
            targettedObject = null;
        }
        else
        {
            //Choose the point closest to the camera
            RaycastHit closestHit = hits[0];

            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].distance < closestHit.distance)
                {
                    closestHit = hits[i];
                }
            }

            //"Return" the closest point
            aimPoint = closestHit.point;
            aimPointNormal = closestHit.normal;
            targettedObject = closestHit.transform;

            closestDist = closestHit.distance;
        }

        //If the distance is not locked, save the lockedDistance
        distanceLocked = Input.GetButton("Fire2");
        if (!distanceLocked)
        {
            lockedDistance = closestDist;
        }
    }
}
