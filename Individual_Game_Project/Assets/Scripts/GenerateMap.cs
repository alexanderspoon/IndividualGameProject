using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GenerateMap : Maps
{

    public int mapWidth;
    public int mapHeight;
    public float NonInteractableThreshold = .7f;

    //Distance between hexes
    private float xOffset = 0.327f;
    private float zOffset = 0.284f;
    private float oddLayerOffset = 0.171f;

    public GameObject basicPrefab; 
    public GameObject mountainPrefab; 
    public GameObject mapOrigin; 
    
    public int[] CustomMap;

    void Awake()
    {
        CreateGrid(mapHeight, mapWidth); 
        InstantiateMap(mapHeight, mapWidth, mapOrigin.transform.position);

        //Displayes all hex info in debug
        // DisplayHexInfo(); 
    }

    //Loops through data container and displays all information
    void DisplayHexInfo() { 
        for (int i = 0; i < hexGrid.GetLength(1); i++) {
            for (int j = 0; j < hexGrid.GetLength(0); j++) {
                LogHexInformation(hexGrid[j,i]);
            }
        }
    }

    //Creates data container
    void CreateGrid(int height, int width) {
        hexGrid = new HexStruct[width, height];
    }

    //Debug logs all hex info
    void LogHexInformation(HexStruct hex) {
        Debug.Log("id: " + hex.h_id);
        Debug.Log("name: " + hex.hexGameObject.name);
        Debug.Log("pos: " + hex.arrayPos);
        Debug.Log("odd: " + hex.odd);
        Debug.Log("interactiable: " + hex.interactible);
        Debug.Log("------------------------");
    }

    void FindMap() {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        switch (sceneName) {
            case "Kurikaesu":
                CustomMap = mapOne;
                break;
            case "LevelTwo":
                CustomMap = mapTwo;
                break;
            default:
                break;
        }
    }

    //Generates hexes and data
    void InstantiateMap(int height, int width, Vector3 origin) {
        FindMap();
        
        int mapReaderIndex = 0;
        
        Vector3 hexPos = origin;
        bool isOdd = false; 

        int hex_id = 0;

        GameObject hexPrefab = basicPrefab;


        for (int z = 0; z < height; z++) {

            if(isOdd) {
                hexPos += new Vector3 (oddLayerOffset, 0, 0);
            }

            for (int x = 0; x < width; x++) {

                HexStruct hexObj = new HexStruct();
                hexObj.h_id = hex_id;
                hex_id++;
                hexObj.arrayPos = new Vector2Int(x,z);
                hexObj.odd = isOdd;

                if(CustomMap[mapReaderIndex] == 1) {
                    hexObj.interactible = false;
                    hexPrefab = mountainPrefab;
                } else {
                    hexObj.interactible = true;
                    hexPrefab = basicPrefab;
                }
                mapReaderIndex++;

                GameObject currentHex = (GameObject)Instantiate(hexPrefab, hexPos, Quaternion.Euler(270f, 0f, 0f));
                hexObj.hexGameObject = currentHex;

                if(hexObj.interactible == false) {
                    currentHex.transform.position += new Vector3 (0,.02f,0);
                }

                hexPos += new Vector3 (xOffset,0,0);
                currentHex.transform.SetParent(this.transform);
                currentHex.name = "Hex_" + x + "_" + z;

                hexGrid[x,z] = hexObj;
            }

            hexPos = new Vector3 (origin.x, origin.y, hexPos.z);
            hexPos += new Vector3 (0, 0, zOffset);

            isOdd = !isOdd;
        }

    }
}
