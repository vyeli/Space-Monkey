using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Trap : MonoBehaviour
{
    public virtual TrapStats TrapStats => _trapStats;
    [SerializeField] protected TrapStats _trapStats;

    void Start()
    {

    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) Player.instance.TakeDamage(_trapStats.Damage);
    }
}