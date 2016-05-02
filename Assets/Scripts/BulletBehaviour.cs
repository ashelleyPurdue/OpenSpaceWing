using UnityEngine;
using System.Collections;

public class BulletBehaviour : MonoBehaviour
{
    private float lifeTime = 5f;

    void FixedUpdate()
    {
        //Destroy when time is up
        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0)
        {
            GameObject.Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //Destroy self
        GameObject.Destroy(gameObject);
    }
}
