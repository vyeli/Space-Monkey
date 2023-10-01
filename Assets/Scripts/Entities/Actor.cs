using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actor : MonoBehaviour, IDamageable
{

    #region IDAMAGEABLE_PROPERTIES
    public int MaxLife => _entityStats.MaxLife;
    public int Life => _life;
    #endregion

    #region PRIVATE_PROPERTIES
    public EntityStats EntityStats => _entityStats;
    private EntityStats _entityStats;
    [SerializeField] protected int _life;
    #endregion

    #region UNITY_EVENTS
    protected void Start()
    {
        _life = MaxLife;
    }
    protected void Update()
    {
        
    }
    #endregion

    #region IDAMAGEABLE_METHODS
    public void TakeDamage(int damage)
    {
        _life -= damage;
        Debug.Log($"{name} has taken {damage} damage");
        if (name.Equals("Player"))
        {
            UiManager.instance.healthText.text = _life.ToString();
        }
        if (_life <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        Debug.Log($"{name} has died");
        if (name.Equals("Player")) EventsManager.instance.EventGameOver(false);
        else Destroy(gameObject);
    }
    #endregion

}