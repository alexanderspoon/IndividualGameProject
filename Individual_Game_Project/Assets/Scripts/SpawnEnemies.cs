using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;

public class SpawnEnemies : Maps
{
    public PieceSO enemySamurai;
    public PieceSO enemyNinja;
    public PieceSO enemySpearman;

    private GameObject prefab;
    private PieceSO prefabSO;

    Vector3 yOffset = new Vector3 (0,.1f,0);

    public int[] EnemyMap; 


    void Start() {
        CreateEnemyPieceContainer();
        SpawnEnemy();
    }

    void CreateEnemyPieceContainer() {
        enemyPieces = new List<PieceStruct>();
    }

    void SpawnEnemy() {
        FindMap();

        for (int i = 0; i < EnemyMap.Length; i++) {

            if(EnemyMap[i] > 1) {

                int enemyCase = EnemyMap[i]; 

                switch (enemyCase) {
                    case 2:
                        prefab = enemySamurai.prefab;
                        prefabSO = enemySamurai;
                        break;
                    case 3:
                        prefab = enemyNinja.prefab;
                        prefabSO = enemyNinja;
                        break;
                    case 4:
                        prefab = enemySpearman.prefab;
                        prefabSO = enemySpearman;
                        break;
                    default:
                        Debug.Log("No Enemy Index Assigned to " + EnemyMap[i]);
                        break;
                }   

                GameObject enemyGameObject = Instantiate(prefab, transform.position, Quaternion.identity);
                
                PieceStruct enemyStruct = new PieceStruct();
                enemyStruct.pieceGameObject = enemyGameObject;

                for (int n = 0; n < hexGrid.GetLength(1); n++) {
                    for (int j = 0; j < hexGrid.GetLength(0); j++) {
                        if(i == hexGrid[j,n].h_id) {
                            enemyStruct.hexLocation = hexGrid[j,n];
                        }
                    }
                }

                enemyStruct.pieceSO = prefabSO;
                enemyGameObject.GetComponent<PieceReference>().pieceSO = prefabSO;
                enemyGameObject.GetComponent<PieceReference>().pieceStruct = enemyStruct;

                enemyStruct.currentHealth = prefabSO.health;
                enemyStruct.diceAmount = prefabSO.damage;
                enemyStruct.range = prefabSO.range;
                enemyStruct.animationName = prefabSO.animationName;

                enemyPieces.Add(enemyStruct);

                GameObject hex = enemyStruct.hexLocation.hexGameObject;
                enemyGameObject.transform.position = hex.transform.position + yOffset; 
                enemyGameObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
        }
    } 


    void FindMap() {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        switch (sceneName) {
            case "Kurikaesu":
                EnemyMap = enemyMapOne;
                break;
            case "LevelTwo":
                EnemyMap = enemyMapTwo;
                break;
            default:
                break;
        }
    }
}
