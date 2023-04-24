using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayerPieces : GameManager
{
    public PieceSO samuraiPrefab;
    public PieceSO ninjaPrefab;
    public PieceSO spearPrefab;

    public GameObject pieceOrigin;

    public Vector3 newPieceOffset;

    // Start is called before the first frame update
    void Start()
    {
        List<PieceSO> piecesToSpawn = new List<PieceSO> {ninjaPrefab, spearPrefab, samuraiPrefab};

        CreatePieceContainer();
        SpawnPieces(piecesToSpawn);
    }

    void CreatePieceContainer() {
        playerPieces = new List<PieceStruct>();
    }

    void SpawnPieces(List<PieceSO> pieces) {

        Vector3 xOffset = new Vector3(0,0,0); 

        for (int i = 0; i < pieces.Count; i++)
        {
            GameObject currentPiece = (GameObject)Instantiate(pieces[i].prefab, pieceOrigin.transform.position + xOffset, Quaternion.identity);
            xOffset += newPieceOffset;
            currentPiece.transform.SetParent(this.transform);

            PieceStruct pieceStr = new PieceStruct();
            pieceStr.pieceGameObject = currentPiece;
            pieceStr.hexLocation = hexGrid[0,0];
            pieceStr.turnOver = false;

            pieceStr.range = pieces[i].range;
            pieceStr.currentHealth = pieces[i].health;
            pieceStr.diceAmount = pieces[i].damage;

            pieceStr.animationName = pieces[i].animationName;

            currentPiece.GetComponent<PieceReference>().pieceSO = pieces[i];
            currentPiece.GetComponent<PieceReference>().pieceStruct = pieceStr;

            playerPieces.Add(pieceStr);
        }
    }
}
