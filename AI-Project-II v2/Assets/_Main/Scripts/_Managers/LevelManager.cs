using System;
using System.Collections;
using System.Collections.Generic;
using Game.Managers;
using Game.Managers.Level;
using UnityEngine;
using UnityEngine.Serialization;

public class LevelManager : MonoManager<LevelManager>
{
    public static readonly string EvaluateCondition = "Update";
    public static readonly string LevelEnd = "LevelEnd";
    
    [SerializeField] private LevelCondition mainCondition;
    [SerializeField] private LevelCondition loseCondition;
    //[SerializeField] private LevelObjective[] subObjectives;

    protected override void OnAwake()
    {
        if (HasWinCondition())
            mainCondition = mainCondition.Clone();

        AddEvent(EvaluateCondition, OnEvaluateHandler);
    }

    private void Start()
    {
        Subscribe(GameManager.Instance);
    }

    private void OnEvaluateHandler(object[] args)
    {
        Debug.Log("Evaluate");
        
        if (HasWinCondition() && mainCondition.Evaluate())
        {
            OnLevelWon(args);
        }
        else if (HasLoseCondition() && loseCondition.Evaluate())
        {
            OnLevelLost(args);
        }

        
    }

    private void OnLevelWon(object[] args)
    {
        NotifyAll(GameManager.GameWonMessage, args);
        OnLevelEnd(args);
    }
    
    private void OnLevelLost(object[] args)
    {
        NotifyAll(GameManager.GameLostMessage, args);
        OnLevelEnd(args);
    }

    private void OnLevelStart()
    {
        
    }

    private void OnLevelEnd(object[] args)
    {
        NotifyAll(LevelEnd, args);
    }

    private bool HasWinCondition() => mainCondition != null;
    private bool HasLoseCondition() => loseCondition != null;

    protected override void OnDisposed()
    {
        base.OnDisposed();
        if (HasWinCondition())
        {
            mainCondition.Dispose();
            mainCondition = null;
        }
        if (HasLoseCondition())
        {
            loseCondition.Dispose();
            loseCondition = null;
        }
        
        Unsubscribe(GameManager.Instance);
    }
}
