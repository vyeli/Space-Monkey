using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Player), typeof(CharacterController))]
public class MovementController : MonoBehaviour, IMoveable, IJumpable
{
    #region I_PROPERTIES
    public float Speed => GetComponent<Player>().PlayerStats.MovementSpeed;
    public float RotationSpeed => GetComponent<Player>().PlayerStats.RotationSpeed;
    public float JumpForce => GetComponent<Player>().PlayerStats.JumpForce;
    #endregion

    [SerializeField] private Camera _playerCamera;
    [SerializeField] private GameObject _playerModel;
    [SerializeField] private float _gravityScale;
    private float _ySpeed;
    private CharacterController _characterController => GetComponent<CharacterController>();
    
    public void Move(Vector3 direction)
    {
        
        direction *= Speed;

        _characterController.Move(direction * Time.deltaTime);

        transform.rotation = Quaternion.Euler(0f, _playerCamera.transform.rotation.eulerAngles.y, 0f);
        Vector3 lookRotation = new Vector3(direction.x, 0f, direction.z);
        Quaternion newRotation = new Quaternion(lookRotation.x, lookRotation.y, lookRotation.z, 0f);
        if (lookRotation != Vector3.zero)
        {
            newRotation = Quaternion.LookRotation(lookRotation);
        }
        _playerModel.transform.rotation = Quaternion.Slerp(_playerModel.transform.rotation, newRotation, RotationSpeed * Time.deltaTime);
    }

    public void Jump()
    {
        _ySpeed = JumpForce;
        UpdateYSpeed();
    }

    public void UpdateYSpeed()
    {
        _ySpeed += Physics.gravity.y * Time.deltaTime * _gravityScale;
        _characterController.Move(Vector3.up * _ySpeed * Time.deltaTime);
    }
}
