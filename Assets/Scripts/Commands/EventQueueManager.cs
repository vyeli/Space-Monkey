using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventQueueManager : MonoBehaviour
{
    #region SINLGETON
    public static EventQueueManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    #endregion

    public Queue<ICommand> EventQueue => _eventQueue;
    private Queue<ICommand> _eventQueue = new Queue<ICommand>();

    public void AddCommand(ICommand command) => _eventQueue.Enqueue(command);

    public void ExecuteCommand()
    {
        if (_eventQueue.Count > 0)
        {
            ICommand command = _eventQueue.Dequeue();
            if (!GameManager.instance.IsPaused)
            {
                if (command is CmdUnShoot && _eventQueue.Count == 0) Debug.Log("No more commands in queue");
                if (command is CmdUnShoot && _eventQueue.Count > 0)
                {
                    ICommand nextCommand = _eventQueue.Peek();
                    if (nextCommand is CmdShoot)
                    {
                        _eventQueue.Dequeue();
                    }
                    else
                    {
                        Debug.Log("Next command is " + nextCommand.GetType().Name);
                    }
                }
                command.Execute();
            }
            // Acá va la lógica de los comandos
            // Ej: si el tipo está stuneado ignorar los inputs de movimiento
            // if (command is CmdMovement && isStunned) continue;
        }
    }

    #region UNITY_EVENTS
    private void Update()
    {
        while (_eventQueue.Count > 0)
        {
            ExecuteCommand();
        }
    }
    #endregion
}
