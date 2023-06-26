using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Plant", menuName = "ScriptableObjects/Plant", order = 1)]
public class Plant : Item
{
    // Grow
    [Header("Grow")]
    public double timeToGrow;
    public double timeToRot;

    // Life cycle
    [Header("Life Cycle")]
    public int life;
    public int maxHarvest;
    public int minHarvest;

    // Season order
    [Header("Season")]
    public List<int> seasons;

    // Prefab
    [Header("Prefab")]
    public Transform plantPrefab;
}
