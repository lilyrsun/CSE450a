using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;
    [SerializeField] private bool _isWalkable;

    public baseUnit OccupiedUnit;
    public bool walkable => _isWalkable && OccupiedUnit == null;

	public virtual void Init(int x, int y){
	}

    private bool isHighlighted = false;

    void OnMouseEnter () {
        _highlight.SetActive(true);
    }

    void OnMouseExit() {
        if (!isHighlighted) 
        {
            _highlight.SetActive(false);
        }
    }

    private void OnMouseDown()
    {
        if (gameManager.Instance.Gamestate != GameState.PlayersTurn) return;

        if (OccupiedUnit != null)
        {
            if (OccupiedUnit.Faction == Faction.Player)
            {
                unitManager.Instance.setSelectedPlayerUnit((basePlayerUnit)OccupiedUnit);
                if (OccupiedUnit != unitManager.Instance.SpawnedTownHall && !unitManager.Instance.SelectedBasePlayerUnit.hasMoved)
                {
                    HighlightMoveableTiles(OccupiedUnit);
                }
                //Debug.Log(unitManager.Instance.SelectedBasePlayerUnit);
                //Debug.Log(OccupiedUnit);
                //Debug.Log(unitManager.Instance.SpawnedTownHall);
            }
            else
            {
                if (unitManager.Instance.SelectedBasePlayerUnit != null && !unitManager.Instance.SelectedBasePlayerUnit.hasMoved)
                {
                    var enemy = (baseNPCUnit)OccupiedUnit;
                    if (IsWithinMoveRange(unitManager.Instance.SelectedBasePlayerUnit.OccuppiedTile, this))
                    {
                        //attack logic here
                        enemy.TakeDamage(34);
                        Debug.Log(enemy.Health.getHealth());
                        unitManager.Instance.SelectedBasePlayerUnit.hasMoved = true;
                        unitManager.Instance.setSelectedPlayerUnit(null);
                        ClearHighlightedTiles();
                       
                    }
                }
            }
        }
        else
        {
            if (unitManager.Instance.SelectedBasePlayerUnit != null && unitManager.Instance.SelectedBasePlayerUnit != unitManager.Instance.SpawnedTownHall && !unitManager.Instance.SelectedBasePlayerUnit.hasMoved)
            {
                if (IsWithinMoveRange(unitManager.Instance.SelectedBasePlayerUnit.OccuppiedTile, this))
                {
                    //Debug.Log("HERE");
                    /*Debug.Log(unitManager.Instance.SelectedBasePlayerUnit);
                    Debug.Log("THISTHISTHIS");*/
                    SetUnit(unitManager.Instance.SelectedBasePlayerUnit);
                    unitManager.Instance.SelectedBasePlayerUnit.hasMoved = true;
                    unitManager.Instance.setSelectedPlayerUnit(null);
                    ClearHighlightedTiles();
                }
            }
        }
    }


    public void SetUnit(baseUnit unit)
    {
        if (unit.OccuppiedTile != null) unit.OccuppiedTile.OccupiedUnit = null;
        unit.transform.position = transform.position;
        OccupiedUnit = unit;
        unit.OccuppiedTile = this;
       // Debug.Log($"tile: {name}");
       // Debug.Log($"Unit is now on tile: {transform.position}"); //+4 -4 y  x can go -1 +1
    }

    public bool IsWithinMoveRange(Tile currentTile, Tile targetTile)
{
    Vector2 currentPos = currentTile.transform.position;
    Vector2 targetPos = targetTile.transform.position;

    float xDif = Mathf.Abs(currentPos.x - targetPos.x);
    float yDif = Mathf.Abs(currentPos.y - targetPos.y);
        //Debug.Log(xDif);
        //Debug.Log(yDif);

        //Debug.Log("THERE");
        return (xDif <= 2 && yDif <= 2) || (yDif <= 2 && xDif == 0); 
}

    private void HighlightMoveableTiles(baseUnit unit)
    {
        ClearHighlightedTiles();
        var allTiles = FindObjectsOfType<Tile>();

        foreach (var tile in allTiles)
        {
            if (IsWithinMoveRange(unit.OccuppiedTile, tile) && tile.walkable)
            {
                tile._highlight.SetActive(true);
                tile.isHighlighted = true;
            }
        }
    }

    private void ClearHighlightedTiles()
    {
        var allTiles = FindObjectsOfType<Tile>();
        foreach (var tile in allTiles)
        {
            tile._highlight.SetActive(false);
            tile.isHighlighted = false;
        }
    }

  /*  public void ResetAllUnits()
    {
        var allTiles = FindObjectsOfType<Tile>();
        foreach (var tile in allTiles)
        {
            if (tile.OccupiedUnit != null && tile.OccupiedUnit is basePlayerUnit playerUnit)
            {
                playerUnit.hasMoved = false;
            }
        }
    }*/

}
