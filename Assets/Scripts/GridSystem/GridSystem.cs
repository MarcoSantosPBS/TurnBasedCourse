using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem
{
    private int height;
    private int width;
    private int cellSize;
    private GridObject[,] gridObjects;

    public GridSystem(int height, int width, int cellSize, Transform transform)
    {
        this.height = height;
        this.width = width;
        this.cellSize = cellSize;
        gridObjects = new GridObject[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                gridObjects[x, z] = new GridObject(gridPosition);
                ShowDebugVisual(transform, gridPosition);
            }
        }
    }

    private void ShowDebugVisual(Transform transform, GridPosition gridPosition)
    {
        Transform instantiated = GameObject.Instantiate(transform, GetWorldPosition(gridPosition), Quaternion.identity);
        instantiated.GetComponent<GridDebugVisual>().Init(gridObjects[gridPosition.x, gridPosition.z]);
    }

    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        return new Vector3(gridPosition.x * cellSize, 0, gridPosition.z * cellSize);
    }

    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        return new GridPosition(
            Mathf.RoundToInt(worldPosition.x / cellSize),
            Mathf.RoundToInt(worldPosition.z / cellSize));
    }

    public GridObject GetGridObject(GridPosition gridPosition)
    {
        return gridObjects[gridPosition.x, gridPosition.z];
    }

    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        return gridPosition.x >= 0 &&
               gridPosition.z >= 0 &&
               gridPosition.x < width &&
               gridPosition.z < height;
    }

    public int GetHeight() => height;
    public int GetWidth() => width;
}
