using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioClip))]
public class ShootEffect : MonoBehaviour
{
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip emptyMagacine;

    // Play the gun shoot sound & Create a new command of CmdShoot type and Send it to the EventQueue
    void Shoot()
    {

        Debug.Log("CurrentBulletCount: " + Player.instance._gun.CurrentBulletCount);
        if (Player.instance._gun.CurrentBulletCount > 0)
            SoundManager.instance.PlaySFX(shootSound);
        else
            SoundManager.instance.PlaySFX(emptyMagacine);

        CmdShoot cmdShoot = new CmdShoot(Player.instance._gun);
        EventQueueManager.instance.AddCommand(cmdShoot);

    }
}
