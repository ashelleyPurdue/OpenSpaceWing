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

        //Update the crosshair pos and direction
        crosshair.forward = -gun.AimPointNormal;
        crosshair.position = gun.AimPoint + gun.AimPointNormal * FORWARD_OFFSET;
    }

    void OnApplicationFocus(bool status)
    {
        Cursor.visible = !status;
    }
}
