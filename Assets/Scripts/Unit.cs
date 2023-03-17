using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private MoveAction moveAction;
    [SerializeField] private SpinAction spinAction;
    [SerializeField] private bool isEnemy;

    private const int ACTION_POINTS_MAX = 2;
    private BaseAction[] baseActions;
    private GridPosition currentPosition;
    private int actionPoints = ACTION_POINTS_MAX;
    public static event Action OnAnyActionPointsChanged;

    private void Awake()
    {
        baseActions = GetComponents<BaseAction>();
    }

    private void Start()
    {
        currentPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitToPosition(currentPosition, this);
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    private void Update()
    {
        if (LevelGrid.Instance.GetGridPosition(transform.position) != currentPosition)
        {
            GridPosition newPosition = LevelGrid.Instance.GetGridPosition(transform.position);
            LevelGrid.Instance.UpdateUnitPlacement(currentPosition, newPosition, this);
            currentPosition = newPosition;
        }
    }

    public  void TakeDamage()
    {
        Debug.Log(transform + "took damage");
    }

    public bool TrySpendActionPointsToTakeAction(BaseAction action)
    {
        if (CanSpendActionPointsToTakeAction(action))
        {
            SpendActionPoints(action.GetActionPointsCost());
            return true;
        }

        return false;
    }

    public bool CanSpendActionPointsToTakeAction(BaseAction action)
    {
        return action.GetActionPointsCost() <= actionPoints;
    }

    private void SpendActionPoints(int amount)
    {
        actionPoints -= amount;
        OnAnyActionPointsChanged?.Invoke();
    }

    private void TurnSystem_OnTurnChanged()
    {
        if ((IsEnemy() && !TurnSystem.Instance.IsPlayerTurn()) ||
            (!IsEnemy() && TurnSystem.Instance.IsPlayerTurn()))
        {
            actionPoints = ACTION_POINTS_MAX;
            OnAnyActionPointsChanged?.Invoke();
        }
    }

    public Vector3 GetWorldPosition()
    {
        return LevelGrid.Instance.GetWorldPosition(currentPosition);
    }

    public SpinAction GetSpinAction() => spinAction;
    public MoveAction GetMoveAction() => moveAction;
    public GridPosition GetGridPosition() => currentPosition;
    public BaseAction[] GetBaseActions() => baseActions;
    public int GetActionPoints() => actionPoints;
    public bool IsEnemy() => isEnemy;
    
}
