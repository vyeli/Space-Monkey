using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CmdMovement : ICommand
{
    private IMoveable _moveable;
    private Vector3 _direction;
    public CmdMovement(IMoveable moveable, Vector3 direction)
    {
        _moveable = moveable;
        _direction = direction;
    }

    public void Execute()
    {
        _moveable.Move(_direction);
    }

    public void Undo()
    {
        throw new System.NotImplementedException();
    }

}