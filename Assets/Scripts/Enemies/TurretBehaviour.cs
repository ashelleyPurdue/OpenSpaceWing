using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurretBehaviour : MonoBehaviour
{
    private float activationRange = 50;

    private int bulletsPerRound = 3;
    private float bulletDelay = 0.2f;
    private float delayAfterRound = 4f;

    private Transform playerRig;
    private Transform player;


    //Events

    void Awake()
    {
        //Get the player and the rig
        playerRig = GameObject.FindObjectOfType<PlayerRigBehaviour>().transform;
        player = GameObject.FindObjectOfType<PlayerBehaviour>().transform;

        //Start the co-routine
        StartCoroutine(BehaviourLoop());
    }


    //Coroutines

    private IEnumerator BehaviourLoop()
    {
        //Wait for the player to be in range
        while (true)
        {
            if (Vector3.Distance(transform.position, playerRig.position) <= activationRange)
            {
                break;
            }

            //Yield
            yield return null;
        }

        //Keep firing at the player while we're in the camera
        while (InCamera())
        {
            //Fire bullets
            for (int i = 0; i < bulletsPerRound; i++)
            {
                //TODO: Fire the bullet
                Debug.Log("Firing bullet " + i);

                //Wait the delay
                yield return new WaitForSeconds(bulletDelay);
            }

            //Wait until the cooldown is up
            yield return new WaitForSeconds(delayAfterRound);
        }
    }


    //Misc methods

    private bool InCamera()
    {
        //Returns if this object is in the camera

        Vector2 viewportPoint = Camera.main.WorldToViewportPoint(transform.position);
        return (viewportPoint.x >= 0 && viewportPoint.x <= 1) && (viewportPoint.y >= 0 && viewportPoint.y <= 1);
    }
}
