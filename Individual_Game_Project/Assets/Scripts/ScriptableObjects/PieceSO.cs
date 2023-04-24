using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PieceSO", menuName = "ScriptableObject", order = 1)]
public class PieceSO : ScriptableObject
{
    public GameObject prefab;
    public int range;
    public int damage;
    public int health;
    public string animationName;
}
