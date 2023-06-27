using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Plant", menuName = "Items/Plant", order = 1)]
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

    // Most common plants for each station:
    // https://www.valeshop.com.br/site/?p=6320

    /* Seasons (Brazil):
     * 
     * Summer: January - March
     * Autumn: April - June
     * Winter: July - September
     * Spring: October - Dezember
     * 
     */
}
