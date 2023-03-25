using System;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem<TGridObject>
{
    private int height;
    private int width;
    private int cellSize;
    private TGridObject[,] gridObjects;

    public GridSystem(int height, int width, int cellSize, Func<GridSystem<TGridObject>, GridPosition, TGridObject> createGridObject)
    {
        this.height = height;
        this.width = width;
        this.cellSize = cellSize;
        gridObjects = new TGridObject[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                gridObjects[x, z] = createGridObject(this, gridPosition);
            }
        }
    }

    public void ShowDebugVisual(Transform transform)
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition position = new GridPosition(x, z);
                Transform instantiated = GameObject.Instantiate(transform, GetWorldPosition(position), Quaternion.identity);
                instantiated.GetComponent<GridDebugVisual>().Init(gridObjects[position.x, position.z]);
            }
        }
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

    public TGridObject GetGridObject(GridPosition gridPosition)
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
