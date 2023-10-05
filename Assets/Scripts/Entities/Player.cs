using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementController))]
public class Player : Actor
{
    public static Player instance;

    // knockback
    [SerializeField] private Vector2 _knockBackPower;
    [SerializeField] private float _knockBackLength;
    [SerializeField] private float _distanceToGround;
    [SerializeField] private GameObject[] _playerPieces;

    // public objects
    private Vector3 moveDirection;
    public Animator animator;
    [SerializeField] public Gun _gun;
    public PlayerStats PlayerStats => _playerStats;
    public override EntityStats EntityStats => PlayerStats;

    [SerializeField] private PlayerStats _playerStats;
    private MovementController _movementController;
    private bool _isKnocking;

    #region ACTION_KEYS
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode _shootKey = KeyCode.Mouse0;
    #endregion

    #region MOVEMENT_COMMAND
    private CmdMovement _cmdMove;
    private CmdMovement _cmdJump;

    private CmdShoot _cmdShoot;

    private void InitCommands()
    {
        _cmdJump = new CmdMovement(_movementController, Vector3.up);
        _cmdShoot = new CmdShoot(_gun);
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
        InitCommands();
        UiManager.instance.UpdateCharacterLife(_life);
        EventsManager.instance.CharacterLifeChange(_life);
    }

    
    void Update()
    {
        if (_isKnocking)
        {
            _movementController.Move(moveDirection);
            return;
        }

        moveDirection = (transform.forward * Input.GetAxisRaw("Vertical")) + (transform.right * Input.GetAxisRaw("Horizontal"));
        moveDirection.Normalize();

        if (moveDirection != Vector3.zero)
        {
            _cmdMove = new CmdMovement(_movementController, moveDirection);
            EventQueueManager.instance.AddCommand(_cmdMove);
        }

        if (Input.GetKey(_jumpKey) && IsGrounded()) _movementController.Jump();
        else if (!IsGrounded()) _movementController.UpdateYSpeed();

        if (Input.GetKey(_shootKey) && !animator.GetBool("Shoot"))
        {
            animator.SetBool("Shoot", true);
        }
        else if (animator.GetBool("Shoot") && Input.GetKeyUp(_shootKey))
        {
            EventQueueManager.instance.AddCommand(_cmdShoot);
            animator.SetBool("Shoot", false);
        }

        animator.SetFloat("Speed", Mathf.Abs(moveDirection.x) + Mathf.Abs(moveDirection.z));
        animator.SetBool("Grounded", IsGrounded());
    }
    #endregion


    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, _distanceToGround);
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        EnterKnockBack();
        EventsManager.instance.CharacterLifeChange(_life);
    }

    private void EnterKnockBack()
    {
        _isKnocking = true;
        animator.SetTrigger("Knocking");
        moveDirection = -transform.forward * _knockBackPower.x + Vector3.up * _knockBackPower.y;
        StartCoroutine(ExitKnockBack());
    }

    IEnumerator ExitKnockBack()
    {
        yield return new WaitForSeconds(_knockBackLength);
        _isKnocking = false;
    }

}