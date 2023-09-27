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
    #endregion

    #region PRIVATE_PROPERTIES
    [SerializeField] protected GameObject _bulletPrefab;
    [SerializeField] protected Transform _bulletRotation;
    [SerializeField] protected Transform _bulletContainer;
    [SerializeField] protected int _currentBulletCount;
    #endregion

    #region UNITY_EVENTS
    private void Start()
    {
        _currentBulletCount = 10;
    }
    #endregion

    #region IGUN_METHODS
    public virtual void Shoot() { }
    #endregion

}