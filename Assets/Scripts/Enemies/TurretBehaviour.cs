using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurretBehaviour : MonoBehaviour
{
    public Transform gun;

    private float activationRange = 50;

    private int bulletsPerRound = 3;
    private float bulletDelay = 0.2f;
    private float delayAfterRound = 4f;

    private float bulletVelocity = 20;

    private Transform playerRig;
    private Transform player;

    private Vector3 lastPlayerRigPos;


    //Events

    void Awake()
    {
        //Get the player and the rig
        playerRig = GameObject.FindObjectOfType<PlayerRigBehaviour>().transform;
        player = GameObject.FindObjectOfType<PlayerBehaviour>().transform;

        lastPlayerRigPos = playerRig.position;

        //Start the co-routine
        StartCoroutine(BehaviourLoop());
    }

    void Update()
    {
        //Compute the player rig's speed.
        float playerSpeed = Vector3.Distance(lastPlayerRigPos, playerRig.position) / Time.deltaTime;
        lastPlayerRigPos = playerRig.position;

        //Aim at the player
        gun.forward = PredictiveAim(1, playerSpeed);
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
                //Fire the bullet
                BulletBehaviour.Create(transform.position + gun.forward, gun.forward, bulletVelocity, GetComponent<DamageSource>());

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

    private Vector3 PredictiveAim(float bulletSpawnOffset, float playerSpeed)
    {
        //Aim at where the player is going to be.
        //Returns the forward vector.
        //TODO: Use math to do this, instead of brute force

        const float INCREMENT = 0.5f;

        //Find the zOffset that, when used, will cause the bullet to get as close as possible to the player
        Vector3 closestForward = Vector3.up;
        float closestDist = float.MaxValue;

        for (float offset = 0; offset <= 10; offset += INCREMENT)
        {
            //Find where the bullet is aiming for
            Vector3 aimPoint = player.position + playerRig.forward * offset;

            //Find the forward that points to the aimPoint
            Vector3 gunForward = (aimPoint - gun.transform.position).normalized;

            //Find the time it would take for the bullet to reach aimPoint
            Vector3 bulletSpawnPos = gun.position + gunForward * bulletSpawnOffset;
            float bulletTravelDist = Vector3.Distance(bulletSpawnPos, aimPoint);
            float time = bulletTravelDist / bulletVelocity;

            //Find where the player will be at that point in time.
            Vector3 predictedPlayerPos = player.position;
            predictedPlayerPos.z += playerSpeed * time;

            //Save this forward if it's the closest
            float forwardDist = Vector3.Distance(aimPoint, predictedPlayerPos);
            if (forwardDist < closestDist)
            {
                closestForward = gunForward;
                closestDist = forwardDist;
            }
        }

        //Return the forward
        return closestForward;
    }

}
