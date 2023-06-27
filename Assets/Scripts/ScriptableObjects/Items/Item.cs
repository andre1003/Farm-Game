using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;


[CreateAssetMenu(fileName = "NewItem", menuName = "ScriptableObjects/Item")]
public class Item : ScriptableObject
{
    // Information
    [Header("Information")]
    public new string name; // Unique. Does not change when language is changed

    // Localization
    [Header("Localization")]
    public LocalizedStringTable localizationTable;

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


    /// <summary>
    /// Get localized commertial name.
    /// </summary>
    /// <returns>Localized commertial name.</returns>
    public string GetCommertialName()
    {
        StringTable table = localizationTable.GetTable();
        return table["commertialName"].LocalizedValue;
    }

    /// <summary>
    /// Get localized description.
    /// </summary>
    /// <returns>Localized description.</returns>
    public string GetDescription()
    {
        StringTable table = localizationTable.GetTable();
        return table["description"].LocalizedValue;
    }
}
