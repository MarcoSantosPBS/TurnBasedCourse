using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private float timer;
    private State state;

    private enum State
    {
        WaitingForEnemyTurn,
        TakingTurn,
        Busy
    }

    private void Awake()
    {
        state = State.WaitingForEnemyTurn;
    }

    private void Start()
    {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    private void Update()
    {
        if (TurnSystem.Instance.IsPlayerTurn()) { return; }

        switch (state)
        {
            case State.WaitingForEnemyTurn:
                break;
            case State.TakingTurn:
                timer -= Time.deltaTime;
                if (timer <= 0) 
                {
                    if (TryTakeEnemyAIAction(SetStateTakingTurn))
                    {
                        state = State.Busy;
                    }
                    else
                    {
                        TurnSystem.Instance.NextTurn();
                    }
                }
                break;
            case State.Busy:
                break;
            default:
                break;
        }
    }

    private void SetStateTakingTurn()
    {
        timer = 0.5f;
        state = State.TakingTurn;
    }

    private bool TryTakeEnemyAIAction(Action onAIActionCompleted)
    {
        foreach (Unit enemyUnit in UnitManager.Instance.GetEnemyUnits())
        {
            if (TryTakeEnemyAIAction(enemyUnit, onAIActionCompleted))
            {
                return true;
            }
        }

        return false;
    }

    private bool TryTakeEnemyAIAction(Unit enemyUnit, Action onAIActionCompleted)
    {
        SpinAction spinAction = enemyUnit.GetSpinAction();

        GridPosition enemyGridPosition = enemyUnit.GetGridPosition();

        if (spinAction.IsValidActionGridPosition(enemyGridPosition))
        {
            if (enemyUnit.TrySpendActionPointsToTakeAction(spinAction))
            {
                spinAction.TakeAction(enemyGridPosition, onAIActionCompleted);
                return true;
            }
        }

        return false;
    }

    private void TurnSystem_OnTurnChanged()
    {
        if (!TurnSystem.Instance.IsPlayerTurn())
        {
            state = State.TakingTurn;
            timer = 2f;
        }
    }
}
