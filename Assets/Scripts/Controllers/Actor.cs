using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actor : MonoBehaviour, IDamageable
{

    #region IDAMAGEABLE_PROPERTIES
    public int MaxLife => _maxLife;
    public int Life => _life;
    #endregion

    #region PRIVATE_PROPERTIES
    [SerializeField] private int _maxLife = 5;
    [SerializeField] private int _life;
    #endregion

    #region UNITY_EVENTS
    public void Start()
    {
        _life = _maxLife;
    }
    public void Update()
    {
        
    }
    #endregion

    #region IDAMAGEABLE_METHODS
    public void TakeDamage(int damage)
    {
        _life -= damage;
        if (_life <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        Destroy(gameObject);
    }
    #endregion

}