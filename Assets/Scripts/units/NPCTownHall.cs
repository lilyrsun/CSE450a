using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTownHall : baseNPCUnit
{
    public override void Die()
    {
        Debug.Log("YOU WIN!");
        gameManager.Instance.ChangeState(GameState.Win);

    }
}
