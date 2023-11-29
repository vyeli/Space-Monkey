using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Enums;
using System;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Animator _playerModelAnimator;
    [SerializeField] private float _animationInterval;
    private float _timeElapsed;
    private int _previousAnimation;
    private int _animationsAmount;

    [SerializeField] private LoadingScreen loadingScreen;

    private enum PlayerAnimations
    {
        Idle = 0,
        Waving = 1,
        Punching = 2,
        SayingYes = 3
    }

    void Start()
    {
        _timeElapsed = 0f;
        _animationsAmount = System.Enum.GetValues(typeof(PlayerAnimations)).Length;
        _previousAnimation = _animationsAmount - 1;     // So first animation goes to one
    }

    // Rotates between animations for the player model
    void Update()
    {
        if (isOnIdleAnimation())
        {
            _timeElapsed += Time.deltaTime;
            if (_timeElapsed > _animationInterval)
            {
                int nextAnimation = getNextAnimation(_previousAnimation);
                _playerModelAnimator.SetInteger("currentAnimation", nextAnimation);
                _previousAnimation = nextAnimation;
                _timeElapsed = 0f;
            }
        }
        else if (currentAnimationOngoing())
        {
            _timeElapsed = 0f;
            _playerModelAnimator.SetInteger("currentAnimation", (int)PlayerAnimations.Idle);
        }
    }

    private int getNextAnimation(int _previousAnimation) {
        if (_previousAnimation < _animationsAmount - 1)    // Hasn't cycled through all animations yet
            return _previousAnimation + 1;
        return 1;
    }

    private bool isOnIdleAnimation() {
        return _playerModelAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle");
    }

    private bool currentAnimationOngoing() {
        return _playerModelAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f;
    }

    public void LoadGame() => loadingScreen.LoadScene((int)Levels.Level1);

}
