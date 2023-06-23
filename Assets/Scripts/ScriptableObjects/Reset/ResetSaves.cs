using UnityEngine;


public class ResetSaves : MonoBehaviour
{
    //// Default
    [Header("Default")]
    // Inventory
    public InventoryObject defaultInventory;

    // Player data
    public PlayerData defaultPlayerData;

    // Game settings
    public GameSettings defaultGameSettings;
    
    // Time system
    public TimeSystem defaultTimeSystem;


    //// Current saves
    [Header("Current Saves")]
    // Inventory
    public InventoryObject inventory;

    // Plantation zones
    public PlantationZones plantationZones;

    // Player data
    public PlayerData playerData;

    // Game settings
    public GameSettings gameSettings;

    // Time system
    public TimeSystem timeSystem;

    // Tutorials
    public InitialTutorial initialTutorial;
    public StoreTutorial storeTutorial;


    /// <summary>
    /// Reset player inventory.
    /// </summary>
    public void ResetInventory()
    {
        inventory.plants = defaultInventory.plants;
    }

    /// <summary>
    /// Reset player data.
    /// </summary>
    public void ResetPlayerData()
    {
        playerData.level = defaultPlayerData.level;
        playerData.xp = defaultPlayerData.xp;
        playerData.nextLvlXp = defaultPlayerData.nextLvlXp;
        playerData.money = defaultPlayerData.money;
    }

    /// <summary>
    /// Reset game settings.
    /// </summary>
    public void ResetGameSettings()
    {
        gameSettings.resolutionWidth = defaultGameSettings.resolutionWidth;
        gameSettings.resolutionHeight = defaultGameSettings.resolutionHeight;
        gameSettings.refreshRate = defaultGameSettings.refreshRate;
        gameSettings.fullscreen = defaultGameSettings.fullscreen;
        gameSettings.generalVolume = defaultGameSettings.generalVolume;
        gameSettings.graphicsPreset = defaultGameSettings.graphicsPreset;
        gameSettings.useAdditionalResource = defaultGameSettings.useAdditionalResource;
        gameSettings.languageIndex = defaultGameSettings.languageIndex;
    }

    /// <summary>
    /// Reset time system.
    /// </summary>
    public void ResetTimeSystem()
    {
        timeSystem.hour = defaultTimeSystem.hour;
        timeSystem.day = defaultTimeSystem.day;
        timeSystem.daysToChangeSeason = defaultTimeSystem.daysToChangeSeason;
        timeSystem.baseClockSeconds = defaultTimeSystem.baseClockSeconds;
        timeSystem.season = defaultTimeSystem.season;
    }

    /// <summary>
    /// Reset plantation zones.
    /// </summary>
    public void ResetPlantationZones()
    {
        plantationZones.positions.Clear();
        plantationZones.plants.Clear();
        plantationZones.plantingDays.Clear();
        plantationZones.plantingHours.Clear();
    }

    /// <summary>
    /// Reset all tutorials.
    /// </summary>
    public void ResetTutorials()
    {
        // Initial tutorial
        initialTutorial.isActive = true;
        initialTutorial.completed = false;
        initialTutorial.SetIndex(0);

        // Store tutorial
        storeTutorial.isActive = false;
        storeTutorial.completed = false;
        storeTutorial.SetIndex(0);
    }

    /// <summary>
    /// Reset all save files.
    /// </summary>
    public void ResetAll()
    {
        ResetInventory();
        ResetPlayerData();
        ResetGameSettings();
        ResetTimeSystem();
        ResetPlantationZones();
        ResetTutorials();
    }

    /// <summary>
    /// Reset all player data saves.
    /// </summary>
    public void ResetAllPlayerData()
    {
        ResetInventory();
        ResetPlayerData();
    }
}
