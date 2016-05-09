using UnityEngine;
using System.Collections;

public class InfiniteAsteroidSpawnerBehaviour : MonoBehaviour
{
    public Transform player;

    public float spawnDistance = 100;   //How far away from the player the asteroids should spawn

    public float minX = -20;
    public float maxX = 20;

    public float minY = -20;
    public float maxY = 20;

    public float minScale = 1;
    public float maxScale = 2;

    public float minInterval = 5;
    public float maxInterval = 20;


    //Private fields
    private float nextAsteroidZ;


    //Events

    void Awake()
    {
        nextAsteroidZ = player.position.z + Random.Range(minInterval, maxInterval);
    }

    void FixedUpdate()
    {
        //Create an asteroid when the player crosses the current threshold
        if (player.position.z >= nextAsteroidZ)
        {
            //Advance the next Z
            nextAsteroidZ += Random.Range(minInterval, maxInterval);

            //Select a random position to put the asteroid
            Vector3 newPos = Vector3.zero;
            newPos.x = Random.Range(minX, maxX);
            newPos.y = Random.Range(minY, maxY);
            newPos.z = player.position.z + spawnDistance;

            //Create the asteroid
            GameObject asteroid = Instantiate<GameObject>(Resources.Load<GameObject>("asteroid_prefab"));
            asteroid.transform.position = newPos;
            asteroid.transform.localScale = Vector3.one * Random.Range(minScale, maxScale);
        }
    }
}
