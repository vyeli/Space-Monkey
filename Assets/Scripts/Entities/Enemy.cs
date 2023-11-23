using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Enums;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : Actor
{
    public override EntityStats EntityStats => _enemyStats;
    public AIState CurrentState => _currentState;
    public float CurrentActionTime => _currentActionTime;

    [SerializeField] private Transform[] _patrolPoints;
    [SerializeField] private EnemyStats _enemyStats;
    [SerializeField] private Animator _animator;

    private NavMeshAgent _agent;
    private AIState _currentState;
    private float _currentActionTime;
    private float _distanceToPlayer;
    private int _currentPatrolPoint;
    

    #region UNITY_EVENTS

    protected override void Start()
    {
        base.Start();
        InitializeAgent();
        _currentState = AIState.Idle;
        _currentPatrolPoint = 0;
    }

    private void InitializeAgent()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = _enemyStats.Speed;
        _agent.stoppingDistance = _enemyStats.PatrolPointStoppingDistance;
    }

    private void HurtBox_OnTriggerEnterEvent(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player.instance.TakeDamage(_enemyStats.Damage);
        }
    }

    void Update()
    {
        if (_currentState != AIState.Dead)
        {
            _distanceToPlayer = Vector3.Distance(transform.position, Player.instance.transform.position);

            if (_distanceToPlayer > _enemyStats.AttackRange && _distanceToPlayer < _enemyStats.ChaseRange)
            {
                StartChasing();
            }
        }

        switch (_currentState)
        {
            case AIState.Idle:
                UpdateIdle();
                break;
            case AIState.Patrol:
                UpdatePatrol();
                break;
            case AIState.Chase:
                UpdateChase();
                break;
            case AIState.Attack:
                UpdateAttack();
                break;
            case AIState.Dead:
                WaitForDespawn();
                break;
        }
    }

    #endregion

    private void StartChasing()
    {
        _animator.SetBool("IsMoving", true);
        _currentState = AIState.Chase;
        _agent.isStopped = false;
    }

    private void UpdateIdle()
    {
        _currentActionTime += Time.deltaTime;
        if (_currentActionTime > _enemyStats.IdleWaitTime)
        {
            StartPatrolling();
        }
    }

    private void StartPatrolling()
    {
        _animator.SetBool("IsMoving", true);
        _currentState = AIState.Patrol;
        _currentActionTime = 0;
    }

    private void UpdatePatrol()
    {
        _agent.SetDestination(_patrolPoints[_currentPatrolPoint].position);

        if (_agent.remainingDistance != 0 && _agent.remainingDistance < _agent.stoppingDistance)
        {
            _animator.SetBool("IsMoving", false);
            StartIdle();
            _currentPatrolPoint = (_currentPatrolPoint + 1) % _patrolPoints.Length;
        }
    }

    private void StartIdle()
    {
        _currentState = AIState.Idle;
        _agent.isStopped = false;
        _agent.SetDestination(transform.position);
    }

    private void UpdateChase()
    {
        _agent.SetDestination(Player.instance.transform.position);

        if (_distanceToPlayer < _enemyStats.AttackRange)
        {
            _animator.SetBool("IsMoving", false);
            StartAttacking();
            _agent.isStopped = true;
            _currentActionTime = _enemyStats.AttackCooldownTime;
        }

        if (_distanceToPlayer > _enemyStats.ChaseRange)
        {
            _animator.SetBool("IsMoving", false);
            StartIdle();
        }
    }

    private void StartAttacking()
    {
        transform.LookAt(Player.instance.transform, Vector3.up);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        _currentState = AIState.Attack;
    }

    private void UpdateAttack()
    {
        _currentActionTime += Time.deltaTime;

        if (_currentActionTime > _enemyStats.AttackCooldownTime)
        {
            if (_distanceToPlayer < _enemyStats.AttackRange)
            {
                _animator.SetTrigger("Attack");
            }
            else
            {
                StartIdle();
            }

            _currentActionTime = 0;
        }
    }

    public bool CanAttack()
    {
        return _currentState == AIState.Attack && _currentActionTime > _enemyStats.AttackCooldownTime;
    }

    public void Attack()
    {
        Player.instance.TakeDamage(_enemyStats.Damage);
    }

    public override void DieEffect()
    {
        EventsManager.instance.PlayerKill(_enemyStats.Score);
        _animator.SetBool("IsDead", true);
        _currentState = AIState.Dead;
        _agent.isStopped = true;
        _currentActionTime = 0;
    }

    private void WaitForDespawn()
    {
        _currentActionTime += Time.deltaTime;

        if (_currentActionTime > _enemyStats.DespawnCooldownTime)
        {
            Destroy(gameObject);
        }
    }

    // Para visualizar rangos en el editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _enemyStats.AttackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _enemyStats.ChaseRange);
    }
}
