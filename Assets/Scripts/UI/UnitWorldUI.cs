using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UnitWorldUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI actionPointsText;
    [SerializeField] private Unit unit;
    [SerializeField] private Image healthBarImage;
    [SerializeField] private HealthSystem healthSystem;

    private void Start()
    {
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;
        healthSystem.OnDamage += HealthSystem_OnDamage;
        Unit_OnAnyActionPointsChanged();
        UpdateHealthBar();
    }

    private void UpdateActionPointsText()
    {
        actionPointsText.text = $"{unit.GetActionPoints()}";
    }

    private void UpdateHealthBar()
    {
        healthBarImage.fillAmount = healthSystem.GetHealthNormalized();
    }

    private void Unit_OnAnyActionPointsChanged()
    {
        UpdateActionPointsText();
    }

    private void HealthSystem_OnDamage()
    {
        UpdateHealthBar();
    }
}
