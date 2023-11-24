using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums;

public class StaticEnemy : Enemy
{
    // Update is called once per frame
    protected override void Start()
    {
        base.Start();
        _currentState = AIState.Attack;
    }

    protected override void Update()
    {
        _currentState = AIState.Attack;
    }
}
