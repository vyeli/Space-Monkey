using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CmdTakeDamage : ICommand
{
    private IDamageable _damageable;
    private int _damage;

    public CmdTakeDamage(IDamageable damageable, int damage)
    {
        _damageable = damageable;
        _damage = damage;
    }

    public void Execute()
    {
        _damageable.TakeDamage(_damage);
    }

    public void Undo() => throw new System.NotImplementedException();
}