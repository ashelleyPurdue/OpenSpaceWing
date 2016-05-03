﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class PlayerLaserSightBehaviour : MonoBehaviour
{
    //Constants
    public const float FORWARD_OFFSET = 0.01f;


    //Global settings
    public static bool enableHitCrosshair = false;


    //References
    public PlayerGunBehaviour gun;
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
    }

    void Update()
    {
        //Draw a line to the target
        line.SetPosition(0, transform.position);
        line.SetPosition(1, gun.AimPoint);

        //Update the aim crosshair
        aimCrosshair.forward = gun.AimPointNormal * -1;
        aimCrosshair.position = gun.AimPoint + gun.AimPointNormal * FORWARD_OFFSET;

        //Update the hit crosshair
        hitCrosshair.GetComponent<SpriteRenderer>().enabled = enableHitCrosshair;
        UpdateHitCrosshair();
    }

    void OnApplicationFocus(bool status)
    {
        Cursor.visible = !status;
    }
    


    //Misc methods

    private void UpdateHitCrosshair()
    {
        //Moves the hit crosshair to the point where the bullet will *actually* hit.

        //Find the point where the bullet would *actually* hit when fired.
        Vector3 bulletDir = (gun.AimPoint - gun.transform.position).normalized;
        float aimpointDist = Vector3.Distance(gun.AimPoint, gun.transform.position);

        RaycastHit[] hits = Physics.RaycastAll(gun.transform.position, bulletDir, aimpointDist);

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
            hitCrosshair.forward = closestHit.normal * -1;
            hitCrosshair.position = closestHit.point + closestHit.normal * FORWARD_OFFSET;
        }
        else
        {
            //If no point was found(IE: player aiming off into the distance), go half-way between the gun and the aim point
            hitCrosshair.forward = (gun.AimPoint - gun.transform.position).normalized * -1;
            hitCrosshair.position = gun.transform.position + bulletDir * aimpointDist / 2;
        }
    }
}
