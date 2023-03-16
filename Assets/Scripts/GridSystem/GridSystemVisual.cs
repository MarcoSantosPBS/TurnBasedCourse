using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    [SerializeField] private Transform gridSystemVisualSinglePrefab;

    private GridSystemVisualSingle[,] gridSystemVisualSingles;

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
    }

    private void Update()
    {
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

    public void ShowGridPositions(List<GridPosition> gridPositions)
    {
        foreach (GridPosition gridPosition in gridPositions)
        {
            gridSystemVisualSingles[gridPosition.x, gridPosition.z].Show();
        }
    }

    private void UpdateGridSystemVisual()
    {
        HideAllGridPosition();
        BaseAction baseAction = UnitActionSystem.Instance.GetSelectedAction();
        List<GridPosition> validGridPositions = baseAction.GetValidActionGridPositions();
        ShowGridPositions(validGridPositions);
    }
}
