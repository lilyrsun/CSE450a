using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class gameManager : MonoBehaviour
{
    public static gameManager Instance;
    public GameState Gamestate;

    public int gold;
    public int goldPerTurn = 10;
    public int upgradeGoldCost = 50;
    public TMPro.TextMeshProUGUI goldDisplay;

    public GameObject PlayAgainButton;
    public GameObject GameOverPanel;  
    public TextMeshProUGUI GameOverText;  

    public Button EndTurnButton, UpgradeGoldButton, SpawnFriendlyUnitButton;

    private void Update()
    {
        goldDisplay.text = $"Gold: {gold}";
    }

    private void Awake()
    {
        Instance = this;

        if (PlayAgainButton != null) {
            PlayAgainButton.SetActive(false);
        }

        if (GameOverPanel != null) {
            GameOverPanel.SetActive(false);  
        }
    }

    private void Start()
    {
        ChangeState(GameState.GenerateGrid);
    }

    public void ChangeState(GameState newState)
    {
        Gamestate = newState;

        if (newState == GameState.Win || newState == GameState.Lose)
        {
            EndTurnButton.interactable = false;
            UpgradeGoldButton.interactable = false;
            SpawnFriendlyUnitButton.interactable = false;
        }
        else
        {
            EndTurnButton.interactable = true;
            UpgradeGoldButton.interactable = true;
            SpawnFriendlyUnitButton.interactable = true;
        }

        if (newState == GameState.Win || newState == GameState.Lose)
        {
            GameOverPanel.SetActive(true);  
            GameOverText.text = (newState == GameState.Win) ? "YOU WIN!" : "YOU LOSE!";
        }
        else
        {
            GameOverPanel.SetActive(false);
            PlayAgainButton.SetActive(false);
        }

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
                AddGold();
                Debug.Log("PLAYER TURN START");
                break;
            case GameState.EnemiesTurn:
                Debug.Log("NPC TURN START");
                MoveAllNPCs();
                this.ChangeState(GameState.PlayersTurn);
                break;
            case GameState.Win:
                Debug.Log("We made IT TO HERE Yo");
                PlayAgainButton.SetActive(true);
                break;
            case GameState.Lose:
                Debug.Log("We made IT TO HERE MAN");
                PlayAgainButton.SetActive(true);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }

    public void UpdateUpgradeGoldButtonDisplay()
    {
        if (UpgradeGoldButton != null)
        {
            TextMeshProUGUI buttonText = UpgradeGoldButton.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = "2x Gold (" + upgradeGoldCost.ToString() + ")";
            }
        }
    }

    private void AddGold()
    {
        gold += goldPerTurn;
        UpdateGoldUI();
    }

    public void UpgradeGold()
    {
        if (gold >= upgradeGoldCost)
        {
            gold -= upgradeGoldCost;
            goldPerTurn *= 2;
            upgradeGoldCost = Mathf.RoundToInt(upgradeGoldCost * 1.5f);
            UpdateGoldUI();
            UpdateUpgradeGoldButtonDisplay();
        }
    }

    public void UpdateGoldUI()
    {
        if (goldDisplay != null)
        {
            goldDisplay.text = $"Gold: {gold}";
        }
    }

    private void MoveAllNPCs()
    {
        StartCoroutine(MoveAllNPCsCoroutine());
    }

    private IEnumerator MoveAllNPCsCoroutine()
    {
        var allUnits = FindObjectsOfType<baseNPCUnit>();
        float delayTime = 0.12f;

        foreach (var unit in allUnits)
        {
            if (unit is NPCTownHall)
            {
                continue;
            }
            unit.MoveNPCToBase(unit);
            yield return new WaitForSeconds(delayTime);
        }
    }
}

public enum GameState
{
    GenerateGrid,
    SpawnUnit,
    SpawnEnemies,
    PlayersTurn,
    EnemiesTurn,
    Lose,
    Win
}
