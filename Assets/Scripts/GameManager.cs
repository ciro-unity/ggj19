using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Character[] characters;

    private GameState gameState = GameState.Gameplay;
    private enum GameState
    {
        Gameplay,
        Menus,
        Pause,
        GameOver,
    }
    
    public ActionController actionController;

    private void Update()
    {
        switch(gameState)
        {
            case GameState.Gameplay:
                foreach(Character c in characters)
                {
                    if(c.playMode == Character.PlayMode.Action)
                    {
                        actionController.UpdateCharacter(c);
                    }
                    else
                    {
                        //TODO: add Tetris Controller stuff
                        
                    }
                }
            break;
        }
    }
}
