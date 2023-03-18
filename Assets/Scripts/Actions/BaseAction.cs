using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    [SerializeField] protected string actionName;

    public static event EventHandler OnAnyActionStarted;
    public static event EventHandler OnAnyActionCompleted;

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

    protected void ActionStart(Action onActionCompleted)
    {
        this.onActionCompleted = onActionCompleted;
        isActive = true;

        OnAnyActionStarted?.Invoke(this, EventArgs.Empty);
    }

    protected void ActionComplete()
    {
        isActive = false;
        onActionCompleted?.Invoke();

        OnAnyActionCompleted?.Invoke(this, EventArgs.Empty);
    }

    public string GetActionName() => actionName;
    public Unit GetUnit() => unit;
    public virtual int GetActionPointsCost() => 1;
    public abstract void TakeAction(GridPosition gridPosition, Action onActionCompleted);
    public abstract List<GridPosition> GetValidActionGridPositions();
}
