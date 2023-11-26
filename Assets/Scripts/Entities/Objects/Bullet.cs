using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Bullet : MonoBehaviour, IBullet
{
    #region I_BULLET_PROPERTIES
    public float Speed => _speed;
    public float LifeTime => _lifeTime;
    new public Collider collider => _collider;
    public IGun Owner => _owner;
    #endregion

    #region PRIVATE_PROPERTIES
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _lifeTime = 2f;
    private Collider _collider;
    [SerializeField] private IGun _owner;
    [SerializeField] private List<int> _layerMasks;
    #endregion

    #region UNITY_EVENTS
    void Start()
    {
        _collider = GetComponent<Collider>();

        Init();
    }

    void Update()
    {
        Travel();

        _lifeTime -= Time.deltaTime;
        if (_lifeTime <= 0)
        {
            Die();
        }
    }

    private void OnDestroy()
    {
        Debug.Log("Bullet has hit target");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_layerMasks.Contains(other.gameObject.layer))
        {
            IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                EventQueueManager.instance.AddCommand(new CmdTakeDamage(damageable, _owner.Damage));
            }

            Die();
        }
    }
    #endregion

    #region IBULLET_METHODS
    public void Init()
    {
        _collider.isTrigger = true;
        // _rigidbody.isKinematic = true;
        // _rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    public void Travel() => transform.position += transform.forward * _speed * Time.deltaTime;

    public void Die() => Destroy(this.gameObject);

    public void SetOwner(IGun owner) => _owner = owner;
    #endregion
}