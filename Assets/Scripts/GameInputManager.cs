using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class GameInputManager : Singleton<GameInputManager>
{
    private int numberOfPlayers = 2;
    private Rewired.Player[] rewiredPlayers;

    private Vector2[] actionMovements;
    private bool[] action;

    void Awake()
    {
        rewiredPlayers = new Rewired.Player[numberOfPlayers];
        for(int i=0; i<numberOfPlayers; i++)
        {
            rewiredPlayers[i] = ReInput.players.GetPlayer(i);
            rewiredPlayers[i].controllers.maps.SetMapsEnabled(true, 0);
        }
    }

    public void SetPlayerInputMode(int playerId, int inputMode)
    {
        rewiredPlayers[playerId].controllers.maps.SetMapsEnabled(true, inputMode);
        rewiredPlayers[playerId].controllers.maps.SetMapsEnabled(true, (inputMode+1)%2);
    }

    //Returns a Vector2 used for movement in the Action phase
    public Vector2 ReadPlayerActionPhaseMovement(int playerId)
    {
        Vector2 movement = new Vector2(rewiredPlayers[playerId].GetAxis(actionHorizontalMovement),
                                        rewiredPlayers[playerId].GetAxis(actionVerticalMovement));
        
        return movement.normalized;
    }

    //Returns a Vector2Int used for input in the Tetris phase
    public Vector2Int ReadPlayerTetrisPhaseMovement(int playerId)
    {
        Vector2Int digitalInput = new Vector2Int(0, 0);
        if(rewiredPlayers[playerId].GetButtonDown(actionLeft))
        {
            digitalInput.x = -1;
        }
        else if(rewiredPlayers[playerId].GetButtonDown(actionRight))
        {
            digitalInput.x = 1;
        }

        if(rewiredPlayers[playerId].GetButtonDown(actionUp))
        {
            digitalInput.y = 1;
        }
        else if(rewiredPlayers[playerId].GetButtonDown(actionDown))
        {
            digitalInput.y = -1;
        }

        return digitalInput;
    }

    //Used in both Action and Tetris phase
    public bool ReadPlayerActionButton(int playerId)
    {
        return rewiredPlayers[playerId].GetButtonDown(actionButtonId);
    }

    //Used in both Action and Tetris phase
    public bool ReadPlayerSecondaryButton(int playerId)
    {
        return rewiredPlayers[playerId].GetButtonDown(secondaryButtonId);
    }

    //Used in both Action and Tetris phase
    public bool ReadPlayerConfirmButton(int playerId)
    {
        return rewiredPlayers[playerId].GetButtonDown(confirmButtonId);
    }

    private string actionHorizontalMovement = "HorizontalMovement";
    private string actionVerticalMovement = "VerticalMovement";
    private string actionLeft = "Left";
    private string actionRight = "Right";
    private string actionUp = "Up";
    private string actionDown = "Down";
    private int actionButtonId = 6;
    private int secondaryButtonId = 7;
    private int confirmButtonId = 8;
}
