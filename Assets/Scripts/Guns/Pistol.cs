using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Gun
{

    [SerializeField] private float _maxLength;
    [SerializeField] private float _duration;
    [SerializeField] private Transform muzzlePoint;
    [SerializeField] private Transform playerModel;

    [SerializeField] private ParticleSystem _muzzleParticles;
    [SerializeField] private ParticleSystem _hitParticles;



    // Now pistol will fire a raycast instead of instantiating a bullet
    [SerializeField] private LineRenderer _beam;

    public Transform MuzzlePoint => muzzlePoint;
    
    public override void Shoot()
    {
        if (_currentBulletCount == 0 && !InfiniteMode)
            return;

        Ray ray = new Ray(muzzlePoint.position, playerModel.forward);
        bool cast = Physics.Raycast(ray, out RaycastHit hit, _maxLength);
        Vector3 hitPoint = cast ? hit.point : ray.GetPoint(_maxLength) + playerModel.forward * _maxLength;

        
        _beam.SetPosition(0, muzzlePoint.position);
        _beam.SetPosition(1, hitPoint);

        _hitParticles.transform.position = hitPoint;

        StartCoroutine(ShowBeam());


        if (cast)
        {
            IDamageable damageable = hit.collider.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                EventQueueManager.instance.AddCommand(new CmdTakeDamage(damageable, Damage));
            }
        }

        base.UpdateBulletCount();
    }

    IEnumerator ShowBeam()
    {
        _muzzleParticles.Play();
        _hitParticles.Play();
        _beam.enabled = true;
        yield return new WaitForSeconds(_duration);
        _beam.enabled = false;
        _hitParticles.Stop();
        _muzzleParticles.Stop();
    }
}