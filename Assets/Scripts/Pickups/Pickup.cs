using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class Pickup : MonoBehaviour
{
    [SerializeField] protected PickupStats _pickupStats;
    private Vector3 _position;

    void Start()
    {
        _position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    void Update()
    {
        float newY = _position.y;
        newY += Mathf.Sin(Time.time * _pickupStats.FloatingSpeed) * _pickupStats.FloatingHeight;
        transform.position = new Vector3(_position.x, newY, _position.z);
        transform.Rotate(0, 0, Time.deltaTime * _pickupStats.RotationSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player picked up " + gameObject.name);
            OnPickup(other);
            PickupEffect();
            Destroy(gameObject);
        }
    }

    public abstract void OnPickup(Collider other);

    public abstract void PickupEffect();
}