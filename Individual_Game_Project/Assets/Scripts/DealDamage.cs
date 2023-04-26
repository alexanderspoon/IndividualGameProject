using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamage : GameManager
{   
    public GameObject map;
    public GameObject audioManager;

    public GameObject attackedPiece;
    private PieceStruct attackedStruct;
    private PieceStruct attackerStruct;
    private int attackRange;
    private HexStruct currentLocation;
    private HexStruct targetHex;
    private List<HexStruct> possibleHexes;

    public void InRangeToDamage(GameObject attacker, GameObject attacked) {
        if(attacker != null && attacked != null) {
            attackerStruct = attacker.GetComponent<PieceReference>().pieceStruct;
            attackRange = attackerStruct.range;
            attackedPiece = attacked;
            currentLocation = attacker.GetComponent<PieceReference>().pieceStruct.hexLocation;
            possibleHexes = map.GetComponent<FindNeighbors>().SelectCircularNeighbors(currentLocation, attackRange, false, false);

            targetHex = attackedPiece.GetComponent<PieceReference>().pieceStruct.hexLocation;

            if(possibleHexes.Contains(targetHex)) {
                RollForDamage(attackerStruct);
            }
        }
    }

    void RollForDamage(PieceStruct attackerStruct) {
        this.gameObject.GetComponent<DiceManager>().RollDice(attackerStruct.diceAmount);
    }

    public void DamagePiece(int damage) {
        Animator attackerAnimator = attackerStruct.pieceGameObject.GetComponentInChildren<Animator>();
        attackerAnimator.Play(attackerStruct.animationName);

        AudioSource audioSource = attackerStruct.pieceGameObject.transform.GetChild(0).GetComponent<AudioSource>();
        audioSource.pitch = (Random.Range(0.95f, 1.05f));
        audioSource.Play();

        attackedStruct = attackedPiece.GetComponent<PieceReference>().pieceStruct;
        attackedPiece.GetComponent<PieceReference>().pieceStruct.currentHealth -= damage;
        attackedPiece.GetComponent<UpdatePieceInformation>().UpdatePieceStats();
        
        if(attackedStruct.currentHealth <= 0) {
            audioManager.GetComponent<AudioManager>().ChangeMusic();

            if(attackedPiece.CompareTag("EnemyPiece")) {
                enemyPieces.Remove(attackedStruct);
            }
            if(attackedPiece.CompareTag("PlayerPiece")) {
                playerPieces.Remove(attackedStruct);
                this.gameObject.GetComponent<SelectPiece>().selectedPiece = null;
            }
            Destroy(attackedPiece);
            CheckWinLoss();
        }
    }

    void CheckWinLoss() {
        if(playerPieces.Count <= 0) {
            this.gameObject.GetComponent<WinLossBehavior>().HandleWinLoss(false);
        } 
        if(enemyPieces.Count <= 0) {
            this.gameObject.GetComponent<WinLossBehavior>().HandleWinLoss(true);
        }
    }


}
