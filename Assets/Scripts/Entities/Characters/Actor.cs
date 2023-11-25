using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actor : MonoBehaviour, IDamageable
{

    #region IDAMAGEABLE_PROPERTIES
    public int MaxLife => EntityStats.MaxLife;
    public int Life { get => _life; set => _life = value; }
    #endregion

    #region PRIVATE_PROPERTIES
    public abstract EntityStats EntityStats { get; }

    protected int _life;
    #endregion

    #region UNITY_EVENTS
    protected virtual void Start()
    {
        _life = MaxLife;
    }
  
    #endregion

    #region IDAMAGEABLE_METHODS
    public virtual void TakeDamage(int damage)
    {
        _life -= damage;
        Debug.Log($"{name} has taken {damage} damage");
        if (_life <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log($"{name} has died");
        DieEffect();
        /*
        if (name.Equals("Player"))
        {
            EventsManager.instance.EventGameOver(false);
        }
        else
        {
            DieEffect();
        }
        */
    }
    public virtual void DieEffect()
    {
        Destroy(gameObject);
    }
    #endregion

}