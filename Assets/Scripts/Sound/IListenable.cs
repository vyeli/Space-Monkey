using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IListenable
{

    void InitAudioSource();
    void PlayOneShot();
    void Play();
    void Stop();
}