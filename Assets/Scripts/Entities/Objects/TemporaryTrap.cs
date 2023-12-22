using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class TemporaryTrap : Trap
{
    public float _interval = 6f;
    public float _duration = 6f;
    [SerializeField] private bool _isActive;
    [SerializeField] private float _inactiveWindow;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private Material _inactiveMaterial;
    [SerializeField] private Material _activeMaterial;
    private float _timeElapsed = 0f;
    private MeshRenderer _meshRenderer;
    void Start()
    {
        base.Start();
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if (_isActive)
        {
            if (!_particleSystem.isPlaying)
                _particleSystem.Play();
            _timeElapsed += Time.deltaTime;
            if (_timeElapsed > _duration * _inactiveWindow)
            {
                if (_timeElapsed < _duration * (1 - _inactiveWindow))
                {
                    _meshRenderer.material = _activeMaterial;
                    _collider.enabled = true;
                }
                else
                {
                    _meshRenderer.material = _inactiveMaterial;
                    _collider.enabled = false;
                }
            }
            if (_timeElapsed >= _duration)
            {
                _particleSystem.Stop();
                _isActive = false;
                _timeElapsed = 0;
            }
        }
        else
        {
            _collider.enabled = false;
            _timeElapsed += Time.deltaTime;
            if (_timeElapsed >= _interval)
            {
                _isActive = true;
                _timeElapsed = 0;
            }
        }
    }
}
