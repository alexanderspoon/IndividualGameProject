using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    //There are 8 Audio mixes here.
    public AudioMixerSnapshot[] mixArray;

    private int currentIndex;

    public void ChangeMusic(int mixIndex, float transitionTime) {
        if(currentIndex != mixIndex) {
            mixArray[mixIndex].TransitionTo(transitionTime);
            currentIndex = mixIndex;
        }
    }
}
