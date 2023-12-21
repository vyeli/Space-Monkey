using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovingEnemy))]
public class NavAgentTester : MonoBehaviour
{
    private MovingEnemy _movingEnemy;
    // Start is called before the first frame update
    void Start()
    {
        _movingEnemy = GetComponent<MovingEnemy>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(_movingEnemy._currentState);
    }
}
