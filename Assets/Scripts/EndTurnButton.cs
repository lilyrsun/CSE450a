using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTurnButton : MonoBehaviour
{
    [SerializeField] private Button endTurnButton;
    [SerializeField] private GameObject glowRadiusObject;

    private void Awake()
    {
        if (endTurnButton != null)
        {
            endTurnButton.onClick.AddListener(EndTurn);
        }

        if (glowRadiusObject != null)
        {
            glowRadiusObject.SetActive(false);
        }
    }

    private void EndTurn()
    {
        Debug.Log("End Turn button pressed.");
        
        if (gameManager.Instance == null)
        {
            Debug.LogError("GameManager instance is NULL! The script might not be assigned in the scene.");
            return; // Prevent further execution if GameManager is missing
        }

        gameManager.Instance.RegeneratePlayerUnits();
        
        Debug.Log("Healing applied. Resetting all units.");
        ResetAllUnits();

        Debug.Log("Switching to NPC turn...");
        gameManager.Instance.ChangeState(GameState.EnemiesTurn);

        Debug.Log("NPC turn started successfully.");

        SetGlow(false);
    }

    private void ResetAllUnits()
    {
        var allTiles = FindObjectsOfType<Tile>();
        foreach (var tile in allTiles)
        {
            if (tile.OccupiedUnit != null && tile.OccupiedUnit is basePlayerUnit playerUnit)
            {
                //playerUnit.hasMoved = false;
                //playerUnit.hasAttacked = false;
                playerUnit.ResetTurnState();
            }
        }
    }

    public void SetGlow(bool glow)
    {
        Debug.Log("SetGlow called. Setting glow to: " + glow);

        if (glowRadiusObject != null)
        {
            glowRadiusObject.SetActive(glow);
        }
        else
        {
            Debug.LogWarning("glowRadiusObject is not assigned in the Inspector.");
        }
    }

}
