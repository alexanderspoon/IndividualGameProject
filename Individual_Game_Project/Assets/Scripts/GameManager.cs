using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameManager : MonoBehaviour
{
    public static HexStruct[,] hexGrid;
    public static List<PieceStruct> playerPieces;
    public static List<PieceStruct> enemyPieces;
}
