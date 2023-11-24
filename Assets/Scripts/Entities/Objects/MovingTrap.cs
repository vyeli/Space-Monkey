using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTrap : Trap
{
    public float Speed => _movingTrapStats.MovingSpeed;
    [SerializeField] private Vector3 _moveDirection;
    [SerializeField] private float _moveDistance;
    private MovingTrapStats _movingTrapStats;

    void Start()
    {
        _movingTrapStats = (MovingTrapStats)_trapStats;
    }

    void Update()
    {
        // Move(Mathf.Sin(2 * Mathf.PI * Time.time * Speed) * _moveDistance * _moveDirection);
        Move(Mathf.Sin(Time.time * Speed) * _moveDistance/2 * _moveDirection * Speed);
        transform.Rotate(_movingTrapStats.RotationSpeed * Time.deltaTime * _moveDirection);
    }

    public void Move(Vector3 direction)
    {
        transform.position += direction * Time.deltaTime;
    }
}