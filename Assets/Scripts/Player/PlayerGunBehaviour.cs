using UnityEngine;
using System.Collections;

public class PlayerGunBehaviour : MonoBehaviour
{
    //Constants
    public const float MAX_DIST = 50;


    //Properties
    public Vector3 AimPoint
    {
        get { return aimPoint; }
    }

    public Vector3 AimPointNormal
    {
        get { return aimPointNormal; }
    }

    //Private fields
    private Vector3 gunPos = Vector3.zero;  //Position of the gun in local space
    private GameObject bulletOriginal;

    private Vector3 aimPoint = Vector3.zero;
    private Vector3 aimPointNormal = Vector3.forward;

    private float bulletSpeed = 50f;


    //Events
    void Awake()
    {
        bulletOriginal = Resources.Load<GameObject>("testBullet_prefab");
    }

    void Update()
    {
        //Update the aim point
        UpdateAimPoint();

        //Look at aim point
        transform.LookAt(aimPoint);

        //Shoot a lazer where the player is aiming
        if (Input.GetButtonDown("Fire1"))
        {
            //Find the direction the bullet should travel in
            Vector3 bulletDir = (aimPoint - transform.TransformPoint(gunPos)).normalized;

            //Fire the bullet
            GameObject bullet = GameObject.Instantiate(bulletOriginal);
            bullet.transform.position = transform.TransformPoint(gunPos);
            bullet.GetComponent<Rigidbody>().velocity = bulletDir * bulletSpeed;
        }
    }


    //Misc methods

    private void UpdateAimPoint()
    {
        //Returns the point in world space that the player is aiming at.

        //Transform the mouse position to a viewport point
        Vector3 mousePos = Vector3.zero;
        mousePos.x = Mathf.InverseLerp(0, Screen.width, Input.mousePosition.x);
        mousePos.y = Mathf.InverseLerp(0, Screen.height, Input.mousePosition.y);

        Debug.Log(mousePos);

        //Cast a ray
        Ray ray = Camera.main.ViewportPointToRay(mousePos);
        RaycastHit[] hits = Physics.RaycastAll(ray, MAX_DIST);

        //If no hits were found, just set it to the maximum distance
        if (hits.Length == 0)
        {
            aimPoint = ray.GetPoint(MAX_DIST);
            aimPointNormal = Vector3.forward;
            return;
        }

        //Choose the point closest to the camera
        Vector3 closestPont = Vector3.zero;
        Vector3 closestNormal = Vector3.zero;
        float closestDist = float.MaxValue;

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].distance < closestDist)
            {
                closestDist = hits[i].distance;
                closestPont = hits[i].point;
                closestNormal = hits[i].normal;
            }
        }

        //Return the closest point
        aimPoint = closestPont;
        aimPointNormal = closestNormal;
    }
}
