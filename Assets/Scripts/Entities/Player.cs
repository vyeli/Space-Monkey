using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{
    public static Player instance;

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
    public bool isKnocking;
    public float knockBackLength = .5f;
    private float knockBackCounter;
    public Vector2 knockBackPower;

    // public objects
    private Vector3 moveDirection;
    public Camera playerCamera;
    public GameObject playerModel;
    public Animator animator;
    
    public GameObject[] playerPieces;
    public GameObject bulletPrefab;
    public GameObject bulletExit;
    [SerializeField] private Transform _bulletContainer;
    [SerializeField] private Gun _gun;
    private MovementController _movementController;

    #region IMOVEABLE_PROPERTIES
    public float Speed => moveSpeed;
    #endregion

    #region ACTION_KEYS
    [SerializeField] private KeyCode _moveForwardKey = KeyCode.W;
    [SerializeField] private KeyCode _moveBackwardKey = KeyCode.S;
    [SerializeField] private KeyCode _moveRightKey = KeyCode.D;
    [SerializeField] private KeyCode _moveLeftKey = KeyCode.A;
    [SerializeField] private KeyCode _shootKey = KeyCode.Mouse0;
    #endregion

    #region MOVEMENT_COMMAND
    private CmdMovement _cmdMoveForward;
    private CmdMovement _cmdMoveBackward;
    private CmdMovement _cmdMoveRight;
    private CmdMovement _cmdMoveLeft;
    private CmdShoot _cmdShoot;

    private void InitMovementCommands()
    {
        _cmdMoveForward = new CmdMovement(_movementController, Vector3.forward);
        _cmdMoveBackward = new CmdMovement(_movementController, Vector3.back);
        _cmdMoveRight = new CmdMovement(_movementController, Vector3.right);
        _cmdMoveLeft = new CmdMovement(_movementController, Vector3.left);
    }
    #endregion

    #region UNITY_EVENTS
    
    new void Start()
    {
        base.Start();
        _movementController = GetComponent<MovementController>();
        InitMovementCommands();

        _cmdShoot = new CmdShoot(_gun);
    }

    
    new void Update()
    {
        base.Update();
        if(Input.GetKeyDown(KeyCode.Return)) EventsManager.instance.EventGameOver(true);
        if (Input.GetKeyDown(KeyCode.Backspace)) TakeDamage(1);
        if (!isKnocking)
        {
            if (Input.GetKey(_moveForwardKey)) EventQueueManager.instance.AddCommand(_cmdMoveForward);
            if (Input.GetKey(_moveBackwardKey)) EventQueueManager.instance.AddCommand(_cmdMoveBackward);
            if (Input.GetKey(_moveRightKey)) EventQueueManager.instance.AddCommand(_cmdMoveRight);
            if (Input.GetKey(_moveLeftKey)) EventQueueManager.instance.AddCommand(_cmdMoveLeft);

            float horizontalMovement = Input.GetAxisRaw("Horizontal");
            float verticalMovement = Input.GetAxisRaw("Vertical");
            moveDirection = new Vector3(horizontalMovement, 0f, verticalMovement);
            // Movement without strategy pattern
            /*
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
            */

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

            // charController.Move(moveDirection * Time.deltaTime);

            if (knockBackCounter <= 0)
            {
                isKnocking = false;
            }
        }
        if (Input.GetKey(_shootKey))
        {
            animator.SetBool("Shoot", true);
        }
        else if (animator.GetBool("Shoot") == true)
        {
            EventQueueManager.instance.AddCommand(_cmdShoot);
            animator.SetBool("Shoot", false);
        }

        animator.SetFloat("Speed", Mathf.Abs(moveDirection.x) + Mathf.Abs(moveDirection.z));
        animator.SetBool("Grounded", true);
        // animator.SetBool("Grounded", charController.isGrounded);
    }
    #endregion

    #region IMOVEABLE_METHODS

    #endregion

    public void KnockBack()
    {
        isKnocking = true;
        knockBackCounter = knockBackLength;

        moveDirection.y = knockBackPower.y;
    }
}
