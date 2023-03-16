using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] Button nextTurnButton;
    [SerializeField] TextMeshProUGUI turnText;
    
    private void Start()
    {
        nextTurnButton.onClick.AddListener(() =>
        {
            TurnSystem.Instance.NextTurn();
        });

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        UpdateTurnText();
    }

    private void UpdateTurnText()
    {
        turnText.text = $"Turn: {TurnSystem.Instance.GetTurn()}";
    }

    private void TurnSystem_OnTurnChanged()
    {
        UpdateTurnText();
    }
}
