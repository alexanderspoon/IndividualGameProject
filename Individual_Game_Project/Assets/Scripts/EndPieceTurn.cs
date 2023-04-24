using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class EndPieceTurn : MonoBehaviour
{
    private XRSocketInteractor socket;
    private GameObject interactionManager;

    void Start() {
        socket = GetComponent<XRSocketInteractor>();
        interactionManager = GameObject.Find("Interaction Manager");
    }
 
    public void SocketCheck() {

        if(socket.GetOldestInteractableSelected() != null) {
            IXRSelectInteractable objName = socket.GetOldestInteractableSelected();
            GameObject piece = objName.transform.gameObject;

            if(piece.CompareTag("PlayerPiece")) {
                piece.GetComponent<PieceReference>().pieceStruct.turnOver = true;
                interactionManager.GetComponent<TurnManager>().CheckForEndTurn();
            }
        }

    }
}




