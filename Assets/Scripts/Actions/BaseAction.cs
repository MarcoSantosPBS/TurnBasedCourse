using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    [SerializeField] protected string actionName;

    protected bool isActive;
    protected Unit unit;
    protected Action onActionCompleted;

    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
    }

    public virtual bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositions = GetValidActionGridPositions();
        return validGridPositions.Contains(gridPosition);
    }

    public string GetActionName() => actionName;
    public virtual int GetActionPointsCost() => 1;
    public abstract void TakeAction(GridPosition gridPosition, Action onActionCompleted);
    public abstract List<GridPosition> GetValidActionGridPositions();
}
