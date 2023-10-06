using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioClip))]
public class ShootSound : MonoBehaviour
{
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip emptyMagacine;

    void PlaySound()
    {
        Debug.Log("CurrentBulletCount: " + Player.instance._gun.CurrentBulletCount);
        if (Player.instance._gun.CurrentBulletCount > 0)
            SoundManager.instance.PlaySFX(shootSound);
        else
            SoundManager.instance.PlaySFX(emptyMagacine);
    }
}
