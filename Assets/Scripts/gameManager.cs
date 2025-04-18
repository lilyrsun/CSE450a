using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class gameManager : MonoBehaviour
{
    public static gameManager Instance;
    public GameState Gamestate;

    public int gold;
    public int goldPerTurn = 10;
    public int upgradeGoldCost = 50;
    public TMPro.TextMeshProUGUI goldDisplay;
    public int turnTrack = 1;
    public int spawnTrack = 5;
    public int upCount = 0;

    public GameObject PlayAgainButton;
    public GameObject GameOverPanel;  
    public TextMeshProUGUI GameOverText;

    public AudioClip moveUnitClip;
    public AudioClip selectUnitClip;
    public AudioClip attackUnitClip;
    private AudioSource audioSource;

    public Button EndTurnButton, UpgradeGoldButton, SpawnFriendlyUnitButton;

    private void Update()
    {
        goldDisplay.text = $"Gold: {gold}";
    }

    private void Awake()
    {
        Instance = this;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

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
        Debug.Log("Changing game state to: " + newState);
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
                turnTrack += 1;
                Debug.Log("NPC TURN START");
                if(turnTrack == spawnTrack)
                {
                    //spawn
                    unitManager.Instance.SpawnNPCWave();
                    spawnTrack += 5;
                }
                MoveAllNPCs();
                CheckGameOver();
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
        Debug.Log("Game state changed successfully to: " + newState);

    }
     private void CheckGameOver()
    {
        if (unitManager.Instance.SpawnedTownHall == null) // ✅ If player's town hall is destroyed
        {
            Debug.Log("🏰 Player's town hall is destroyed! Game over.");
            ChangeState(GameState.Lose);
            return;
        }

        var allEnemies = FindObjectsOfType<baseNPCUnit>();
        if (allEnemies.Length == 0) // ✅ If all enemies are gone
        {
            Debug.Log("🏆 All enemies defeated! You win.");
            ChangeState(GameState.Win);
            return;
        }
    }

    public void UpdateUpgradeButtonDisplay()
    {
        if (UpgradeGoldButton != null)
        {
            TextMeshProUGUI buttonText = UpgradeGoldButton.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = "2x Gold (" + upgradeGoldCost.ToString() + ")";
            }
        }

        if (SpawnFriendlyUnitButton != null)
        { 
            TextMeshProUGUI buttonText = SpawnFriendlyUnitButton.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = "Recruit Unit (" + unitManager.Instance.spawnUnitCost.ToString() + ")";
            }
        }
        if (SpawnFriendlyUnitButton != null)
        {
            TextMeshProUGUI buttonText = SpawnFriendlyUnitButton.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                if (unitManager.Instance.spawnUnitCost >= 2000000)
                {
                    buttonText.text = "MAX";
                }
            }
        }
    }
    private void AddGold()
    {
        int unitCount = Mathf.Max(0, unitManager.Instance.GetPlayerUnitCount() - 2); // Subtract 2 for initial tower & unit
        int maintenanceCost = unitCount * 1;

        gold += goldPerTurn;
        gold -= maintenanceCost;

        if (gold < 0) gold = 0;
        if (gold >= 2000000) gold = 2000000;

        UpdateGoldUI();
        Debug.Log($"Turn Gold: +{goldPerTurn}, Maintenance: -{maintenanceCost}, Total Gold: {gold}");
    }




    public void UpgradeGold()
    {
        if (gold >= upgradeGoldCost && upCount <= 17)
        {
            gold -= upgradeGoldCost;
            goldPerTurn *= 2;
            upgradeGoldCost = Mathf.RoundToInt(upgradeGoldCost * 1.5f);
            UpdateGoldUI();
            UpdateUpgradeButtonDisplay();
            upCount++;
        }
        if (upCount >= 18)
        {
            TextMeshProUGUI buttonText = UpgradeGoldButton.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = "MAX";
            }
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

    public void RegeneratePlayerUnits()
    {
        foreach (var unit in FindObjectsOfType<basePlayerUnit>())
        {
            unit.RegenerateHealth(10); 
        }
    }

    public void CheckIfAllPlayerUnitsMoved()
    {
        var allPlayerUnits = FindObjectsOfType<basePlayerUnit>();

        // Only count units that are NOT the town hall
        var movableUnits = allPlayerUnits.Where(unit => unit != unitManager.Instance.SpawnedTownHall).ToList();

        bool allMoved = movableUnits.All(unit => unit.hasMoved);

        Debug.Log("All movable player units moved: " + allMoved);
        EndTurnButton.GetComponent<EndTurnButton>().SetGlow(allMoved);
    }
    public void RevealTilesAroundUnits()
    {
        var allTiles = FindObjectsOfType<Tile>();
        var playerUnits = FindObjectsOfType<basePlayerUnit>();

        foreach (var tile in allTiles)
        {
            tile.isVisible = false; // Reset visibility
        }

        foreach (var unit in playerUnits)
        {
            if (unit == unitManager.Instance.SpawnedTownHall) continue;

            var origin = unit.OccuppiedTile;
            if (origin == null) continue;

            foreach (var tile in allTiles)
            {
               float distance = Vector2.Distance(tile.transform.position, origin.transform.position);
                if (distance <= 4f)
                {
                    tile.isVisible = true;
                    tile.isRevealed = true;
                }
            }
        }

        foreach (var tile in allTiles)
        {
            tile.UpdateFog();
        }
    }

    public void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
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
