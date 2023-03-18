using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    private List<Unit> units;
    private List<Unit> enemyUnits;
    private List<Unit> friendlyUnits;

    public static UnitManager Instance;


    private void Awake()
    {
        units = new List<Unit>();
        enemyUnits = new List<Unit>();
        friendlyUnits = new List<Unit>();

        if (Instance != null)
        {
            print($"There's already an instance of Unit Manager: {gameObject.name}");
            Destroy(gameObject);
        }

        Instance = this;
    }

    private void Start()
    {
        Unit.OnAnyUnitSpawned += Unit_OnAnyUnitSpawned;
        Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;
    }

    private void Unit_OnAnyUnitSpawned(object sender, EventArgs e)
    {
        Unit unit = (Unit)sender;

        units.Add(unit);

        if (unit.IsEnemy()) { enemyUnits.Add(unit); }
        else { friendlyUnits.Add(unit); }
    }

    private void Unit_OnAnyUnitDead(object sender, EventArgs e)
    {
        Unit unit = (Unit)sender;

        units.Remove(unit);

        if (unit.IsEnemy()) { enemyUnits.Remove(unit); }
        else { friendlyUnits.Remove(unit); }
    }

    public List<Unit> GetUnits() => units;
    public List<Unit> GetEnemyUnits() => enemyUnits;
    public List<Unit> GetFriendlyUnits() => friendlyUnits;
}
