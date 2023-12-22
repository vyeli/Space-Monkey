using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Trap : MonoBehaviour
{
    public virtual TrapStats TrapStats => _trapStats;
    [SerializeField] protected TrapStats _trapStats;
    protected Collider _collider;

    protected void Start()
    {
        _collider = GetComponent<Collider>();
        _collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) Player.instance.TakeDamage(_trapStats.Damage);
    }
}