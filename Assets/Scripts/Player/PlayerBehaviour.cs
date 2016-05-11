using UnityEngine;
using System.Collections;

public class PlayerBehaviour : MonoBehaviour
{
    public const float OFFICIAL_FOV = 60f;           //The "official" FOV, used for calculating movement bounds
    public const float OFFICIAL_ASPECT = 16f / 9f;   //The "official" aspect ratio, used for calculating movement bounds.

    public PlayerGunBehaviour gun;
    public Transform model;

    private float xWidth;
    private float yWidth;

    private float strafeSpeed = 10;
    private float rotSpeed = 90f;


    //Events

    void Awake()
    {
        //Calculate the min/max x and y
        yWidth = transform.localPosition.z * Mathf.Tan(OFFICIAL_FOV * 0.5f * Mathf.Deg2Rad);
        xWidth = yWidth * OFFICIAL_ASPECT;
    }

    void Update()
    {
        MovementControls();
        Rotate();

        //Quit when pressing esc
        if (Input.GetButtonDown("Cancel"))
        {
            Application.Quit();
            Cursor.visible = true;
        }
    }


    //Misc methods

    private void MovementControls()
    {
        //Find the new position
        Vector3 pos = transform.localPosition;

        Vector3 dir = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        float percent = dir.magnitude / Mathf.Sqrt(2);
        pos += dir.normalized * percent * strafeSpeed * Time.deltaTime;

        //Keep the ship in-bounds
        if (Mathf.Abs(pos.x) > xWidth)
        {
            pos.x = xWidth * Mathf.Sign(pos.x);
        }

        if (Mathf.Abs(pos.y) > yWidth)
        {
            pos.y = yWidth * Mathf.Sign(pos.y);
        }

        //Update the position
        transform.localPosition = pos;
    }

    private void Rotate()
    {
        //Get the direction the ship's movement wants to point
        Vector3 moveDir = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        moveDir.Normalize();

        //Get the direction the gun wants the ship to point
        Vector3 gunDir = (gun.AimPoint - transform.position).normalized;

        //Average the two directions to get the total direction
        Vector3 totalTargetDir = ((moveDir + gunDir) / 2).normalized;
        Quaternion targetRot = Quaternion.LookRotation(totalTargetDir);

        //Rotate towards the target direction
        model.rotation = Quaternion.RotateTowards(model.rotation, targetRot, rotSpeed * Time.deltaTime);
    }
}
