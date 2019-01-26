using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour
{
    public void UpdateCharacter(Character c)
    {
        //Movement
        Vector2 movement = GameInputManager.Instance.ReadPlayerActionPhaseMovement(c.playerId);
        c.Move(movement);

        if(GameInputManager.Instance.ReadPlayerActionButton(c.playerId))
        {
            c.PerformAction();
        }

        if(GameInputManager.Instance.ReadPlayerSecondaryButton(c.playerId))
        {
            c.PerformSecondary();
        }
    }
}
