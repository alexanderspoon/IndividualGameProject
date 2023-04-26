using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnPlayerPieces : GameManager
{
    public PieceSO samuraiPrefab;
    public PieceSO ninjaPrefab;
    public PieceSO spearPrefab;

    public GameObject pieceOrigin;

    public Vector3 newPieceOffset;

    List<PieceSO> piecesToSpawn; 

    // Start is called before the first frame update
    void Start()
    {
        DetermineLoadout();
        CreatePieceContainer();
        SpawnPieces(piecesToSpawn);
    }


    void DetermineLoadout() {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        switch (sceneName) {
            case "Kurikaesu":
                piecesToSpawn = new List<PieceSO> {ninjaPrefab, spearPrefab, samuraiPrefab};
                break;
            case "LevelTwo":
                piecesToSpawn = new List<PieceSO> {ninjaPrefab, spearPrefab, spearPrefab, samuraiPrefab};
                break;
            default:
                break;
        }
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
