using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player), typeof(CharacterController))]
public class MovementController : MonoBehaviour, IMoveable, IJumpable
{
    #region I_PROPERTIES
    public float Speed => GetComponent<Player>().PlayerStats.MovementSpeed;
    public float JumpForce => GetComponent<Player>().PlayerStats.JumpForce;
    #endregion

    [SerializeField] private Camera _playerCamera;
    [SerializeField] private GameObject _playerModel;
    [SerializeField] private float _gravityScale;
    private float _ySpeed;
    private CharacterController _characterController => GetComponent<CharacterController>();
    
    public void Move(Vector3 direction)
    {
        Vector3 cameraDirection = _playerCamera.transform.TransformDirection(direction);
        cameraDirection.Normalize();
        cameraDirection.y = 0;
        _characterController.Move(cameraDirection * Time.deltaTime * Speed);
        transform.rotation = Quaternion.Euler(0f, _playerCamera.transform.rotation.eulerAngles.y, 0f);
        _playerModel.transform.rotation = Quaternion.Slerp(_playerModel.transform.rotation, Quaternion.LookRotation(cameraDirection), Speed * Time.deltaTime);
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

    public bool IsGrounded()
    {
        return _characterController.isGrounded;
    }
}
