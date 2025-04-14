using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class baseNPCUnit : baseUnit
{

   public override void TakeDamage(int damage)
    {
        Debug.Log("⚔️ NPC TakeDamage() Called - Before: " + Health.getHealth());

        base.TakeDamage(damage); 

        Debug.Log("⚔️ NPC TakeDamage() Called - After: " + Health.getHealth());
    }


    public void MoveNPCToBase(baseNPCUnit npc)
    {

        Tile currentTile = npc.OccuppiedTile;
        if (currentTile == null) return;
        Tile targetPlayerTile = FindPlayerUnitInRange(currentTile);

        if (targetPlayerTile != null)
        {
            // NPC ATTACK LOGIC
            basePlayerUnit playerUnit = (basePlayerUnit)targetPlayerTile.OccupiedUnit;
            if (playerUnit != null)
            {
                playerUnit.TakeDamage(35);;
                Debug.Log("NPC Attacked Player Unit! HP Remaining " + playerUnit.Health.getHealth());
            }
            else
            {
                Debug.Log("YOU ARE ALREADY DEAD");
            }
            return;
        }

        Vector2Int currentTilePos = new Vector2Int((int)currentTile.transform.position.x, (int)currentTile.transform.position.y);
        Vector2Int targetTilePos = new Vector2Int(-5, 0);

        int moveX = 0, moveY = 0;

        if (currentTilePos.x > targetTilePos.x + 1) moveX = -1;
        else if (currentTilePos.x < targetTilePos.x - 1) moveX = 1;

        if (currentTilePos.y > targetTilePos.y + 1) moveY = -1;
        else if (currentTilePos.y < targetTilePos.y - 1) moveY = 1;

        Tile bestTile = FindTileAtPosition(currentTilePos.x + moveX, currentTilePos.y + moveY);

        if (bestTile != null && bestTile.walkable)
        {
            Vector2 pos = bestTile.transform.position;
            if ((int)pos.x == 10 && (int)pos.y == 12)
            {
                return; 
            }
            currentTile.OccupiedUnit = null;
            Debug.Log("Moving to " + bestTile + "!");
            bestTile.SetUnit(npc);
            npc.hasMoved = true;
        }
    }

    private Tile FindTileAtPosition(int x, int y)
    {
        foreach (var tile in FindObjectsOfType<Tile>())
        {
            if ((int)tile.transform.position.x == x && (int)tile.transform.position.y == y)
                return tile;
        }
        return null;
    }
    


    private Tile FindPlayerUnitInRange(Tile npcTile)
    {

        foreach (var tile in FindObjectsOfType<Tile>())
        {
            if (npcTile.IsWithinMoveRange(npcTile, tile) && tile.OccupiedUnit is basePlayerUnit)
            {
                basePlayerUnit playerUnit = (basePlayerUnit)tile.OccupiedUnit;

                if (playerUnit != null && playerUnit.Health.getHealth() > 0)
                {
                    return tile;
                }
            }
        }
        return null;
    }

}
