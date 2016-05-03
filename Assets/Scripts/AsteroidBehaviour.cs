using UnityEngine;
using System.Collections;

public class AsteroidBehaviour : MonoBehaviour
{
    public const float MIN_SIZE = 0.25f;
    public const float SPLIT_DISTANCE = 0.5f;

    void OnDead()
    {
        //If this asteroid was big enough, create two new asteroids.
        if (transform.localScale.x >= MIN_SIZE)
        {
            for (int i = 0; i < 2; i++)
            {
                //Create the child
                GameObject child = Instantiate(gameObject);

                //Change the child's position and scale
                child.transform.localScale = transform.localScale * 0.5f;
                child.transform.position = transform.position + Random.onUnitSphere * SPLIT_DISTANCE;
            }
        }

        //Destroy this asteroid
        GameObject.Destroy(gameObject);
    }
}
