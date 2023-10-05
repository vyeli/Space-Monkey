using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums : MonoBehaviour
{
    public enum Levels
    {
        MainMenu,
        Level1,
        EndGame,
    }

    public enum AIState
    {
        Idle,
        Patrol,
        Chase,
        Attack,
        Dead,
    }
}
