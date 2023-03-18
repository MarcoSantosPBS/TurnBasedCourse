using System;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    [SerializeField] private Transform gridSystemVisualSinglePrefab;
    [SerializeField] private List<GridVisualTypeMaterial> gridVisualTypes;

    private GridSystemVisualSingle[,] gridSystemVisualSingles;

    [Serializable]
    public struct GridVisualTypeMaterial
    {
        public GridVisualType gridVisualType;
        public Material material;
    }

    public enum GridVisualType
    {
        Red,
        Yellow,
        White,
        Blue,
        RedSoft
    }

    private void Start()
    {
        gridSystemVisualSingles = new GridSystemVisualSingle[
            LevelGrid.Instance.GetWidth(),
            LevelGrid.Instance.GetHeight()
        ];

        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetWidth(); z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Transform instantiated = Instantiate(gridSystemVisualSinglePrefab,
                    LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);

                GridSystemVisualSingle gridSystemVisualSingle = instantiated.GetComponent<GridSystemVisualSingle>();
                gridSystemVisualSingle.Hide();
                gridSystemVisualSingles[x, z] = gridSystemVisualSingle;
            }
        }

        UnitActionSystem.Instance.OnSelectedActionChange += Instance_OnSelectedActionChange;
        LevelGrid.Instance.OnAnyUnitMovedGridPosition += LevelGrid_OnAnyUnitMovedGridPosition;
        UpdateGridSystemVisual();
    }

    public void HideAllGridPosition()
    {
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {
                gridSystemVisualSingles[x, z].Hide();
            }
        }
    }

    public void ShowGridPositionRange(GridPosition gridPosition, int range, GridVisualType gridVisualType)
    {
        List<GridPosition> validGridPositions = new List<GridPosition>();

        for (int x = -range; x <= range; x++)
        {
            for (int z = -range; z <= range; z++)
            {
                GridPosition offsetPosition = new GridPosition(x, z);
                GridPosition testGridPosition = gridPosition + offsetPosition;
                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) { continue; }
                if (testDistance > range) { continue; }

                validGridPositions.Add(testGridPosition);
            }
        }

        ShowGridPositions(validGridPositions, gridVisualType);
    }

    public void ShowGridPositions(List<GridPosition> gridPositions, GridVisualType gridVisualType)
    {
        foreach (GridPosition gridPosition in gridPositions)
        {
            gridSystemVisualSingles[gridPosition.x, gridPosition.z].Show(GetGridVisualTypeMaterial(gridVisualType));
        }
    }

    private void UpdateGridSystemVisual()
    {
        HideAllGridPosition();
        BaseAction baseAction = UnitActionSystem.Instance.GetSelectedAction();
        List<GridPosition> validGridPositions = baseAction.GetValidActionGridPositions();
        Unit unit = UnitActionSystem.Instance.GetSelectedUnit();

        GridVisualType gridVisualType;

        switch (baseAction)
        {
            case SpinAction spinAction:
                gridVisualType = GridVisualType.Blue;
                break;
            case ShootAction shootAction:
                int range = shootAction.GetMaxShootDistance();
                ShowGridPositionRange(unit.GetGridPosition(), range, GridVisualType.RedSoft);
                gridVisualType = GridVisualType.Red;
                break;
            default:
                gridVisualType = GridVisualType.White;
                break;
        }

        ShowGridPositions(validGridPositions, gridVisualType);
    }

    private Material GetGridVisualTypeMaterial(GridVisualType gridVisualType)
    {
        foreach (GridVisualTypeMaterial gridVisualTypeMaterial in gridVisualTypes)
        {
            if (gridVisualTypeMaterial.gridVisualType == gridVisualType)
            {
                return gridVisualTypeMaterial.material;
            }
        }

        Debug.LogError("Could not find GridVisualTypeMaterial for type: " + gridVisualType);
        return null;
    }

    public void Instance_OnSelectedActionChange()
    {
        UpdateGridSystemVisual();
    }

    public void LevelGrid_OnAnyUnitMovedGridPosition()
    {
        UpdateGridSystemVisual();
    }
}
