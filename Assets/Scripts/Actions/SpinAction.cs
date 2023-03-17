using System;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{
    private float spinnedAmount;

    private void Update()
    {
        if (!isActive) { return; }
        
        float spinAmount = 360 * Time.deltaTime;
        spinnedAmount += spinAmount;

        transform.eulerAngles += new Vector3(0, spinAmount, 0);

        if (spinnedAmount >= 360)
        {
            ActionComplete();
        }
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionCompleted)
    {
        spinnedAmount = 0;
        ActionStart(onActionCompleted);
    }

    public override List<GridPosition> GetValidActionGridPositions()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        return new List<GridPosition>() { unitGridPosition };
    }

}
