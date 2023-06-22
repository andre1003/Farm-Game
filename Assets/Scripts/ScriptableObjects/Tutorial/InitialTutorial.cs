using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewInitialTutorial", menuName = "ScriptableObjects/Tutorials/Initial Tutorial")]
public class InitialTutorial : TutorialInstruction
{
    // Initial game state
    private int plantationZones;

    public override void StartInstruction(int index)
    {
        // Call base Start method
        base.StartInstruction(index);

        // Set initial game state
        plantationZones = GridController.instance.zones.positions.Count;

        // If the initial index is invalid, exit
        if(initialIndex == -1)
        {
            return;
        }

        // For the initial tutorial, update UI at the start method
        instructionIndex = initialIndex;
        TutorialManager.instance.UpdateUIDelay(1f, instructions[instructionIndex], true);
    }

    public override void SetupActions()
    {
        // Call base SetupActions method
        base.SetupActions();

        //// Set the actions
        // Move
        actions[1] += MoveAround;

        // Inventory
        actions[2] += OpenInventory;
        actions[3] += ChangeInvetoryTab;
        actions[4] += CloseInventory;

        // Plantation
        actions[5] += StartEditMode;
        actions[6] += CreatePlantationZone;
        actions[7] += StopEditMode;
        actions[8] += PlantSomething;
    }




    #region Actions
    /// <summary>
    /// Check if player is moving.
    /// </summary>
    private void MoveAround()
    {
        actionCompleted = MovementController.instance.IsCharacterMoving();
    }

    /// <summary>
    /// Check if player opened inventory.
    /// </summary>
    private void OpenInventory()
    {
        actionCompleted = InventoryUI.instance.inventoryCanvas.activeSelf;
    }

    /// <summary>
    /// Check if player has changed inventory tab.
    /// </summary>
    private void ChangeInvetoryTab()
    {
        actionCompleted = InventoryUI.instance.GetHarvested();
    }

    /// <summary>
    /// Check if player closed inventory.
    /// </summary>
    private void CloseInventory()
    {
        actionCompleted = !InventoryUI.instance.inventoryCanvas.activeSelf;
    }

    /// <summary>
    /// Check if player is on edit mode.
    /// </summary>
    private void StartEditMode()
    {
        actionCompleted = PlayerDataManager.instance.GetEditMode();
    }

    /// <summary>
    /// Check if player created a plantation zone.
    /// </summary>
    private void CreatePlantationZone()
    {
        actionCompleted = plantationZones != GridController.instance.zones.positions.Count;
    }

    /// <summary>
    /// Check if player is not on edit mode.
    /// </summary>
    private void StopEditMode()
    {
        actionCompleted = !PlayerDataManager.instance.GetEditMode();
    }

    /// <summary>
    /// Check if player have planted something.
    /// </summary>
    private void PlantSomething()
    {
        // Get all plants and loop them
        List<Plant> plants = GridController.instance.zones.GetPlants();
        foreach(Plant plant in plants)
        {
            // If there is a plant, set action to complete and exit
            if(plant != null)
            {
                actionCompleted = true;
                return;
            }
        }

        // If there are no planted plants, set action to incomplete
        actionCompleted = false;
    }
    #endregion
}
