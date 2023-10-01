using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
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
    // private Rigidbody _rigidbody => GetComponent<Player>().Rigidbody;
    private CharacterController _characterController => GetComponent<Player>().CharacterController;
    
    public void Move(Vector3 direction)
    {
        Vector3 cameraDirection = _playerCamera.transform.TransformDirection(direction);
        cameraDirection.y = 0;
        _characterController.Move(cameraDirection * Time.deltaTime * Speed);
        transform.rotation = Quaternion.Euler(0f, _playerCamera.transform.rotation.eulerAngles.y, 0f);
        _playerModel.transform.rotation = Quaternion.Slerp(_playerModel.transform.rotation, Quaternion.LookRotation(cameraDirection), Speed * Time.deltaTime);
        /*
        Vector3 cameraDirection = _playerCamera.transform.TransformDirection(direction);
        cameraDirection.y = 0;
        transform.position += cameraDirection * Time.deltaTime * Speed;
        transform.rotation = Quaternion.Euler(0, _playerCamera.transform.eulerAngles.y, 0);
        _playerModel.transform.rotation = Quaternion.Slerp(_playerModel.transform.rotation, Quaternion.LookRotation(cameraDirection), Speed * Time.deltaTime);
        */
    }

    /*
    public void Jump()
    {
        _rigidbody.AddForce(Vector3.up * Mathf.Sqrt(-JumpForce * Physics.gravity.y), ForceMode.VelocityChange);
    }
    */

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
        // return Physics.CheckSphere(transform.position, 0.1f, LayerMask.GetMask("Ground"));
    }
}
