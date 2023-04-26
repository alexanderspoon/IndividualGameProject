using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.XR.Interaction.Toolkit.UI;

public class WinLossBehavior : MonoBehaviour
{
    public VisualEffect smoke;
    public VisualEffect fireworks;

    public Canvas WinLossCanvas;

    void Start() {
        smoke.enabled = false;
        fireworks.enabled = false;
    }


    public void HandleWinLoss(bool didWin) {
        if(didWin) {
            fireworks.enabled = true;
            WinLossCanvas.transform.GetChild(0).gameObject.SetActive(true);
        } else {
            smoke.enabled = true;
            WinLossCanvas.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

}
