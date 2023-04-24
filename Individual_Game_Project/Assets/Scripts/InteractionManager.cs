using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class InteractionManager : GameManager
{

    public GameObject map;
    private PieceStruct selectedPiece;
    private XRGrabInteractable grabInt;

    public void MovePiece(GameObject piece) {
        PieceStruct pieceStruct = piece.GetComponent<PieceReference>().pieceStruct;

        map.GetComponent<FindNeighbors>().SelectCircularNeighbors(pieceStruct.hexLocation, pieceStruct.range, true, true);
    }  

}
