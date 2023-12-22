using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour, IGun
{
    [SerializeField] private GunStats _gunStats;

    #region IGUN_PROPERTIES
    public GameObject BulletPrefab => _bulletPrefab;
    public Transform BulletContainer => _bulletContainer;
    public int CurrentBulletCount { get => _currentBulletCount; set => _currentBulletCount = value; }
    public int Damage => _gunStats.Damage;
    #endregion

    #region PRIVATE_PROPERTIES
    [SerializeField] protected GameObject _bulletPrefab;
    [SerializeField] protected Transform _bulletContainer;
    [SerializeField] protected int _currentBulletCount;
    #endregion

    #region UNITY_EVENTS
    private void Start()
    {
        // EventsManager.instance.BulletCountChange(_currentBulletCount);
        UiManager.instance.UpdateBulletCount(_currentBulletCount.ToString());
    }
    #endregion

    #region IGUN_METHODS
    public virtual void Shoot() { }
    #endregion

    protected void UpdateBulletCount()
    {
        if (!InfiniteMode)
        {
            _currentBulletCount--;
            EventsManager.instance.BulletCountChange(_currentBulletCount.ToString());
        }
    }

    private bool _infiniteMode;

    public bool InfiniteMode
    {
        get { return _infiniteMode; }
        set { UpdateInfiniteMode(value); }
    }

    private void UpdateInfiniteMode(bool value)
    {
        if (value)
        {
            UiManager.instance.UpdateBulletCount("\u221E");
        }
        else
        {
            UiManager.instance.UpdateBulletCount(_currentBulletCount.ToString());
        }
        _infiniteMode = value;
    }

}