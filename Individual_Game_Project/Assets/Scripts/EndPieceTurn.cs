using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class EndPieceTurn : GameManager
{
    private XRSocketInteractor socket;
    private GameObject interactionManager;
    
    public InteractionLayerMask noInteractionMask;

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
                piece.GetComponent<XRGrabInteractable>().interactionLayers = noInteractionMask;
                if(enemyTurn == false) {
                    interactionManager.GetComponent<TurnManager>().CheckForEndTurn();
                }
                AudioSource audioSource = piece.GetComponent<AudioSource>();
                audioSource.pitch = (Random.Range(0.95f, 1.05f));
                audioSource.Play();
            }
        }

    }
}




