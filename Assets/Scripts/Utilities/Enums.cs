using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums : MonoBehaviour
{
    public enum Levels
    {
        MainMenu,
        Level1,
        DefeatScreen,
        VictoryScreen,
        Ranking,
    }

    public enum ScoreLetter
    {
        SS,
        S,
        A,
        B,
        C,
        D
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
