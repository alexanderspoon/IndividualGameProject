using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesterScript : GameManager
{
    public GameObject testedObject;

    public int row = 9;
    public int column = 9;
    public int range = 5;
    public bool origin = true;
    public string selection = "R";
    
    List<HexStruct> myHexStructs;

    void Update()
    {
        if(row > 0 && row < hexGrid.GetLength(0) && column > 0 && column < hexGrid.GetLength(0)) {
            myHexStructs = testedObject.GetComponent<FindNeighbors>().SelectCircularNeighbors(hexGrid[row,column], range, origin);
            ChangeColors(myHexStructs);
        } 
    }

    void ChangeColors(List<HexStruct> myHexStructs) {
        for (int i = 0; i < hexGrid.GetLength(1); i++) {
            for (int j = 0; j < hexGrid.GetLength(0); j++) {

                Renderer renderer = hexGrid[j,i].hexGameObject.GetComponentInChildren<Renderer>();
                Material material = renderer.material;

                if(hexGrid[j,i].interactible) {
                    material.color = new Color (0,0,0,1);
                } else {
                    material.color = new Color (1,0,0,1);
                }


            }
        }

        for (int i = 0; i < myHexStructs.Count; i++)
        {
            Renderer renderer = myHexStructs[i].hexGameObject.GetComponentInChildren<Renderer>();
            Material material = renderer.material;
            material.color = new Color (0,0,1,1);
        }
    }


}
