using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBusyUI : MonoBehaviour
{
    private void Start()
    {
        UnitActionSystem.Instance.OnBusyChange += UnitActionSystem_OnBusyChange;
        UnitActionSystem_OnBusyChange(false);
    }

    private void UnitActionSystem_OnBusyChange(bool isBusy)
    {
        gameObject.SetActive(isBusy);
    }
}
