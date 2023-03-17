using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] Button nextTurnButton;
    [SerializeField] TextMeshProUGUI turnText;
    [SerializeField] Transform enemyTurnUI;
    
    private void Start()
    {
        nextTurnButton.onClick.AddListener(() =>
        {
            TurnSystem.Instance.NextTurn();
        });

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        UpdateTurnText();
        UpdateEnemyTurnUIVisibility();
        UpdateEndTurnVisibility();
    }

    private void UpdateTurnText()
    {
        turnText.text = $"Turn: {TurnSystem.Instance.GetTurn()}";
    }

    private void TurnSystem_OnTurnChanged()
    {
        UpdateEnemyTurnUIVisibility();
        UpdateEndTurnVisibility();
        UpdateTurnText();
    }

    private void UpdateEndTurnVisibility()
    {
        nextTurnButton.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn());
    }

    private void UpdateEnemyTurnUIVisibility()
    {
        enemyTurnUI.gameObject.SetActive(!TurnSystem.Instance.IsPlayerTurn());
    }
}
