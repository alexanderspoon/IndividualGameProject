using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixerSnapshot[] mixArray;

    private int currentIndex;
    private float transitionTime;

    void Start() {
        currentIndex = 1;
        transitionTime = 3f;
    }

    public void ChangeMusic() {
        if(currentIndex < mixArray.Length) {
            mixArray[currentIndex].TransitionTo(transitionTime);
            currentIndex++;
        }
    }
}
