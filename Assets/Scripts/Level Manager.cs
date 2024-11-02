using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public UIManager UIManager;
    public SoundManager soundManager;
    [SerializeField] private GameState _gameState;
    void Start()
    {
        _gameState = GameState.start;
    }

    public GameState GetGameState()
    {
        return _gameState;
    }

    public void SetGameState(GameState gameState)
    {
        if (gameState == GameState.start)
        {
            UIManager.SetGameStateUI(GameState.start);
        }
        else if (gameState == GameState.play)
        {
            UIManager.SetGameStateUI(GameState.play);
        }
        else if (gameState == GameState.win)
        {
            UIManager.SetGameStateUI(GameState.win);
        }
        else if (gameState == GameState.lose)
        {
            UIManager.SetGameStateUI(GameState.lose);
        }
        _gameState = gameState;
    }

    public void StartGame()
    {
        SetGameState(GameState.play);
    }

    public void GetMoney()
    {

    }

    public void GetDrink()
    {
        
    }
}
