using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    private float lastPlayerZ;

    //Events

    void Awake()
    {
        nextAsteroidZ = player.position.z + Random.Range(minInterval, maxInterval);
        lastPlayerZ = player.position.z;
    }

    void FixedUpdate()
    {
        //Reset the next asteroid z if the player moved backwards
        if (player.position.z < lastPlayerZ)
        {
            nextAsteroidZ = player.position.z + Random.Range(minInterval, maxInterval);

            //Destroy all asteroids
            GameObject[] allObjs = GameObject.FindObjectsOfType<GameObject>();
            foreach (GameObject go in allObjs)
            {
                if (go.GetComponent<AsteroidBehaviour>() != null)
                {
                    GameObject.Destroy(go);
                }
            }

        }
        lastPlayerZ = player.position.z;

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
