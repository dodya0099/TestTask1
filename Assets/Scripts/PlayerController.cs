using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private Animator _animator;
    [SerializeField] private Sound[] _stepSound, _skinChangeSound, _loseSound;
    [SerializeField] private GameObject _poorSkin, _middleSkin, _casulaSkin, _blingSkin;
    private void Update()
    {
        Animation();
        if (_levelManager.GetGameState() == GameState.play)
            _levelManager.soundManager.PlayStepSound(_stepSound);
    }

    private void Animation()
    {
        GameState gameState = _levelManager.GetGameState();

        if (gameState == GameState.start)
            _animator.SetBool("idle", true);
        else if (gameState == GameState.play)
            _animator.SetBool("walk", true);
        else if (gameState == GameState.win)
            _animator.SetBool("win", true);
        else if (gameState == GameState.lose)
            _animator.SetBool("lose", true);

    }

    public void ChangeSkin(int currentMoney)
    {
        if (currentMoney == 0)
        {
            _levelManager.SetGameState(GameState.lose);
            _levelManager.soundManager.PlaySound(_loseSound);
            _animator.SetBool("lose", true);
            _poorSkin.SetActive(true);
            _casulaSkin.SetActive(false);
            _middleSkin.SetActive(false);
            _blingSkin.SetActive(false);
        }
        else if (currentMoney == 30 || currentMoney == 60)
        {
            _animator.SetTrigger("changeSkin");
            _levelManager.soundManager.PlaySound(_skinChangeSound);
        }
        else if (currentMoney < 30)
        {
            _poorSkin.SetActive(false);
            _casulaSkin.SetActive(true);
            _middleSkin.SetActive(false);
            _blingSkin.SetActive(false);
        }
        else if (currentMoney < 60)
        {
            _poorSkin.SetActive(false);
            _casulaSkin.SetActive(false);
            _middleSkin.SetActive(true);
            _blingSkin.SetActive(false);
        }
        else if (currentMoney > 60)
        {
            _poorSkin.SetActive(false);
            _casulaSkin.SetActive(false);
            _middleSkin.SetActive(false);
            _blingSkin.SetActive(true);
        }
    }


}
