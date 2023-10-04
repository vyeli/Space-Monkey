using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementController))]
public class Player : Actor
{
    public static Player instance;

    // knockback
    public bool isKnocking;
    public float knockBackLength = .5f;
    private float knockBackCounter;
    public Vector2 knockBackPower;

    // public objects
    private Vector3 moveDirection;
    public Animator animator;
    
    public GameObject[] playerPieces;
    [SerializeField] private Gun _gun;
    public PlayerStats PlayerStats => _playerStats;
    public override EntityStats EntityStats => PlayerStats;

    [SerializeField] private PlayerStats _playerStats;
    private MovementController _movementController;

    #region ACTION_KEYS
    [SerializeField] private KeyCode _moveForwardKey = KeyCode.W;
    [SerializeField] private KeyCode _moveBackwardKey = KeyCode.S;
    [SerializeField] private KeyCode _moveRightKey = KeyCode.D;
    [SerializeField] private KeyCode _moveLeftKey = KeyCode.A;
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode _shootKey = KeyCode.Mouse0;
    #endregion

    #region MOVEMENT_COMMAND
    private CmdMovement _cmdMoveForward;
    private CmdMovement _cmdMoveBackward;
    private CmdMovement _cmdMoveRight;
    private CmdMovement _cmdMoveLeft;
    private CmdMovement _cmdJump;
    private CmdShoot _cmdShoot;

    private void InitMovementCommands()
    {
        _cmdMoveForward = new CmdMovement(_movementController, Vector3.forward);
        _cmdMoveBackward = new CmdMovement(_movementController, Vector3.back);
        _cmdMoveRight = new CmdMovement(_movementController, Vector3.right);
        _cmdMoveLeft = new CmdMovement(_movementController, Vector3.left);
        _cmdJump = new CmdMovement(_movementController, Vector3.up);
    }
    #endregion

    #region UNITY_EVENTS

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    
    protected override void Start()
    {
        base.Start();

        _movementController = GetComponent<MovementController>();
        InitMovementCommands();
        EventsManager.instance.CharacterLifeChange(_life);

        _cmdShoot = new CmdShoot(_gun);
    }

    
    void Update()
    {
        if (!isKnocking)
        {
            if (Input.GetKey(_moveForwardKey)) EventQueueManager.instance.AddCommand(_cmdMoveForward);
            if (Input.GetKey(_moveBackwardKey)) EventQueueManager.instance.AddCommand(_cmdMoveBackward);
            if (Input.GetKey(_moveRightKey)) EventQueueManager.instance.AddCommand(_cmdMoveRight);
            if (Input.GetKey(_moveLeftKey)) EventQueueManager.instance.AddCommand(_cmdMoveLeft);

            if (Input.GetKey(_jumpKey) && _movementController.IsGrounded()) _movementController.Jump();
            else _movementController.UpdateYSpeed();

            float horizontalMovement = Input.GetAxisRaw("Horizontal");
            float verticalMovement = Input.GetAxisRaw("Vertical");
            moveDirection = new Vector3(horizontalMovement, 0f, verticalMovement).normalized;
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
            */
        }
        /*
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
        */
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
        animator.SetBool("Grounded", _movementController.IsGrounded());
    }
    #endregion

    public void KnockBack()
    {
        isKnocking = true;
        knockBackCounter = knockBackLength;

        moveDirection.y = knockBackPower.y;
    }
}
