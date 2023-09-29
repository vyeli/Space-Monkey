using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CmdShoot : ICommand
{
    private IGun _gun;

    public CmdShoot(Gun gun)
    {
        _gun = gun;
    }

    public void Execute()
    {
        _gun.Shoot();
        // EventManager.instance.AvatarChange();
    }

    public void Undo() => throw new System.NotImplementedException();
}