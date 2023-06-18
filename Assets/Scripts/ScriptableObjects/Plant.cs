using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Plant", menuName = "ScriptableObjects/Plant", order = 1)]
public class Plant : ScriptableObject
{
    // Name
    [Header("Name")]
    public new string name;

    // Grow
    [Header("Grow")]
    public double timeToGrow;
    public double timeToRot;

    // Price
    [Header("Price")]
    public float buyValue;
    public float baseSellValue;

    // Life cycle
    [Header("Life Cycle")]
    public int life;
    public int maxHarvest;
    public int minHarvest;

    // Level
    [Header("Level")]
    public int levelRequired;
    public int xp;

    // Season order
    [Header("Season")]
    public List<int> seasons;

    // Icon
    [Header("Icon")]
    public Sprite icon;

    // Prefab
    [Header("Prefab")]
    public Transform plantPrefab;
}
