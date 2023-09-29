using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CmdMovement : ICommand
{
    private IMoveable _moveable;
    private Vector3 _direction;
    private float _speed;

    public CmdMovement(IMoveable moveable, Vector3 direction, float speed)
    {
        _moveable = moveable;
        _direction = direction;
        _speed = speed;
    }

    public void Execute()
    {
        _moveable.Move(_direction * _speed * Time.deltaTime);
    }

    public void Undo()
    {
        throw new System.NotImplementedException();
    }

}