using UnityEngine;
using System.Collections;

public class BulletBehaviour : MonoBehaviour
{
    private float lifeTime = 5f;

    private Vector3 lastPos;
    private Rigidbody myRigidbody;


    //Events

    void Awake()
    {
        lastPos = transform.position;
        myRigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        //Sweep cast to see if we've hit anything
        CheckForCollision();

        //Destroy when time is up
        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0)
        {
            GameObject.Destroy(gameObject);
        }
    }


    //Misc methods

    private void CheckForCollision()
    {
        //Checks for a collision along the path we took

        //Get all objects we hit on the way
        Vector3 posDiff = lastPos - transform.position;
        RaycastHit[] hits = myRigidbody.SweepTestAll(posDiff.normalized, posDiff.magnitude, QueryTriggerInteraction.Collide);

        //Find the object we hit *last*(since the sweep test is going backwards)
        float furthestDist = float.MinValue;
        Collider other = null;
        Vector3 collisionPoint = Vector3.zero;

        foreach(RaycastHit hit in hits)
        {
            if (hit.distance > furthestDist)
            {
                furthestDist = hit.distance;
                other = hit.collider;
                collisionPoint = hit.point;
            }
        }

        //Perform a collision event if we hit anything
        if (other != null)
        {
            //Move to the point where we hit
            transform.position = collisionPoint;

            //Call the event
            OnCollision(other);
        }

        //Update the last pos
        lastPos = transform.position;
    }

    private void OnCollision(Collider other)
    {
        //Damage the other if it has healpoints
        HealthPoints otherHeath = other.GetComponent<HealthPoints>();
        if (otherHeath != null)
        {
            otherHeath.AttackFrom(GetComponent<DamageSource>());
        }

        //Destroy self
        GameObject.Destroy(gameObject);
    }
}
