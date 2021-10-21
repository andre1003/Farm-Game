using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Plant", menuName = "ScriptableObjects/Plant", order = 1)]
public class Plant : ScriptableObject {
    public new string name;
    
    public double timeToGrow;
    public double timeToRot;

    public float buyValue;
    public float baseSellValue;

    public int life;
    public int maxHarvest;
    public int minHarvest;
    public int levelRequired;

    public Texture2D icon;

}
