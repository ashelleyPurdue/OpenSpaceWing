using UnityEngine;
using System.Collections;

public class PlayerBehaviour : MonoBehaviour
{
    public PlayerGunBehaviour gun;
    public Transform model;

    private float strafeSpeed = 10f;
    private float forwardSpeed = 4;
    private float rotSpeed = 90f;

    void Update()
    {
        MovementControls();
        Rotate();

        //Close game when pressing escape
        if (Input.GetButtonDown("Cancel"))
        {
            Application.Quit();
            Cursor.visible = true;
        }
    }

    //Misc methods

    private void MovementControls()
    {
        //Move
        Vector3 dir = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        transform.position += dir.normalized * strafeSpeed * Time.deltaTime;
    }

    private void Rotate()
    {
        //Get the direction the ship's movement wants to point
        Vector3 moveDir = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        moveDir.z = forwardSpeed;
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
