using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IBullet
{
    #region PRIVATE_PROPERTIES
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _lifeTime = 2f;
    #endregion

    #region I_BULLET_PROPERTIES
    public float Speed => _speed;
    public float LifeTime => _lifeTime;
    public void Travel() => transform.position += transform.forward * _speed * Time.deltaTime;
    #endregion

    #region UNITY_EVENTS
    void Start() { }

    void Update()
    {
        Travel();

        _lifeTime -= Time.deltaTime;
        if (_lifeTime <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    #endregion
}