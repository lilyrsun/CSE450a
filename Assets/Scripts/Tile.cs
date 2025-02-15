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
	
    void OnMouseEnter () {
        _highlight.SetActive(true);
    }

    void OnMouseExit() {
        _highlight.SetActive(false);
    }

    private void OnMouseDown()
    {
        if (gameManager.Instance.Gamestate != GameState.PlayersTurn) return;

        if (OccupiedUnit != null)
        {
            if (OccupiedUnit.Faction == Faction.Player) unitManager.Instance.setSelectedPlayerUnit((basePlayerUnit)OccupiedUnit);
            else
            {
                if (unitManager.Instance.SelectedBasePlayerUnit != null)
                {
                    var enemy = (baseNPCUnit)OccupiedUnit;
                    //attack logic here
                    unitManager.Instance.setSelectedPlayerUnit(null);
                }
            }
        }
        else
        {
            if (unitManager.Instance.SelectedBasePlayerUnit != null)
            {
                SetUnit(unitManager.Instance.SelectedBasePlayerUnit);
                unitManager.Instance.setSelectedPlayerUnit(null);
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

}
