using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Enums;

public class MainMenu : MonoBehaviour
{
    public Animator playerModelAnimator;
    public float animationInterval = 4f;
    private float _timeElapsed = 0f;
    private int _previousAnimation = 3;     // Last animation played

    void Start()
    {
        
    }

    // Rotates between animations for the player model
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GameLevelsManager.instance.LoadCurrentLevel();
        }
        if (isOnIdleAnimation())
        {
            _timeElapsed += Time.deltaTime;
            if (_timeElapsed > animationInterval)
            {
                int nextAnimation = getNextAnimation(_previousAnimation);
                playerModelAnimator.SetInteger("currentAnimation", nextAnimation);
                _previousAnimation = nextAnimation;
                _timeElapsed = 0f;
            }
        } else if (currentAnimationOngoing())
        {
            _timeElapsed = 0f;
            playerModelAnimator.SetInteger("currentAnimation", 0);
        }
    }

    private int getNextAnimation(int _previousAnimation) {
        if (_previousAnimation < 3)
            return _previousAnimation + 1;
        return 1;
    }

    private bool isOnIdleAnimation() {
        return playerModelAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle");
    }

    private bool currentAnimationOngoing() {
        return playerModelAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f;
    }
}
