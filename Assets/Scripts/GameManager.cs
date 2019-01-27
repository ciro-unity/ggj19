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
    public Transform[] characterSpawnPoints;

    private void Awake()
    {
        foreach(Character c in characters)
        {
            actionController.SetupListeners(c);
        }
    }

    private void Start()
    {
        RespawnCharacter(0);
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

    public void StartTetrisMode(int playerId, GameObject tetrisPiecePrefab)
    {
        float xOffset = (playerId == 0) ? -35 : 35f;
        tetrisControllers[playerId].transform.parent.DOMoveX(xOffset, .3f).SetEase(Ease.InOutBack);
        tetrisControllers[playerId].StartTetrisMode(tetrisPiecePrefab);
        
        characters[playerId].StartTetrisMode();
    }

    public void EndTetrisMode(int playerId)
    {
        float xOffset = (playerId == 0) ? -70f : 70f;
        tetrisControllers[playerId].transform.parent.DOMoveX(xOffset, .3f).SetEase(Ease.InOutBack);
        
        characters[playerId].StartActionMode();
    }

    public void RespawnCharacter(int playerId)
    {
        characters[playerId].Respawn(characterSpawnPoints[playerId].position);
    }
}
