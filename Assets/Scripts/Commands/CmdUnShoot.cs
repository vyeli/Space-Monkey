using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CmdUnShoot : ICommand
{
    public void Execute() { }

    public void Undo() => throw new System.NotImplementedException();
}