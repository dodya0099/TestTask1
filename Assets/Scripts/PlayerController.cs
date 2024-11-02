using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private LevelManager _levelManager;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }
    private void Update()
    {
        Animation();
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
}
