using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _startPanel, _gamePanel, _winPanel, _losePanel;
   public void SetGameStateUI(GameState gameState)
   {
    if (gameState == GameState.start)
    {
        _startPanel.SetActive(true);
        _gamePanel.SetActive(false);
        _winPanel.SetActive(false);
        _losePanel.SetActive(false);
    }
    else if (gameState == GameState.play)
    {
        _startPanel.SetActive(false);
        _gamePanel.SetActive(true);
        _winPanel.SetActive(false);
        _losePanel.SetActive(false);
    }
    else if (gameState == GameState.win)
    {
        _startPanel.SetActive(false);
        _gamePanel.SetActive(false);
        _winPanel.SetActive(true);
        _losePanel.SetActive(false);
    }
    else if (gameState == GameState.lose)
    {
        _startPanel.SetActive(false);
        _gamePanel.SetActive(false);
        _winPanel.SetActive(false);
        _losePanel.SetActive(true);
    }
   }
}
