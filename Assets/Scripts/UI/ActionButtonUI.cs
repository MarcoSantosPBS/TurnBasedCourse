using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMeshProUGUI;
    [SerializeField] Button button;
    [SerializeField] Image selectedImage;

    BaseAction action;

    public void SetBaseAction(BaseAction action)
    {
        textMeshProUGUI.text = action.GetActionName().ToUpper();
        button.onClick.AddListener(() =>
        {
            UnitActionSystem.Instance.SetSelectedAction(action);
        });

        this.action = action;
    }

    public void UpdateSelectedVisual()
    {
        bool isSelected = UnitActionSystem.Instance.GetSelectedAction() == action;
        selectedImage.enabled = isSelected;
    }
}
