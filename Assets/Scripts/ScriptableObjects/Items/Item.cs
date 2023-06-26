using UnityEngine;


[CreateAssetMenu(fileName = "NewItem", menuName = "ScriptableObjects/Item")]
public class Item : ScriptableObject
{
    // Information
    [Header("Information")]
    public new string name;
    [TextArea] public string description;

    // Price
    [Header("Price")]
    public float buyValue;
    public float baseSellValue;

    // Level
    [Header("Level")]
    public int levelRequired;
    public int xp;

    // Icon
    [Header("Icon")]
    public Sprite icon;
}
