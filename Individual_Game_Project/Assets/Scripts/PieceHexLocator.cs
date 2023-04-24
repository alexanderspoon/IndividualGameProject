using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceHexLocator : GameManager
{
    private HexStruct currentPos;
    private GameObject interactionManager;

    void Start() {
        interactionManager = GameObject.Find("Interaction Manager");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hex")) {

            for (int i = 0; i < hexGrid.GetLength(1); i++) {
                for (int j = 0; j < hexGrid.GetLength(0); j++) {
                    if(hexGrid[j,i].hexGameObject.name == other.gameObject.name) {
                        currentPos = hexGrid[j,i];
                    }
                }
            }

            this.GetComponent<PieceReference>().pieceStruct.hexLocation = currentPos;
        }

        if (other.CompareTag("RHController")) {
            interactionManager.GetComponent<InteractionManager>().MovePiece(this.gameObject);
        }

    }
}
