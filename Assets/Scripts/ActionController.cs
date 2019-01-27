using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour
{
    public void SetupListeners(Character c)
    {
        c.DashLanded += OnDashLanded;
    }

    public void UpdateCharacter(Character c)
    {
        bool secondaryButtonPressed = GameInputManager.Instance.ReadPlayerSecondaryButton(c.playerId);
        if(secondaryButtonPressed)
        {
            c.PerformSecondary();
        }
        else
        {
            //Movement
            Vector2 movement = GameInputManager.Instance.ReadPlayerActionPhaseMovement(c.playerId);
            c.Move(movement);
        }

        if(GameInputManager.Instance.ReadPlayerActionButton(c.playerId))
        {
            c.PerformAction();
        }
    }

    //--------------- Events
    private void OnDashLanded(Vector2 direction, Character victim)
    {
        victim.OnHit(direction);
    }
}
