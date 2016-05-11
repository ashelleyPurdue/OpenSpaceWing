using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class PlayerLaserSightBehaviour : MonoBehaviour
{
    //Constants
    public const float FORWARD_OFFSET = 0.01f;
    public const float ROTATION_CUTOFF_DISTANCE = 2f;      //The crosshair will not rotate when targetting objects this far away.

    public const float RAYCAST_RADIUS = 0.2f;       //How much leniency the raycast has


    //Global settings
    public static bool enableHitCrosshair = true;
    public static bool enableAimCrosshair = false;
    public static bool enabledCursor = true;

    //References
    public PlayerGunBehaviour gun;
    public Texture2D cursor;
    public Transform hitCrosshair;
    public Transform aimCrosshair;


    //Private fields
    private LineRenderer line;

    
    //Events
    void Awake()
    {
        //Get the line renderer
        line = GetComponent<LineRenderer>();

        //Un-parent the crosshair object
        hitCrosshair.SetParent(null);

        //Set the cursor
        if (enabledCursor)
        {
            Vector2 center = new Vector2(cursor.width, cursor.height);
            center *= 0.5f;

            Cursor.SetCursor(cursor, center, CursorMode.Auto);
        }
    }

    void Update()
    {
        //Draw a line to the target
        line.SetPosition(0, transform.position);
        line.SetPosition(1, gun.AimPoint);

        //Update the aim crosshair
        aimCrosshair.GetComponent<SpriteRenderer>().enabled = enableAimCrosshair;
        if (enableAimCrosshair)
        {
            UpdateCrosshair(aimCrosshair, gun.TargettedObject, gun.AimPoint, gun.AimPointNormal);
        }

        //Update the hit crosshair
        hitCrosshair.GetComponent<SpriteRenderer>().enabled = enableHitCrosshair;
        UpdateHitCrosshair();
    }

    void OnApplicationFocus(bool status)
    {
        //If the cursor is not enabled, hide the cursor when the application is in focus
        if (!enabledCursor)
        {
            Cursor.visible = !status;
        }
    }
    


    //Misc methods

    private void UpdateCrosshair(Transform crosshair, Transform targettedObject, Vector3 point, Vector3 normal)
    {
        //Updates the given crosshair's position and rotaiton.
        //Special processing is done to make the results look good

        //Default to using the given normal
        crosshair.forward = normal;

        //If the targetted object has a rigidbody, use a different method of rotating so the crosshair doesn't look bad
        if (targettedObject != null && targettedObject.GetComponent<Rigidbody>() != null)
        {
            //Rotate the way the gun is facing if too far away
            if (Vector3.Distance(point, gun.transform.position) >= ROTATION_CUTOFF_DISTANCE)
            {
                crosshair.forward = gun.transform.forward;
            }
        }

        //Update the position
        crosshair.position = point + crosshair.forward * FORWARD_OFFSET;
    }

    private void UpdateHitCrosshair()
    {
        //Find the point where the bullet would *actually* hit when fired.
        Vector3 bulletDir = (gun.AimPoint - gun.transform.position).normalized;
        float aimpointDist = Vector3.Distance(gun.AimPoint, gun.transform.position);

        RaycastHit[] hits = Physics.SphereCastAll(gun.transform.position, RAYCAST_RADIUS, bulletDir, aimpointDist);

        bool foundHit = false;
        RaycastHit closestHit = new RaycastHit();
        float closestDist = float.MaxValue;

        foreach (RaycastHit h in hits)
        {
            if (h.distance < closestDist)
            {
                closestDist = h.distance;
                closestHit = h;

                foundHit = true;
            }
        }

        if (foundHit)
        {
            //Move the crosshair to where the bullet would hit, if a point was found.
            UpdateCrosshair(hitCrosshair, closestHit.transform, closestHit.point, closestHit.normal);
        }
        else
        {
            //If no point was found(IE: player aiming off into the distance), move to the aim point
            Vector3 normal = (gun.AimPoint - gun.transform.position).normalized;
            Vector3 pos = gun.transform.position + bulletDir * aimpointDist;
            UpdateCrosshair(hitCrosshair, null, pos, normal);
        }
    }
}
