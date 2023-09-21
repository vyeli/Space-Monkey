using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public Animator playerModelAnimator;
    public float animationInterval = 4f;
    private float _timeElapsed = 0f;
    private bool _isPlayingAnimation = false;
    private int _previousAnimation = 3;     // Last animation played

    void Start()
    {
        
    }

    // Rotates between animations for the player model
    void Update()
    {
        _timeElapsed += Time.deltaTime;
        if (_timeElapsed > 1f && _isPlayingAnimation) {
            playerModelAnimator.SetInteger("currentAnimation", 0);
            _isPlayingAnimation = false;
            _timeElapsed = 0f;
        }
        if (_timeElapsed > animationInterval && !_isPlayingAnimation)
        {
            int nextAnimation = getNextAnimation(_previousAnimation);
            playerModelAnimator.SetInteger("currentAnimation", nextAnimation);
            _previousAnimation = nextAnimation;
            _isPlayingAnimation = true;
            _timeElapsed = 0f;
        }
    }

    private int getNextAnimation(int _previousAnimation) {
        if (_previousAnimation < 3)
            return _previousAnimation + 1;
        return 1;
    }
}
