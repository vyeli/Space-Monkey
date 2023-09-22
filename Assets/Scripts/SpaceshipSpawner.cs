using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipSpawner : MonoBehaviour
{

    public GameObject spaceship1;
    public GameObject spaceship2;
    public GameObject spaceship3;
    public GameObject spaceship4;
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
            GameObject spaceship = null;
            switch(Random.Range(0, 4)) {
                case 0:
                    spaceship = spaceship1;
                    break;
                case 1:
                    spaceship = spaceship2;
                    break;
                case 2:
                    spaceship = spaceship3;
                    break;
                case 3:
                    spaceship = spaceship4;
                    break;
                default:
                    break;
            }
            GameObject newSpaceship = Instantiate(spaceship, transform.position, Quaternion.identity);
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