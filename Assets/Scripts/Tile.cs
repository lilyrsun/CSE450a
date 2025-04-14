using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile : MonoBehaviour
{
    // NEW: Tile type enum and field.
    public enum TileType { Grass, Water, Mountain }
    public TileType tileType;

    [SerializeField] protected SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;
    [SerializeField] private GameObject _AttackHighlight;
    [SerializeField] private bool _isWalkable;

    public baseUnit OccupiedUnit;
    public bool walkable => _isWalkable && OccupiedUnit == null;

    public virtual void Init(int x, int y)
    {
        // Optionally, store grid coordinates if needed.
    }

    private bool isHighlighted = false;

    void OnMouseEnter()
    {
        _highlight.SetActive(true);
    }

    void OnMouseExit()
    {
        if (!isHighlighted)
        {
            _highlight.SetActive(false);
        }
    }

    void Start()
    {
        ClearHighlightedTiles();
    }

    private void OnMouseDown()
    {
        if (gameManager.Instance.Gamestate != GameState.PlayersTurn) return;

        if (OccupiedUnit != null)
        {
            if (OccupiedUnit.Faction == Faction.Player)
            {
                unitManager.Instance.setSelectedPlayerUnit((basePlayerUnit)OccupiedUnit);
                if (!OccupiedUnit.hasMoved)
                {
                    HighlightMoveableTiles(OccupiedUnit);
                }
            }
            else
            {
                if (unitManager.Instance.SelectedBasePlayerUnit != null &&
                    !unitManager.Instance.SelectedBasePlayerUnit.hasMoved)
                {
                    var enemy = (baseNPCUnit)OccupiedUnit;
                    if (IsWithinMoveRange(unitManager.Instance.SelectedBasePlayerUnit.OccuppiedTile, this))
                    {
                        enemy.TakeDamage(34);
                        unitManager.Instance.SelectedBasePlayerUnit.hasMoved = true;
                        gameManager.Instance.CheckIfAllPlayerUnitsMoved();

                        unitManager.Instance.SelectedBasePlayerUnit.hasAttacked = true;
                        unitManager.Instance.SelectedBasePlayerUnit.UpdateMoveIndicator();
                        unitManager.Instance.SelectedBasePlayerUnit.hasAttacked = true;
                        unitManager.Instance.setSelectedPlayerUnit(null);
                        ClearHighlightedTiles();
                    }
                }
            }
        }
        else
        {
            if (unitManager.Instance.SelectedBasePlayerUnit != null &&
                unitManager.Instance.SelectedBasePlayerUnit != unitManager.Instance.SpawnedTownHall &&
                !unitManager.Instance.SelectedBasePlayerUnit.hasMoved)
            {
                if (IsWithinMoveRange(unitManager.Instance.SelectedBasePlayerUnit.OccuppiedTile, this) && walkable)
                {
                    SetUnit(unitManager.Instance.SelectedBasePlayerUnit);
                    unitManager.Instance.SelectedBasePlayerUnit.hasMoved = true;
                    gameManager.Instance.CheckIfAllPlayerUnitsMoved();

                    unitManager.Instance.SelectedBasePlayerUnit.UpdateMoveIndicator();
                    unitManager.Instance.SelectedBasePlayerUnit.hasAttacked = true;
                    unitManager.Instance.setSelectedPlayerUnit(null);
                    ClearHighlightedTiles();
                }
            }
        }
    }

    public void SetUnit(baseUnit unit)
    {
        if (unit.OccuppiedTile != null)
            unit.OccuppiedTile.OccupiedUnit = null;
        unit.transform.position = transform.position;
        OccupiedUnit = unit;
        unit.OccuppiedTile = this;
        gameManager.Instance.RevealTilesAroundUnits();
    }

    public bool IsWithinMoveRange(Tile currentTile, Tile targetTile)
    {
        Vector2 currentPos = currentTile.transform.position;
        Vector2 targetPos = targetTile.transform.position;

        float xDif = Mathf.Abs(currentPos.x - targetPos.x);
        float yDif = Mathf.Abs(currentPos.y - targetPos.y);

        bool inWater = currentTile is waterTile;
        if (inWater)
        {
            return (xDif <= 1 && yDif <= 1);
        }

        return (xDif <= 2 && yDif <= 2) || (yDif <= 2 && xDif == 0);
    }

    private void HighlightMoveableTiles(baseUnit unit)
    {
        ClearHighlightedTiles();
        var allTiles = FindObjectsOfType<Tile>();

        foreach (var tile in allTiles)
        {
            if (IsWithinMoveRange(unit.OccuppiedTile, tile))
            {
                if (tile.OccupiedUnit != null && tile.OccupiedUnit.Faction != unit.Faction)
                {
                    tile._AttackHighlight.SetActive(true);
                }
                else if (tile.walkable)
                {
                    tile._highlight.SetActive(true);
                }

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
            tile._AttackHighlight.SetActive(false); // Reset enemy highlight
            tile.isHighlighted = false;

        }
    }

    public bool isRevealed = false;
    public bool isVisible = false;
    public GameObject fogOverlay; // Drag a dark semi-transparent sprite over each tile in the editor

    public void UpdateFog()
    {
        if (fogOverlay != null && fogOverlay.GetComponent<SpriteRenderer>() != null)
        {
            bool shouldBeActive = !isVisible && !isRevealed;
            fogOverlay.SetActive(shouldBeActive);
        }
    }
}
