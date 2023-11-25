using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingShootingEnemy : MovingEnemy
{
    [SerializeField] private Turret turret;

    protected override void AttackAction()
    {
        base.AttackAction();
        turret.Shoot();
    }
}
