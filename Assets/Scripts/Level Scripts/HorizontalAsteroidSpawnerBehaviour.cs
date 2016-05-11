using UnityEngine;
using System.Collections;

public class HorizontalAsteroidSpawnerBehaviour : MonoBehaviour
{
    public GameObject asteroidOriginal;
    public Transform player;

    private float maxDist = 100;
    private float minDist = 2;

    private float minSpeed = 1f;
    private float maxSpeed = 20f;

    private float createTime = 1f;
    private float createTimer = 0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Create an asteroid when the timer is up
        createTimer += Time.deltaTime;
        if (createTimer >= createTime)
        {
            createTimer = 0f;
            CreateAsteroid();
        }
	}

    //Misc methods

    private void CreateAsteroid()
    {
        //Create the asteroid
        Vector3 startPos = RandomStartPos(Random.Range(minDist, maxDist));
        GameObject asteroid = Instantiate<GameObject>(asteroidOriginal);
        asteroid.transform.position = startPos;

        //Give it velocity
        asteroid.GetComponent<Rigidbody>().AddForce(Random.Range(minSpeed, maxSpeed) * -1, 0, 0, ForceMode.VelocityChange);
    }

    private Vector3 RandomStartPos(float depth)
    {
        //TODO: Dynamically change min/max height
        return new Vector3(10, Random.Range(-10, 10), depth) + player.position;
    }
}
