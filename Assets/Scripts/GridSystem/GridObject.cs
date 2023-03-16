using System;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    private List<Unit> units;
    private GridPosition gridPosition;

    public GridObject(GridPosition gridPosition)
    {
        units = new List<Unit>();
        this.gridPosition = gridPosition;
    }

    public void AddUnit(Unit unit)
    {
        units.Add(unit);
    }

    public void RemoveUnit(Unit unit)
    {
        units.Remove(unit);
    }

    public List<Unit> GetUnits()
    {
        return units;
    }

    public bool HasAnyUnit()
    {
        return units.Count > 0;
    }

    public override string ToString()
    {
        string baseString = string.Empty;

        foreach (Unit unit in units)
        {
            baseString += "\n" + unit.name;
        }

        return gridPosition.ToString() + baseString;
    }
}

