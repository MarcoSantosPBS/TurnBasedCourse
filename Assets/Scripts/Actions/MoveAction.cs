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
    [SerializeField] private Animator animator;
   
    private Vector3 targetPosition;

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
            animator.SetBool("IsRunning", true);
            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.position += direction * Time.deltaTime * speed;
            transform.forward += direction * Time.deltaTime * rotationSpeed;
        }
        else
        {
            isActive = false;
            animator.SetBool("IsRunning", false);
            onActionCompleted?.Invoke();
        }
    }

    public override void TakeAction(GridPosition targetPosition, Action onActionCompleted)
    {
        this.targetPosition = LevelGrid.Instance.GetWorldPosition(targetPosition);
        this.onActionCompleted = onActionCompleted;
        isActive = true;
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

}
