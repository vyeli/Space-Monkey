using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementController))]
public class Player : Actor
{
    public static Player instance;

    private Vector3 rayHit;

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

        EventsManager.instance.OnGameOver += OnGameOver;

        _movementController = GetComponent<MovementController>();
        InitCommands();
        UiManager.instance.UpdateCharacterLife(_life);
        EventsManager.instance.CharacterLifeChange(_life);
    }

    
    void Update()
    {
        if (GameManager.instance.GameEnded) return;
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

        if (Input.GetKeyDown(_shootKey) && !animator.GetBool("Shoot"))
        {
            EventQueueManager.instance.AddCommand(_cmdShoot);
            animator.SetBool("Shoot", true);
        }
        else if (animator.GetBool("Shoot") && Input.GetKeyUp(_shootKey))
        {
            animator.SetBool("Shoot", false);
        }

        animator.SetFloat("Speed", Mathf.Abs(moveDirection.x) + Mathf.Abs(moveDirection.z));
        animator.SetBool("Grounded", IsGrounded());
    }
    #endregion


    bool IsGrounded()
    {
        Debug.DrawRay(transform.position + Vector3.up, Vector3.down * (_distanceToGround + 1), Color.red);
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down * (_distanceToGround + 1), out var hit, _distanceToGround + 1))
        {
            rayHit = hit.point;
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Killzone"))
            {
                GameManager.instance.RespawnPlayer();
            }
            return true;
        }
        return false;
    }

    public override void TakeDamage(int damage)
    {
        if (GameManager.instance.GameEnded)
        {
            if (GameManager.instance.PlayerWon()) EnterKnockBack();
            return;
        }
        base.TakeDamage(damage);
        EnterKnockBack();
        EventsManager.instance.PlayerDamaged();
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

    public override void DieEffect()
    {
        // animator.SetTrigger("Death");
        EventsManager.instance.EventGameOver(false);
    }

    public void OnGameOver(bool _isVictory)
    {
        animator.SetFloat("Speed", 0);      // Reset speed animation
        if (_isVictory)
        {
            animator.SetTrigger("Victory");
        }
        else
        {
            animator.SetTrigger("Death");
        }
    }

}