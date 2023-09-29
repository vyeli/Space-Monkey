using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour, IMoveable
{
    #region IMOVEABLE_PROPERTIES
    public float Speed => _speed;
    #endregion

    #region PRIVATE_PROPERTIES
    [SerializeField] private float _speed = 10f;
    [SerializeField] private CharacterController characterController;
    #endregion
    
    public void Move(Vector3 direction)
    {
        direction.Normalize();
        direction.y = 0;
        characterController.Move(direction * Time.deltaTime * _speed);
    }
}
