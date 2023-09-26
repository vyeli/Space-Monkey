using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    private void Awake()
    {
       instance = this;
    }


    // public variables
    public float moveSpeed;
    public float jumpForce;
    public float gravityScale = 5f;
    public float rotateSpeed = 5f;

    // knockback
    public bool isKocking;
    public float knockBackLength = .5f;
    private float knockBackCounter;
    public Vector2 knockBackPower;

    // public objects
    private Vector3 moveDirection;
    public CharacterController charController;
    public Camera playerCamera;
    public GameObject playerModel;
    public Animator animator;
    
    public GameObject[] playerPieces;
    public GameObject bulletPrefab;
    public GameObject bulletExit;

    #region UNITY_EVENTS
    
    void Start()
    {
        
    }

    
    void Update()
    {
        if (!isKocking)
        {
            float yStore = moveDirection.y;

            // move
            moveDirection = (transform.forward * Input.GetAxisRaw("Vertical")) + (transform.right * Input.GetAxisRaw("Horizontal"));
            moveDirection.Normalize();
            moveDirection *= moveSpeed;

            moveDirection.y = yStore;

            // jump
            if (charController.isGrounded)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    moveDirection.y = jumpForce;
                }
            }
          
            moveDirection.y += Physics.gravity.y * Time.deltaTime * gravityScale;
            

            charController.Move(moveDirection * Time.deltaTime);

            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) // if we are moving
            {
                transform.rotation = Quaternion.Euler(0f, playerCamera.transform.rotation.eulerAngles.y, 0f); // rotate the player to the camera's rotation
                Vector3 lookRotation = new Vector3(moveDirection.x, 0f, moveDirection.z); // get the direction we are moving
                Quaternion newRotation = new Quaternion(lookRotation.x, lookRotation.y, lookRotation.z, 0f); // create a quaternion from the direction we are moving
                if (lookRotation != Vector3.zero) // if we are moving
                {
                    newRotation = Quaternion.LookRotation(lookRotation); // rotate the player model to the direction we are moving
                }

                playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, rotateSpeed * Time.deltaTime);
            }
        }
        else
        {
            knockBackCounter -= Time.deltaTime;

            float yStore = moveDirection.y;
            moveDirection = playerModel.transform.forward * -knockBackPower.x;
            moveDirection.y = yStore;

            moveDirection.y += Physics.gravity.y * Time.deltaTime * gravityScale;

            charController.Move(moveDirection * Time.deltaTime);

            if (knockBackCounter <= 0)
            {
                isKocking = false;
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }


        animator.SetFloat("Speed", Mathf.Abs(moveDirection.x) + Mathf.Abs(moveDirection.z));
        animator.SetBool("Grounded", charController.isGrounded);
    }
    #endregion

    public void KnockBack()
    {
        isKocking = true;
        knockBackCounter = knockBackLength;

        moveDirection.y = knockBackPower.y;
    }

    private void Shoot() => Instantiate(bulletPrefab, bulletExit.transform.position, bulletExit.transform.rotation);
}
