using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayerPieces : GameManager
{
    public GameObject archerPrefab;
    public GameObject samuraiPrefab;
    public GameObject cavalryPrefab;
    public GameObject spearPrefab;

    public GameObject pieceOrigin;

    // Start is called before the first frame update
    void Start()
    {
        List<GameObject> piecesToSpawn = new List<GameObject> {archerPrefab, spearPrefab, samuraiPrefab, spearPrefab};

        CreatePieceContainer();
        SpawnPieces(piecesToSpawn);
    }

    void CreatePieceContainer() {
        playerPieces = new List<PieceStruct>();
    }

    void SpawnPieces(List<GameObject> pieces) {

        Vector3 xOffset = new Vector3(0,0,0); 

        for (int i = 0; i < pieces.Count; i++)
        {
            GameObject currentPiece = (GameObject)Instantiate(pieces[i], pieceOrigin.transform.position + xOffset, Quaternion.identity);
            xOffset += new Vector3(.2f,0,0);
            currentPiece.transform.SetParent(this.transform);

            PieceStruct pieceObj = new PieceStruct();
            pieceObj.hexLocation = hexGrid[i,0]; //This will break if there are more pieces than hexes in the row
            pieceObj.pieceGameObject = currentPiece;
            pieceObj.playerControllable = true;

            playerPieces.Add(pieceObj);
        }
    }
}
