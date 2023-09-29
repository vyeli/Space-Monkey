using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour, IGun
{
    #region IGUN_PROPERTIES
    public GameObject BulletPrefab => _bulletPrefab;
    public Transform BulletContainer => throw new System.NotImplementedException();
    public Quaternion Rotation => _bulletRotation.rotation;
    public int CurrentBulletCount => _currentBulletCount;
    public int Damage => _damage;
    #endregion

    #region PRIVATE_PROPERTIES
    [SerializeField] protected GameObject _bulletPrefab;
    [SerializeField] protected Transform _bulletRotation;
    [SerializeField] protected Transform _bulletContainer;
    [SerializeField] protected int _currentBulletCount = 10;
    [SerializeField] protected int _damage = 1;
    #endregion

    #region UNITY_EVENTS
    private void Start()
    {
        UiManager.instance.bulletCount.text = _currentBulletCount.ToString();
    }
    #endregion

    #region IGUN_METHODS
    public virtual void Shoot() { }
    #endregion

}