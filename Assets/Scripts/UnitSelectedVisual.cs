using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectedVisual : MonoBehaviour
{
    [SerializeField] MeshRenderer mesh;
    [SerializeField] Unit unit;

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChange += UnitActionSystem_UpdateVisuals;
        UnitActionSystem_UpdateVisuals();
    }

    private void UnitActionSystem_UpdateVisuals()
    {
        mesh.enabled = UnitActionSystem.Instance.GetSelectedUnit() == unit;
    }
}
