using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maps : GameManager
{
    // 0 = Free Space
    // 1 = Mountain

    public int[] mapOne = new int[55] 
    {
        0, 0, 0, 0, 0, 
         0, 0, 1, 0, 0,
        1, 0, 0, 1, 0,
         0, 0, 0, 1, 0,
        0, 1, 0, 0, 0, 
         1, 0, 0, 0, 0,
        0, 0, 0, 0, 0,
         0, 0, 1, 1, 0,
        0, 1, 0, 0, 0,
         0, 0, 0, 0, 1,
        0, 0, 1, 0, 0,
    };

    public int[] mapTwo = new int[55] 
    {
        0, 0, 0, 0, 1, 
         0, 1, 0, 0, 0,
        1, 0, 0, 1, 1,
         1, 0, 1, 0, 0,
        0, 0, 0, 0, 0, 
         0, 1, 0, 1, 1,
        0, 1, 0, 0, 0,
         1, 0, 0, 1, 0,
        0, 0, 1, 1, 0,
         0, 0, 0, 0, 1,
        0, 0, 1, 0, 0,
    };

    // 1 = Non-Spawnable space from Generate Map
    // 2 = Samurai
    // 3 = Spearman
    // 4 = Ninja
    public int[] enemyMapOne = new int[55] 
    {
        0, 0, 0, 0, 0, 
         0, 0, 1, 0, 0,
        1, 0, 0, 1, 0,
         0, 0, 0, 1, 0,
        0, 1, 0, 0, 0, 
         1, 0, 0, 0, 0,
        0, 0, 0, 0, 0,
         0, 0, 1, 1, 0,
        4, 1, 0, 0, 0,
         0, 0, 0, 3, 1,
        0, 2, 1, 0, 0,
    };

    public int[] enemyMapTwo = new int[55] 
    {
        0, 0, 0, 0, 1, 
         0, 1, 0, 0, 0,
        1, 0, 0, 1, 1,
         1, 0, 1, 0, 0,
        0, 0, 0, 0, 0, 
         0, 1, 0, 1, 1,
        0, 1, 0, 0, 0,
         1, 0, 0, 1, 0,
        0, 4, 1, 1, 2,
         4, 0, 0, 0, 1,
        0, 3, 1, 2, 0,
    };
}
