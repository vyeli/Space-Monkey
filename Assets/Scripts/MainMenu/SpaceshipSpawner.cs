using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] spaceships;
    [SerializeField] private float minDelay;
    [SerializeField] private float maxDelay;
    [SerializeField] private float speed;
    [SerializeField] private Vector3 rotate;

    private float nextTimeToSpawn;
    private GameObject _currentSpaceship;
    private bool _hasTimePassed;

    void Start()
    {
        nextTimeToSpawn = Time.time + Random.Range(minDelay, maxDelay);
    }


    void Update()
    {
        _hasTimePassed = Time.time > nextTimeToSpawn;
        if (!_hasTimePassed && _currentSpaceship != null) _currentSpaceship.GetComponent<SpaceshipController>().Move(Vector3.down);
        else if (_hasTimePassed)
        {
            nextTimeToSpawn = Time.time + Random.Range(minDelay, maxDelay);
            GameObject spaceship = Instantiate(spaceships[Random.Range(0, spaceships.Length)], transform.position, Quaternion.identity);
            spaceship.transform.Rotate(rotate);
            spaceship.AddComponent<SpaceshipController>();
            spaceship.GetComponent<SpaceshipController>().Speed = speed;
            _currentSpaceship = spaceship;
        }
        
    }
}

public class SpaceshipController : MonoBehaviour, IMoveable
{
    public float Speed { get; set; }

    public void Move(Vector3 direction)
    {
        transform.Translate(direction * Speed * Time.deltaTime);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}