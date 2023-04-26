using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TurnManager : GameManager
{   
    private int turnTracker; 
    public GameObject map;

    private List<HexStruct> hexesToRemove = new List<HexStruct>();
    private List<HexStruct> hexesInRange = new List<HexStruct>();
    private List<HexStruct> surrondingHexes = new List<HexStruct>();
    private List<HexStruct> playerCheck = new List<HexStruct>();

    private List<PieceStruct> attackableEnemies = new List<PieceStruct>();
    private List<PieceStruct> playerPiecesInRange = new List<PieceStruct>();

    private Queue<PieceStruct> pieceQueue = new Queue<PieceStruct>();

    public InteractionLayerMask pieceMask;
    private XRSocketInteractor interactor;

    void AddEnemiesToQueue() {
        enemyTurn = true;
        foreach (PieceStruct enemyStr in enemyPieces) {
            pieceQueue.Enqueue(enemyStr);
        }
    }

    public void CheckForEndTurn() {
        turnTracker = 0;
        foreach (PieceStruct piece in playerPieces) {
            if (piece.turnOver == true) {
                turnTracker++;
            }
        }
        if(turnTracker == playerPieces.Count) {
            AddEnemiesToQueue();
            MoveForwardInQueue();
        }
    }

    void ReEnablePieces() {
        foreach (PieceStruct piece in playerPieces) {
            piece.turnOver = false;
            piece.pieceGameObject.GetComponent<XRGrabInteractable>().interactionLayers = pieceMask;
        }

    }

    void MoveForwardInQueue() {
        if(pieceQueue.Count > 0) {
            StartCoroutine(DoEnemyTurn(pieceQueue.Dequeue()));
        } 
    }

    void StopCoroutine() {
        StopCoroutine(DoEnemyTurn(null));
    }

    IEnumerator DoEnemyTurn(PieceStruct enemy) {
        yield return new WaitForSeconds(1f);

        hexesToRemove.Clear();
        playerPiecesInRange.Clear();
        hexesInRange.Clear();
        surrondingHexes.Clear();
        playerCheck.Clear();
        attackableEnemies.Clear();

        if(enemy != null) {
            HexStruct enemyLocation = enemy.hexLocation;

            hexesInRange = map.GetComponent<FindNeighbors>().SelectCircularNeighbors(enemyLocation, enemy.range, false, false);

            // Check if a piece is already on one of the selected tiles and remove it from the selection
            for (int i = 0; i < hexesInRange.Count; i++) {
                foreach (PieceStruct enemyStr in enemyPieces) {
                    if(enemyStr.hexLocation.hexGameObject.name == hexesInRange[i].hexGameObject.name) {
                        hexesToRemove.Add(hexesInRange[i]);
                    }
                }
                foreach (PieceStruct pieceStr in playerPieces) {
                    if(pieceStr.hexLocation.hexGameObject.name == hexesInRange[i].hexGameObject.name) {
                        hexesToRemove.Add(hexesInRange[i]);
                        playerPiecesInRange.Add(pieceStr);
                    }
                }
            }

            foreach (HexStruct hex in hexesToRemove) {
                hexesInRange.Remove(hex);
            }

            hexesToRemove.Clear();

            playerCheck = map.GetComponent<FindNeighbors>().SelectCircularNeighbors(enemyLocation, 1, false, false);

            foreach (PieceStruct pieceStr in playerPieces) {
                if(playerCheck.Contains(pieceStr.hexLocation)) {
                    attackableEnemies.Add(pieceStr);
                }
            }  

            if(attackableEnemies.Count > 0) {
                //Attack
                int randomPiece = Random.Range(0, attackableEnemies.Count);
                if(enemy.pieceGameObject != null) {
                    RotatePiece(enemyLocation, enemy.pieceGameObject, attackableEnemies[randomPiece].hexLocation.hexGameObject);
                    this.gameObject.GetComponent<DealDamage>().InRangeToDamage(enemy.pieceGameObject, attackableEnemies[randomPiece].pieceGameObject);
                    yield return new WaitForSeconds(4f);
                }
            } else {
                if(playerPiecesInRange.Count > 0) {
                    //Move Towards Player If In Range
                    int randomPiece = Random.Range(0, playerPiecesInRange.Count);
                    HexStruct playerLocation = playerPiecesInRange[randomPiece].hexLocation;
                    surrondingHexes = map.GetComponent<FindNeighbors>().SelectCircularNeighbors(playerLocation, 1, false, false);
                    foreach (var hex in surrondingHexes) {
                        if(!hexesInRange.Contains(hex)) {
                            hexesToRemove.Add(hex);
                        }
                        foreach (PieceStruct enemyStr in enemyPieces) {
                            if(hex.hexGameObject.name == enemyStr.hexLocation.hexGameObject.name) {
                                hexesToRemove.Add(hex);
                            }
                        }   
                        foreach (PieceStruct pieceStr in playerPieces) {
                            if(hex.hexGameObject.name == pieceStr.hexLocation.hexGameObject.name) {
                                hexesToRemove.Add(hex);
                            }
                        }   
                    }
                    foreach (HexStruct hex in hexesToRemove) {
                        surrondingHexes.Remove(hex);
                    }
                    if(surrondingHexes.Count > 0) {
                        int randomHexIndex = Random.Range(0, surrondingHexes.Count); 
                        MovePiece(enemyLocation, enemy.pieceGameObject, surrondingHexes[randomHexIndex].hexGameObject);
                    }
                } else {  
                    //Move Randomly in Range
                    if(hexesInRange.Count > 0) {
                        int randomHexIndex = Random.Range(0, hexesInRange.Count); 
                        MovePiece(enemyLocation, enemy.pieceGameObject, hexesInRange[randomHexIndex].hexGameObject);
                    }
                }
            }
        }

        if(pieceQueue.Count > 0) {
            MoveForwardInQueue();
        } else {
            ReEnablePieces();
            enemyTurn = false;
            StopCoroutine();
        }
        
    }


    void MovePiece(HexStruct currentHex, GameObject piece, GameObject targetHex) {
        if(piece != null && targetHex != null) {
            AudioSource audioSource = piece.GetComponent<AudioSource>();
            audioSource.pitch = (Random.Range(0.95f, 1.05f));
            audioSource.Play();

            RotatePiece(currentHex, piece, targetHex);

            interactor = currentHex.hexGameObject.GetComponent<XRSocketInteractor>();
            if(interactor.enabled == true) {
                interactor.enabled = false;
                Invoke("EnableInteractor", .25f);
            }

            piece.transform.position = targetHex.transform.position + new Vector3(0,.15f,0);
        }
    }

    void RotatePiece(HexStruct currentHex, GameObject piece, GameObject targetHex) {
        
        if (piece != null)
        {   
            interactor = currentHex.hexGameObject.GetComponent<XRSocketInteractor>();
            if(interactor.enabled == true) {
                interactor.enabled = false;
                Invoke("EnableInteractor", .25f);
            }

            Transform target = targetHex.transform;
            Vector3 direction = target.position - piece.transform.position;

            direction.y = 0f;

            if (direction.magnitude > 0f) {
                direction.Normalize();
            }

            Quaternion rotation = Quaternion.LookRotation(direction);
            piece.transform.rotation = Quaternion.Euler(0f, rotation.eulerAngles.y, 0f);
            piece.transform.position = piece.transform.position + new Vector3(0,.15f,0);
        }
    }

    void EnableInteractor() {
        interactor.enabled = true;
    }
}
