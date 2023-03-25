using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode 
{
    private int gCost;
    private int hCost;
    private int fCost;
    private PathNode cameFromNode;
    private GridPosition gridPosition;

    public PathNode(GridPosition gridPosition)
    {
        this.gridPosition = gridPosition;
    }

    public override string ToString()
    {
        return gridPosition.ToString();
    }

    public int GetGCost() => gCost;
    public int GetHCost() => hCost;
    public int GetFCost() => fCost;
    public GridPosition GetGridPosition() => gridPosition;
    public PathNode GetCameFromNode() => cameFromNode;

    public void SetGCost(int gCost) => this.gCost = gCost;
    public void SetHCost(int hCost) => this.hCost = hCost;
    public void SetCameFromNode(PathNode cameFromNode) => this.cameFromNode = cameFromNode;

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public void ResetCameFromPathNode()
    {
        cameFromNode = null;
    }

}
