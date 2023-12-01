using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSFX : MonoBehaviour
{
    [SerializeField] private AudioClip buttonClick;
    [SerializeField] private AudioClip buttonHover;

    [SerializeField] private SoundtrackPlayer soundtrackPlayer;

    public void PlayButtonClick()
    {
        soundtrackPlayer.PlaySFX(buttonClick);
    }

    public void PlayButtonHover()
    {
        soundtrackPlayer.PlaySFX(buttonHover);
    }
}
