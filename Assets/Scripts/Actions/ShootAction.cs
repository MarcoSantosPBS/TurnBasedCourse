using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{
    [SerializeField] private int maxShootDistance = 7;

    private enum State
    {
        Aiming,
        Shooting,
        Cooloff
    }

    private State state;
    private float stateTimer;
    private Unit targetUnit;
    private bool canShootBullet;

    public event EventHandler<OnShootEventArgs> OnShoot;

    public class OnShootEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit shootingUnit;
    }

    private void Update()
    {
        if (!isActive) { return; }

        stateTimer -= Time.deltaTime;

        switch (state)
        {
            case State.Aiming:
                Vector3 direction = (targetUnit.transform.position - transform.position).normalized;
                float rotationSpeed = 10f;
                transform.forward += direction * Time.deltaTime * rotationSpeed;
                break;
            case State.Shooting:
                if (canShootBullet)
                {
                    Shoot();
                    canShootBullet = false;
                }
                break;
            case State.Cooloff:
                break;
        }

        if (stateTimer <= 0f)
        {
            NextState();
        }
    }

    private void Shoot()
    {
        targetUnit.TakeDamage();
        OnShoot?.Invoke(this, new OnShootEventArgs
        {
            targetUnit = targetUnit,
            shootingUnit = unit
        });
    }

    private void NextState()
    {
        switch (state)
        {
            case State.Aiming:
                state = State.Shooting;
                stateTimer = .1f;
                break;
            case State.Shooting:
                state = State.Cooloff;
                stateTimer = .5f;
                break;
            case State.Cooloff:
                ActionComplete();
                break;
        }
    }

    public override List<GridPosition> GetValidActionGridPositions()
    {
        List<GridPosition> validGridPositions = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxShootDistance; x <= maxShootDistance; x++)
        {
            for (int z = -maxShootDistance; z <= maxShootDistance; z++)
            {
                GridPosition offsetPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetPosition;
                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) { continue; }
                if (testDistance > maxShootDistance) { continue; }
                if (!LevelGrid.Instance.HasAnyUnitInGridPosition(testGridPosition)) { continue; }

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                if (targetUnit.IsEnemy() == unit.IsEnemy()) { continue; }

                validGridPositions.Add(testGridPosition);
            }
        }

        return validGridPositions;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionCompleted)
    {
        state = State.Aiming;

        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        canShootBullet = true;
        stateTimer = 1f;
        ActionStart(onActionCompleted);
    }
}
