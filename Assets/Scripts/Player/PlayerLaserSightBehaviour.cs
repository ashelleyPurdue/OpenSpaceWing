using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class PlayerLaserSightBehaviour : MonoBehaviour
{
    //Constants
    public const float FORWARD_OFFSET = 0.01f;


    //References
    public PlayerGunBehaviour gun;
    public Transform crosshair;


    //Private fields
    private LineRenderer line;

    
    //Events
    void Awake()
    {
        //Get the line renderer
        line = GetComponent<LineRenderer>();

        //Un-parent the crosshair object
        crosshair.SetParent(null);
    }

    void Update()
    {
        //Draw a line to the target
        line.SetPosition(0, transform.position);
        line.SetPosition(1, gun.AimPoint);

        //Find the point where the bullet would *actually* hit when fired.
        Vector3 bulletDir = (gun.AimPoint - gun.transform.position).normalized;
        RaycastHit[] hits = Physics.RaycastAll(gun.transform.position, bulletDir, Vector3.Distance(gun.AimPoint, gun.transform.position));

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
            crosshair.forward = closestHit.normal * -1;
            crosshair.position = closestHit.point + closestHit.normal * FORWARD_OFFSET;
        }
        else
        {
            //If no point was found(IE: player aiming off into the distance), use the aim point
            crosshair.forward = gun.AimPointNormal * -1;
            crosshair.position = gun.AimPoint + gun.AimPointNormal * FORWARD_OFFSET;
        }
    }

    void OnApplicationFocus(bool status)
    {
        Cursor.visible = !status;
    }
}
