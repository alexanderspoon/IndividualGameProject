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

    void AddEnemiesToQueue() {
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
            foreach (PieceStruct piece in playerPieces) {
                piece.turnOver = false;
            }

            AddEnemiesToQueue();
            MoveForwardInQueue();
        }
    }

    void MoveForwardInQueue() {
        StartCoroutine(DoEnemyTurn(pieceQueue.Dequeue()));
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
                    this.gameObject.GetComponent<DealDamage>().InRangeToDamage(enemy.pieceGameObject, attackableEnemies[randomPiece].pieceGameObject);
                }
                yield return new WaitForSeconds(5f);
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
                    int randomHexIndex = Random.Range(0, surrondingHexes.Count); 
                    MovePiece(enemyLocation, enemy.pieceGameObject, surrondingHexes[randomHexIndex].hexGameObject);
                } else {  
                    //Move Randomly in Range
                    if(hexesInRange.Count > 0) {
                        int randomHexIndex = Random.Range(0, hexesInRange.Count); 
                        MovePiece(enemyLocation, enemy.pieceGameObject, hexesInRange[randomHexIndex].hexGameObject);
                    }
                }
                yield return new WaitForSeconds(1.5f);
            }
        }

        if(pieceQueue.Count > 0) {
            MoveForwardInQueue();
        }
    }

    private XRSocketInteractor interactor;

    void MovePiece(HexStruct currentHex, GameObject piece, GameObject targetHex) {
        if(piece != null && targetHex != null) {
            interactor = currentHex.hexGameObject.GetComponent<XRSocketInteractor>();
            interactor.enabled = false;
            Invoke("EnableInteractor", .25f);
            piece.transform.position = targetHex.transform.position + new Vector3(0,.1f,0);
        }
    }

    void EnableInteractor() {
        interactor.enabled = true;
    }
}
