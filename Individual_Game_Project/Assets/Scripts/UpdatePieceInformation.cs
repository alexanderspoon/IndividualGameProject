using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdatePieceInformation : MonoBehaviour
{
    public TextMeshProUGUI health;
    public TextMeshProUGUI dice;

    void Start() {
        UpdatePieceStats();
    }

    public void UpdatePieceStats() {
        health.text = this.gameObject.GetComponent<PieceReference>().pieceStruct.currentHealth.ToString();
        dice.text = this.gameObject.GetComponent<PieceReference>().pieceStruct.diceAmount.ToString();
    }
    
}
