using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    [SerializeField] private Transform debugVisual;

    private int width;
    private int height;
    private float cellsize;
    private GridSystem<PathNode> gridSystem;

    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    public static Pathfinding Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;
        gridSystem = new GridSystem<PathNode>(10, 10, 2, CreatePathNode);
        gridSystem.ShowDebugVisual(debugVisual);
    }

    private PathNode CreatePathNode(GridSystem<PathNode> gridSystem, GridPosition gridPosition)
    {
        return new PathNode(gridPosition);
    }

    public List<GridPosition> FindNode(GridPosition startPosition, GridPosition endPosition)
    {
        
        List<PathNode> openList = new List<PathNode>();
        List<PathNode> closedList = new List<PathNode>();

        PathNode startNode = gridSystem.GetGridObject(startPosition);
        PathNode endNode = gridSystem.GetGridObject(endPosition);

        openList.Add(startNode);

        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);

                PathNode node = gridSystem.GetGridObject(gridPosition);
                node.SetGCost(int.MaxValue);
                node.SetHCost(0);
                node.CalculateFCost();
                node.ResetCameFromPathNode();
            }
        }

        startNode.SetGCost(0);
        startNode.SetHCost(CalculateDistance(startPosition, endPosition));
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostNode(openList);

            if (currentNode == endNode)
            {
                return CalculatePath(currentNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            List<PathNode> neighbours = GetNeighbours(currentNode);

            foreach (PathNode neighbour in neighbours)
            {
                if (closedList.Contains(neighbour))
                {
                    continue;
                }

                int tentativeGCost = 
                    currentNode.GetGCost() + CalculateDistance(currentNode.GetGridPosition(), neighbour.GetGridPosition());

                if (tentativeGCost < neighbour.GetGCost())
                {
                    neighbour.SetCameFromNode(currentNode);
                    neighbour.SetGCost(tentativeGCost);
                    neighbour.SetHCost(CalculateDistance(neighbour.GetGridPosition(), endPosition));
                    neighbour.CalculateFCost();
                    

                    if (!openList.Contains(neighbour)) { openList.Add(neighbour); }
                }
            }
        }
        // No path found
        return null;
    }

    private List<GridPosition> CalculatePath(PathNode endNode)
    {
        List<GridPosition> positions = new List<GridPosition>();

        positions.Add(endNode.GetGridPosition());

        PathNode previousPath = endNode.GetCameFromNode();

        while (previousPath != null)
        {
            positions.Add(previousPath.GetGridPosition());
            previousPath = previousPath.GetCameFromNode();
        }

        positions.Reverse();
        return positions;
    }

    private int CalculateDistance(GridPosition gridPositionA, GridPosition gridPositionB)
    {
        GridPosition position = gridPositionA - gridPositionB;

        int xDistance = Mathf.Abs(position.x);
        int zDistance = Mathf.Abs(position.z);

        int remainingDistance = Mathf.Abs(xDistance - zDistance);

        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + remainingDistance * MOVE_STRAIGHT_COST;
    }

    private PathNode GetLowestFCostNode(List<PathNode> nodes)
    {
        PathNode lowestFCostNode = nodes[0];

        foreach (PathNode node in nodes)
        {
            if (lowestFCostNode.GetFCost() > node.GetFCost()) { lowestFCostNode = node;}
        }

        return lowestFCostNode;
    }

    private List<PathNode> GetNeighbours(PathNode currentNode)
    {
        List<PathNode > neighbours = new List<PathNode>();

        GridPosition gridPosition = currentNode.GetGridPosition();

        if (gridPosition.x - 1 >= 0)
        {
            //left 
            neighbours.Add(GetNode(gridPosition.x - 1, gridPosition.z + 0));

            if (gridPosition.z + 1 < gridSystem.GetHeight())
            {
                // Upper left
                neighbours.Add(GetNode(gridPosition.x - 1, gridPosition.z + 1));
            }

            if (gridPosition.z - 1 >= 0)
            {
                // Lower left
                neighbours.Add(GetNode(gridPosition.x - 1, gridPosition.z - 1));
            }
        }

        if (gridPosition.x + 1 < gridSystem.GetWidth())
        {
            //Right 
            neighbours.Add(GetNode(gridPosition.x + 1, gridPosition.z + 0));

            if (gridPosition.z + 1 < gridSystem.GetHeight())
            {
                // Upper Right
                neighbours.Add(GetNode(gridPosition.x + 1, gridPosition.z + 1));
            }

            if (gridPosition.z - 1 >= 0)
            {
                // Lower Right
                neighbours.Add(GetNode(gridPosition.x + 1, gridPosition.z - 1));
            }
        }

        if (gridPosition.z + 1 < gridSystem.GetHeight())
        {
            // Up
            neighbours.Add(GetNode(gridPosition.x + 0, gridPosition.z + 1));
        }

        if (gridPosition.z - 1 >= 0)
        {
            // Down
            neighbours.Add(GetNode(gridPosition.x + 0, gridPosition.z - 1));
        }

        return neighbours;
    }

    private PathNode GetNode(int x, int z)
    {
        return gridSystem.GetGridObject(new GridPosition(x, z));
    }
}
