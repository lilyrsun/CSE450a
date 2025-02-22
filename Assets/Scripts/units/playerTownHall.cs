using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerTownHall : basePlayerUnit
{
    public override void Die()
    {
        Debug.Log("YOU LOSE!");
        gameManager.Instance.ChangeState(GameState.Lose);
    }

}
