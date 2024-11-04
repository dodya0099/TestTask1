using Enums;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public UIManager UIManager;
    public UIAnimationManager UIAnimationManager;
    public SoundManager soundManager;
    [SerializeField] private GameState _gameState;
    public PlayerController player;
    private int _currentMoney = 0;
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
        _currentMoney += 2;
        UIManager.SetUIMoney(_currentMoney);
        UIManager.ChangeCoins(2, _currentMoney);
        player.ChangeSkin(_currentMoney);

    }

    public void GetDrink()
    {
        if (_currentMoney > 20)
            _currentMoney -= 20;
        else
            _currentMoney = 0;

        UIManager.SetUIMoney(_currentMoney);

        UIManager.ChangeCoins(-20, _currentMoney);
        player.ChangeSkin(_currentMoney);
    }

    public int GetCurrentMoney()
    {
        return _currentMoney;
    }
}
