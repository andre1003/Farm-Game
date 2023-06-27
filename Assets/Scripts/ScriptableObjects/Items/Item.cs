using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;


[CreateAssetMenu(fileName = "NewItem", menuName = "Items/Item")]
public class Item : ScriptableObject
{
    // Information
    [Header("Information")]
    public new string name; // Unique. Does not change when language is changed

    // Localization
    [Header("Localization")]
    public LocalizedStringTable nameLocalization;
    public LocalizedStringTable descriptionLocalization;
    // It might be interesting to have two big tables, one for names and the other for
    // description, and search the localized string based on item name

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
        StringTable table = nameLocalization.GetTable();
        return table[name].LocalizedValue;
    }

    /// <summary>
    /// Get localized description.
    /// </summary>
    /// <returns>Localized description.</returns>
    public string GetDescription()
    {
        StringTable table = descriptionLocalization.GetTable();
        return table[name].LocalizedValue;
    }
}
