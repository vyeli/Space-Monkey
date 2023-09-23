using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipSpawner : MonoBehaviour
{

    public GameObject[] spaceships;
    public float minDelay = 0f;
    public float maxDelay = 10f;
    public float speed = 50f;
    public float rotateX = 0f;
    public float rotateY = 0f;
    public float rotateZ = 0f;

    private float nextTimeToSpawn = 0f;

    void Start()
    {
        nextTimeToSpawn = Time.time + Random.Range(minDelay, maxDelay);
    }


    void Update()
    {
        if (Time.time > nextTimeToSpawn)
        {
            nextTimeToSpawn = Time.time + Random.Range(minDelay, maxDelay);
            GameObject newSpaceship = Instantiate(spaceships[Random.Range(0, spaceships.Length)], transform.position, Quaternion.identity);
            Transform spaceshipTransform = newSpaceship.GetComponent<Transform>();
            spaceshipTransform.Rotate(rotateX, rotateY, rotateZ);
            newSpaceship.AddComponent<Move>();
            newSpaceship.GetComponent<Move>().speed = speed;
        }
        
    }
}

public class Move : MonoBehaviour
{
    public float speed;

    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}