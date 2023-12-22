using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Enums;

[RequireComponent(typeof(NavMeshAgent))]
public class MovingEnemy : StaticEnemy
{
    public override EntityStats EntityStats => _enemyStats;
    public override StaticEnemyStats StaticEnemyStats => _enemyStats;
    [SerializeField] protected EnemyStats _enemyStats;
    [SerializeField] private Transform[] _patrolPoints;
    private NavMeshAgent _agent;
    private int _currentPatrolPoint;
    

    #region UNITY_EVENTS

    protected override void Start()
    {
        base.Start();
        InitializeAgent();
        _currentPatrolPoint = 0;
    }

    private void InitializeAgent()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = _enemyStats.Speed;
        _agent.stoppingDistance = _enemyStats.PatrolPointStoppingDistance;
    }

    protected override void Update()
    {
        base.UpdateAlertState();

        if (_currentState != AIState.Dead && _animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            _agent.velocity = Vector3.zero;
        }

        switch (_currentState)
        {
            case AIState.Idle:
                base.UpdateIdle();
                break;
            case AIState.Patrol:
                UpdatePatrol();
                break;
            case AIState.Chase:
                UpdateChase();
                break;
            case AIState.Attack:
                base.UpdateAttack();
                break;
            case AIState.Dead:
                base.WaitForDespawn();
                break;
        }
    }

    protected override void AlertAction()
    {
        StartChase();
    }

    protected override bool HasActiveAlert()
    {
        return _distanceToPlayer > StaticEnemyStats.AttackRange && _distanceToPlayer < _enemyStats.ChaseRange;
    }

    #endregion
    protected override void AfterIdle()
    {
        StartPatrol();
    }

    protected override void StartIdle()
    {
        base.StartIdle();
        _agent.isStopped = false;
        _agent.SetDestination(transform.position);
    }

    private void StartPatrol()
    {
        _animator.SetBool("IsMoving", true);
        _currentState = AIState.Patrol;
        _agent.SetDestination(_patrolPoints[_currentPatrolPoint].position);
        _currentActionTime = 0;
    }

    private void UpdatePatrol()
    {
        if (!_agent.pathPending && _agent.remainingDistance < _agent.stoppingDistance)
        {
            _animator.SetBool("IsMoving", false);
            base.StartIdle();
            _currentPatrolPoint = (_currentPatrolPoint + 1) % _patrolPoints.Length;
        }
    }

    private void StartChase()
    {
        _animator.SetBool("IsMoving", true);
        _currentState = AIState.Chase;
        _agent.isStopped = false;
    }

    private void UpdateChase()
    {
        if (_distanceToPlayer > _enemyStats.AttackRange && _distanceToPlayer < _enemyStats.ChaseRange)
            _agent.SetDestination(Player.instance.transform.position);
        
        if (_distanceToPlayer < _enemyStats.AttackRange)
        {
            _animator.SetBool("IsMoving", false);
            base.StartAttack();
            _agent.isStopped = true;
            _agent.SetDestination(transform.position);
            _currentActionTime = _enemyStats.AttackCooldownTime;
        }

        if (_distanceToPlayer > _enemyStats.ChaseRange)
        {
            _animator.SetBool("IsMoving", false);
            base.StartIdle();
        }
    }

    protected override void AttackAction()
    {
        base.AttackAction();
    }

    public override void DieEffect()
    {
        base.DieEffect();
        _agent.isStopped = true;
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _enemyStats.ChaseRange);
    }
}
