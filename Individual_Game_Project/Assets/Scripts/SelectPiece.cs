using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class SelectPiece : GameManager
{
    public InputActionReference primaryPressRef = null;

    private float maxDistance = 10f; 
    private GameObject hitObject; 

    public GameObject rhController; 
    public GameObject selectedPiece; 

    public GameObject map; 

    public InteractionLayerMask noInteractionMask;

    private void Awake() {
        primaryPressRef.action.started += ShootRaycast;
    }

    private void OnDestroy() {
        primaryPressRef.action.started -= ShootRaycast;
    }

    private void ShootRaycast(InputAction.CallbackContext context) {
        Ray ray = new Ray(rhController.transform.position, rhController.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance)) {
            hitObject = hit.collider.gameObject;

            DeselectAllPieces();

            if(hitObject.CompareTag("PlayerPiece") && hitObject.GetComponent<PieceReference>().pieceStruct.turnOver == false) {
                GameObject selector = hitObject.transform.Find("Selector").gameObject;
                selector.GetComponent<MeshRenderer>().enabled = true;
                selector.SetActive(true);
                selectedPiece = hitObject;
                map.GetComponent<FindNeighbors>().SelectCircularNeighbors(selectedPiece.GetComponent<PieceReference>().pieceStruct.hexLocation, 1, false, true, "red");
            }

            if(selectedPiece != null && hitObject.CompareTag("EnemyPiece")) {
                selectedPiece.GetComponent<XRGrabInteractable>().interactionLayers = noInteractionMask;
                StartCoroutine(ManageEnemyPlayerInteraction(hitObject));
            }

        }
    }

    IEnumerator ManageEnemyPlayerInteraction(GameObject enemy) {
        selectedPiece.GetComponent<PieceReference>().pieceStruct.turnOver = true;
        this.gameObject.GetComponent<DealDamage>().InRangeToDamage(selectedPiece, enemy);
        yield return new WaitForSeconds(5.0f);                
        if(enemyTurn == false) {
            this.gameObject.GetComponent<TurnManager>().CheckForEndTurn();
        }
    }


    private void DeselectAllPieces() {
        for (int i = 0; i < playerPieces.Count; i++) {
            GameObject selector = playerPieces[i].pieceGameObject.transform.Find("Selector").gameObject;
            selector.GetComponent<MeshRenderer>().enabled = false;
            selector.SetActive(false);
        }
    }



}
