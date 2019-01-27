using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
    public TetrisController[] tetrisControllers;

    private void Awake()
    {
        foreach(Character c in characters)
        {
            actionController.SetupListeners(c);
        }
    }

    private void Start()
    {
        
    }

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
                        
                        //TEST
                        if(GameInputManager.Instance.ReadPlayerConfirmButton(c.playerId))
                            StartTetrisMode(c.playerId);
                    }
                    else
                    {
                        //TODO: add Tetris Controller stuff

                        //TEST
                        if(GameInputManager.Instance.ReadPlayerConfirmButton(c.playerId))
                            EndTetrisMode(c.playerId);
                    }
                }
            break;
        }
    }

    public void StartTetrisMode(int playerId)
    {
        characters[playerId].StartTetrisMode();
        float xOffset = (playerId == 0) ? -35f : 35f;
        tetrisControllers[playerId].transform.DOMoveX(xOffset, .3f).SetEase(Ease.InOutBack);
    }

    public void EndTetrisMode(int playerId)
    {
        characters[playerId].StartActionMode();
        float xOffset = (playerId == 0) ? -55f : 55f;
        tetrisControllers[playerId].transform.DOMoveX(xOffset, .3f).SetEase(Ease.InOutBack);
    }
}
