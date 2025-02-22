using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTurnButton : MonoBehaviour
{
    [SerializeField] private Button endTurnButton;

    private void Awake()
    {
        if (endTurnButton != null)
        {
            endTurnButton.onClick.AddListener(EndTurn);
        }
    }

    private void EndTurn()
    {
        ResetAllUnits();
        //Debug.Log("Turn Over");
        gameManager.Instance.ChangeState(GameState.EnemiesTurn);
    }


    private void ResetAllUnits()
    {
        var allTiles = FindObjectsOfType<Tile>();
        foreach (var tile in allTiles)
        {
            if (tile.OccupiedUnit != null && tile.OccupiedUnit is basePlayerUnit playerUnit)
            {
                playerUnit.hasMoved = false;
            }
        }
    }

}
