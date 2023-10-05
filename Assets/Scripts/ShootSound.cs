using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource), typeof(AudioClip))]
public class ShootSound : MonoBehaviour
{
    public AudioClip shootSound;
    public AudioClip emptyMagacine;
    public AudioSource AudioSource;

    void PlaySound()
    {
        Debug.Log("CurrentBulletCount: " + Player.instance._gun.CurrentBulletCount);
        if (Player.instance._gun.CurrentBulletCount > 0)
            AudioSource.PlayOneShot(shootSound);
        else
            AudioSource.PlayOneShot(emptyMagacine);

    }
}
