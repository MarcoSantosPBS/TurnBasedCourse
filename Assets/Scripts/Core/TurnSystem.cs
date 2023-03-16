using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    private int turn = 1;
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
        OnTurnChanged?.Invoke();
    }

    public int GetTurn() => turn;
}
