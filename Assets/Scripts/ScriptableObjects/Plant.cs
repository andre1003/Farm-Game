using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Plant", menuName = "ScriptableObjects/Plant", order = 1)]
public class Plant : ScriptableObject
{
    // Name
    public new string name;

    // Grow
    public double timeToGrow;
    public double timeToRot;

    // Price
    public float buyValue;
    public float baseSellValue;

    // Life cycle
    public int life;
    public int maxHarvest;
    public int minHarvest;

    // Level
    public int levelRequired;
    public int xp;

    // Season order
    public List<int> seasons;

    // Icon
    public Sprite icon;

    // Prefab
    public Transform plantPrefab;
}
