using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums;

public class TurretEnemy : StaticEnemy
{
    [SerializeField] private Turret turret;
    [SerializeField] private GameObject _turretHead;

    protected override void Update()
    {
        base.Update();
    }

    protected override void AttackAction()
    {
        turret.Shoot();
    }

    protected override void UpdateAttack()
    {
        base.UpdateAttack();
        _turretHead.transform.LookAt(Player.instance.transform, Vector3.up);
    }

    public override void DieEffect()
    {
        EventsManager.instance.PlayerKill(StaticEnemyStats.Score);
        _currentState = AIState.Dead;
        _currentActionTime = 0;
    }

}