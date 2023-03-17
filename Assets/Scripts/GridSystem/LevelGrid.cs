using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    [SerializeField] private Transform gridDebugPrefab;

    private GridSystem gridSystem;
    public static LevelGrid Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log($"There's already an LevelGrid Instantiated in the scene: {Instance.gameObject}");
            Destroy(gameObject);
        }

        Instance = this;
        gridSystem = new GridSystem(10, 10, 2, gridDebugPrefab);
    }

    public void AddUnitToPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = GetGridObject(gridPosition);
        gridObject.AddUnit(unit);
    }

    public void RemoveUnitFromPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = GetGridObject(gridPosition);
        gridObject.RemoveUnit(unit);
    }

    public void UpdateUnitPlacement(GridPosition from, GridPosition To, Unit unit)
    {
        GetGridObject(from).RemoveUnit(unit);
        GetGridObject(To).AddUnit(unit);
    }

    public bool HasAnyUnitInGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = GetGridObject(gridPosition);
        return gridObject.HasAnyUnit();
    }

    public Unit GetUnitAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = GetGridObject(gridPosition);
        return gridObject.GetUnit();
    }

    public Vector3 GetWorldPosition(GridPosition gridPosition) => gridSystem.GetWorldPosition(gridPosition);
    public GridPosition GetGridPosition(Vector3 worldPosition) => gridSystem.GetGridPosition(worldPosition);
    public GridObject GetGridObject(GridPosition gridPosition) => gridSystem.GetGridObject(gridPosition);
    public bool IsValidGridPosition(GridPosition gridPosition) => gridSystem.IsValidGridPosition(gridPosition);
    public int GetHeight() => gridSystem.GetHeight();
    public int GetWidth() => gridSystem.GetWidth();
}
