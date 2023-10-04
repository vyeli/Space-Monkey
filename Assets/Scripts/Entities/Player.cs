using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementController))]
public class Player : Actor
{
    public static Player instance;

    public float distanceToGround = 0.4f;

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
    private CmdMovement _cmdMove;
    private CmdMovement _cmdJump;

    private CmdShoot _cmdShoot;

    private void InitMovementCommands()
    {
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

            moveDirection = (transform.forward * Input.GetAxisRaw("Vertical")) + (transform.right * Input.GetAxisRaw("Horizontal"));
            moveDirection.Normalize();

            _cmdMove = new CmdMovement(_movementController, moveDirection);
            EventQueueManager.instance.AddCommand(_cmdMove);

            Debug.Log("Player is grounded: " + IsGrounded());

            if (Input.GetKey(_jumpKey) && IsGrounded()) _movementController.Jump();
            else if (!IsGrounded()) _movementController.UpdateYSpeed();

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
            animator.SetBool("Grounded", IsGrounded());
        }
    }
    #endregion


    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, distanceToGround);
    }
}