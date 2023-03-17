using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    private int turn = 1;
    private bool isPlayerTurn = true;
    public static TurnSystem Instance;
    public event Action OnTurnChanged;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("There's already an instance of Turn System instantiated");
            Destroy(gameObject);
        }

        Instance = this;
    }

    public void NextTurn()
    {
        turn++;
        isPlayerTurn = !isPlayerTurn;
        OnTurnChanged?.Invoke();
    }

    public int GetTurn() => turn;
    public bool IsPlayerTurn() => isPlayerTurn;
}
