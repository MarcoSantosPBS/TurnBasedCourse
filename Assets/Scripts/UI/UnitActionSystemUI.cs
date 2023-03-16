using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainer;
    [SerializeField] private TextMeshProUGUI actionPointsText;

    private List<ActionButtonUI> actionButtons;

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChange += UnitActionSystem_OnSelectedUnitChange;
        UnitActionSystem.Instance.OnSelectedActionChange += UnitActionSystem_OnSelectedActionChange;
        UnitActionSystem.Instance.OnActionStarted += UnitActionSystem_OnActionStarted;
        
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;

        actionButtons = new List<ActionButtonUI>();

        DeleteExistingButtons();
        CreateUnitActionButtons();
        UpdateSelectedVisual();
        UpdateActionPoints();
    }

    private void CreateUnitActionButtons()
    {
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

        foreach (BaseAction baseAction in selectedUnit.GetBaseActions())
        {
            Transform actionButtonTransform = Instantiate(actionButtonPrefab, actionButtonContainer);
            ActionButtonUI actionButtonUI = actionButtonTransform.GetComponent<ActionButtonUI>();
            actionButtonUI.SetBaseAction(baseAction);
            actionButtons.Add(actionButtonUI);
        }
    }

    private void DeleteExistingButtons()
    {
        actionButtons.Clear();

        foreach (Transform buttonTransform in actionButtonContainer)
        {
            Destroy(buttonTransform.gameObject);
        }
    }

    private void UnitActionSystem_OnSelectedUnitChange()
    {
        DeleteExistingButtons();
        CreateUnitActionButtons();
        UpdateSelectedVisual();
        UpdateActionPoints();
    }

    private void UpdateSelectedVisual()
    {
        foreach (ActionButtonUI actionButton in actionButtons)
        {
            actionButton.UpdateSelectedVisual();
        }
    }

    private void UpdateActionPoints()
    {
        Unit unit = UnitActionSystem.Instance.GetSelectedUnit();
        actionPointsText.text = $"Action Points: {unit.GetActionPoints()}";
    }

    private void UnitActionSystem_OnSelectedActionChange()
    {
        UpdateSelectedVisual();
    }

    private void UnitActionSystem_OnActionStarted()
    {
        UpdateActionPoints();
    }

    private void TurnSystem_OnTurnChanged()
    {
        UpdateActionPoints();
    }

    private void Unit_OnAnyActionPointsChanged()
    {
        UpdateActionPoints();
    }
}
