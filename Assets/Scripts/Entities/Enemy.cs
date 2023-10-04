using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : Actor
{
    public override EntityStats EntityStats => _entityStats;
    [SerializeField] private EntityStats _entityStats;
    [SerializeField] private Transform[] _patrolPoints;
    private int _currentPatrolPoint;
    private AIState _currentState;
    private float _currentActionTime;

    #region ENEMY_STATS
    [SerializeField] private float _speed;
    [SerializeField] private float _idleWaitTime;
    [SerializeField] private Animator _animator;
    private NavMeshAgent _agent;
    #endregion

    private enum AIState
    {
        Idle,
        Patrol,
        Chase,
        Attack,
    }
    
    #region UNITY_EVENTS
    protected override void Start()
    {
        base.Start();
        _agent = GetComponent<NavMeshAgent>();
        _currentState = AIState.Patrol;
        _currentPatrolPoint = 0;
    }

    void Update()
    {
        switch(_currentState)
        {
            case AIState.Idle:
                Idle();
                break;
            case AIState.Patrol:
                Patrol();
                break;
            case AIState.Chase:
                Chase();
                break;
            case AIState.Attack:
                Attack();
                break;
            default:
                break;
        }
    }
    #endregion

    public void Idle()
    {
        _currentActionTime += Time.deltaTime;
        if (_currentActionTime > _idleWaitTime)
        {
            _currentActionTime = 0;
            _currentState = AIState.Patrol;
        }
    }

    public void Patrol()
    {
        _agent.SetDestination(_patrolPoints[_currentPatrolPoint].position);
        if (_agent.remainingDistance <= .2f)
        {
            _currentPatrolPoint = (_currentPatrolPoint + 1) % _patrolPoints.Length;
            _currentState = AIState.Idle;
        }
    }

    public void Chase()
    {

    }

    public void Attack()
    {

    }
}