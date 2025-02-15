using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    public static gameManager Instance;
    public GameState Gamestate;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ChangeState(GameState.GenerateGrid);
    }

    public void ChangeState(GameState newState)
    {
        Gamestate = newState;
        switch (newState)
        {
            case GameState.GenerateGrid:
                GridManager.Instance.GenerateGrid();
                break;
            case GameState.SpawnUnit:
                unitManager.Instance.SpawnUnit();
                break;
            case GameState.SpawnEnemies:
                unitManager.Instance.SpawnNPCUnits();
                break;
            case GameState.PlayersTurn:
                break;
            case GameState.EnemiesTurn:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }
}

public enum GameState
{
    GenerateGrid,
    SpawnUnit,
    SpawnEnemies,
    PlayersTurn,
    EnemiesTurn
}
