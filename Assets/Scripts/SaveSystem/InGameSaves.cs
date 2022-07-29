using UnityEngine;

public class InGameSaves {
    //private static bool canMove = true;
    private static bool isBusy = false;
    private static GameObject plantationZone = null;
    private static float clockSeconds = 2f;

    // Change isBusy, so the game can detect if player is doing something or not
    // This will help to don't open store when crafting or vice-versa
    public static void ChangeIsBusy() {
        isBusy = !isBusy;
    }

    // Get isBusy value
    public static bool GetIsBusy() {
        return isBusy;
    }
    
    // Set current plantation zone
    public static void SetPlantationZone(GameObject zone) {
        plantationZone = zone;
    }

    // Get current plantation zone
    public static GameObject GetPlantationZone() {
        return plantationZone;
    }

    public static float GetClockSeconds() {
        return clockSeconds;
    }
}
