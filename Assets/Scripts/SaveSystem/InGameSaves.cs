using UnityEngine;


public class InGameSaves
{
    // Is busy?
    private static bool isBusy = false;

    // Plantation zone reference
    private static GameObject plantationZone = null;


    /// <summary>
    /// Change isBusy, so the game can detect if player is doing something or not.
    /// This will help to not open store when crafting or paused, for example.
    /// </summary>
    public static void ChangeIsBusy()
    {
        isBusy = !isBusy;
    }

    /// <summary>
    /// Get isBusy value.
    /// </summary>
    /// <returns>isBusy value.</returns>
    public static bool GetIsBusy()
    {
        return isBusy;
    }

    /// <summary>
    /// Set current plantation zone.
    /// </summary>
    /// <param name="zone">Plantation zone reference.</param>
    public static void SetPlantationZone(GameObject zone)
    {
        plantationZone = zone;
    }

    /// <summary>
    /// Get current plantation zone.
    /// </summary>
    /// <returns>Current plantation zone.</returns>
    public static GameObject GetPlantationZone()
    {
        return plantationZone;
    }
}
