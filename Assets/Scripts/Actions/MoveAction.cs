using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
    [SerializeField] private float stoppingDistance;
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private int maxMoveDistance;
    [SerializeField] private LayerMask groundLayerMask;

    private Vector3 targetPosition;

    public event Action OnStartMoving;
    public event Action OnStopMoving;

    protected override void Awake()
    {
        base.Awake();
        targetPosition = transform.position;
    }

    private void Update()
    {
        if (!isActive) { return; }

        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.position += direction * Time.deltaTime * speed;
            transform.forward += direction * Time.deltaTime * rotationSpeed;
        }
        else
        {
            OnStopMoving?.Invoke();
            ActionComplete();
        }
    }

    public override void TakeAction(GridPosition targetPosition, Action onActionCompleted)
    {
        this.targetPosition = LevelGrid.Instance.GetWorldPosition(targetPosition);
        OnStartMoving?.Invoke();
        ActionStart(onActionCompleted);
    }

    public override List<GridPosition> GetValidActionGridPositions()
    {
        List<GridPosition> validGridPositions = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) { continue; }
                if (testGridPosition == unitGridPosition) { continue; }
                if (LevelGrid.Instance.HasAnyUnitInGridPosition(testGridPosition)) { continue; }

                validGridPositions.Add(testGridPosition);
            }
        }

        return validGridPositions;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        ShootAction shootAction = unit.GetShootAction();
        int targetCount = shootAction.GetTargetCountAtPosition(gridPosition);

        return new EnemyAIAction()
        {
            gridPosition = gridPosition,
            actionValue = targetCount * 10
        };
    }

}
