using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums;

public class StaticEnemy : Actor
{
    public override EntityStats EntityStats => _staticEnemyStats;
    public virtual StaticEnemyStats StaticEnemyStats => _staticEnemyStats;
    protected AIState CurrentState => _currentState;
    protected float CurrentActionTime => _currentActionTime;
    [SerializeField] private StaticEnemyStats _staticEnemyStats;
    [SerializeField] private AudioClip _deathSound;
    [SerializeField] protected Animator _animator;
    
    public AIState _previousState;
    public AIState _currentState;
    protected float _currentActionTime = 0;
    protected float _distanceToPlayer;

    #region UNITY_EVENTS

    protected override void Start()
    {
        base.Start();
        _currentState = AIState.Idle;
        // _previousState = _currentState;
    }

    private void HurtBox_OnTriggerEnterEvent(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player.instance.TakeDamage(StaticEnemyStats.Damage);
        }
    }

    protected virtual void Update()
    {
        UpdateAlertState();
        
        switch (_currentState)
        {
            case AIState.Idle:
                UpdateIdle();
                break;
            case AIState.Attack:
                UpdateAttack();
                break;
            case AIState.Dead:
                WaitForDespawn();
                break;
        }
    }

    protected void UpdateAlertState()
    {
        if (_currentState != AIState.Dead)
        {
            _distanceToPlayer = Vector3.Distance(transform.position, Player.instance.transform.position);

            if (HasActiveAlert()) AlertAction();
        }
        
        // if (_previousState != _currentState)
        // {
        //     _previousState = _currentState;
        //     _currentActionTime = 0;
        // }
    }

    protected virtual void AlertAction()
    {
        StartAttack();
    }

    protected virtual bool HasActiveAlert()
    {
        return _distanceToPlayer < StaticEnemyStats.AttackRange;
    }

    #endregion

    protected virtual void StartIdle()
    {
        _currentState = AIState.Idle;
    }

    protected void UpdateIdle()
    {
        _currentActionTime += Time.deltaTime;
        if (_currentActionTime > StaticEnemyStats.IdleWaitTime)
        {
            AfterIdle();
        }
    }

    protected virtual void AfterIdle()
    {
        if(!HasActiveAlert()) StartIdle();
        else StartAttack();
    }

    protected void StartAttack()
    {
        transform.LookAt(Player.instance.transform, Vector3.up);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        _currentState = AIState.Attack;
        // _currentActionTime = StaticEnemyStats.AttackCooldownTime;
    }

    protected virtual void UpdateAttack()
    {
        _currentActionTime += Time.deltaTime;

        if (_currentActionTime > StaticEnemyStats.AttackCooldownTime)
        {
            if (_distanceToPlayer < StaticEnemyStats.AttackRange)
            {
                AttackAction();
            }
            else
            {
                StartIdle();
            }

            _currentActionTime = 0;
        }
    }

    protected virtual void AttackAction()
    {
        _animator.SetTrigger("Attack");
    }

    public bool CanAttack()
    {
        return _currentState == AIState.Attack && _currentActionTime > StaticEnemyStats.AttackCooldownTime;
    }

    public void Attack()
    {
        Player.instance.TakeDamage(StaticEnemyStats.Damage);
    }

    public override void DieEffect()
    {
        SoundManager.instance.PlaySFX(_deathSound);
        EventsManager.instance.PlayerKill(StaticEnemyStats.Score);
        _animator.SetBool("IsDead", true);
        _currentState = AIState.Dead;
        _currentActionTime = 0;
    }

    protected void WaitForDespawn()
    {
        _currentActionTime += Time.deltaTime;

        if (_currentActionTime > StaticEnemyStats.DespawnCooldownTime)
        {
            Destroy(gameObject);
        }
    }

    // Para visualizar rangos en el editor
    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, StaticEnemyStats.AttackRange);
    }
}
