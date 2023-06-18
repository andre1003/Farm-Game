using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class TutorialManager : MonoBehaviour
{
    // Instruction actions
    public delegate void InstructionAction();
    public List<InstructionAction> actions;

    // UI
    public GameObject instructionCanvas;
    public TextMeshProUGUI instructionText;

    // Instruction
    public List<string> instructions = new List<string>();
    public List<float> instructionTimers = new List<float>();


    // Instruction
    private int instructionIndex = 0;
    private bool actionCompleted = false;
    private bool canCallNextInstruction = true;


    // Awake method
    void Awake()
    {
        // Get tutorial instruction index from save game
        instructionIndex = PlayerDataManager.instance.playerData.instructionIndex;
    }

    // Start is called before the first frame update
    void Start()
    {
        // If there is no canvas, or text or instruction, exit
        if(instructionCanvas == null || instructionText == null || instructions.Count == 0 || instructionIndex == -1)
        {
            return;
        }

        // Update instruction UI
        StartCoroutine(WaitForUpdateUI());

        // Setup all tutorial actions
        SetupActions();
    }

    // Update method
    void Update()
    {
        // Check for action update, if any
        CheckAction();
    }

    private IEnumerator WaitForUpdateUI()
    {
        yield return new WaitForSeconds(1f);
        UpdateUI(instructions[instructionIndex], true);
    }

    private void SetupActions()
    {
        // Set actions list, adding instructions.Count actions.
        // This is for getting an action with the sabe index as the instructions
        actions = new List<InstructionAction>();
        foreach(string instruction in instructions)
        {
            actions.Add(null);
        }

        // Set the actions
        actions[1] += MoveAround;
        actions[2] += OpenInventory;
    }


    /// <summary>
    /// Get next instruction, if any.
    /// </summary>
    public void NextInstruction()
    {
        // If instruction index is -1, exit
        if(instructionIndex == -1)
        {
            UpdateUI("", false);
            return;
        }

        // If there is an action to be executed and it's not completed, exit
        if(actions[instructionIndex] != null && !actionCompleted)
        {
            UpdateUI("", false);
            return;
        }

        // Increase instruction index and save it
        instructionIndex++;
        PlayerDataManager.instance.playerData.instructionIndex = instructionIndex;

        // Set action completed to false
        actionCompleted = false;

        // If instruction index is out of bounds, set it to -1, save it and exit
        if(instructionIndex >= instructions.Count)
        {
            instructionIndex = -1;
            PlayerDataManager.instance.playerData.instructionIndex = instructionIndex;
            UpdateUI("", false);
            return;
        }

        // If instruction index is valid, set instruction text
        UpdateUI(instructions[instructionIndex], true);

        // Allow action check to work
        canCallNextInstruction = true;
    }

    /// <summary>
    /// Get previous instruction.
    /// </summary>
    public void PreviousInstruction()
    {
        // If instruction index is -1, exit
        if(instructionIndex == -1)
        {
            UpdateUI("", false);
            return;
        }

        // Decrease instruction index
        instructionIndex--;

        // If instruction index is less than 0, set it to 0
        if(instructionIndex < 0)
        {
            instructionIndex = 0;
        }

        // Save tutorial instruction index
        PlayerDataManager.instance.playerData.instructionIndex = instructionIndex;

        // Set instruction text
        UpdateUI(instructions[instructionIndex], true);
    }

    /// <summary>
    /// Get instruction at a given index.
    /// </summary>
    /// <param name="index">Index of instruction.</param>
    public void InstructionAtIndex(int index)
    {
        // If the given index is not valid, exit
        if(index < 0 ||  index >= instructions.Count)
        {
            UpdateUI("", false);
            return;
        }

        // Set instruction index and text
        instructionIndex = index;
        PlayerDataManager.instance.playerData.instructionIndex = instructionIndex;
        UpdateUI(instructions[instructionIndex], true);
    }

    /// <summary>
    /// Update instruction UI.
    /// </summary>
    /// <param name="instruction">Instruction to display.</param>
    /// <param name="canvasActive">Active status of instruction canvas.</param>
    public void UpdateUI(string instruction, bool canvasActive)
    {
        // Set instruction canvas active status and text
        instructionCanvas.SetActive(canvasActive);
        instructionText.text = instruction;

        // If canvas is active, pause game
        if(canvasActive)
        {
            PauseGame();
        }

        // If canvas is NOT active, resume game
        else
        {
            ResumeGame();
        }
    }

    /// <summary>
    /// Pause game completly.
    /// </summary>
    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    /// <summary>
    /// Resume game completly.
    /// </summary>
    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    /// <summary>
    /// Check for action update.
    /// </summary>
    private void CheckAction()
    {
        // If there is no instruction for the player, exit
        if(instructionIndex == -1)
        {
            return;
        }

        // If there is no action to be made, exit
        if(actions[instructionIndex] == null)
        {
            return;
        }

        // If the action have been completed, get next instruction
        if(actionCompleted)
        {
            if(canCallNextInstruction)
            {
                canCallNextInstruction = false;
                StartCoroutine(WaitForNextInstruction());
            }
        }

        // If the action have NOT been completed, invoke it
        else
        {
            actions[instructionIndex].Invoke();
        }        
    }

    /// <summary>
    /// Wait 0.5 seconds and get next instructions.
    /// </summary>
    private IEnumerator WaitForNextInstruction()
    {
        yield return new WaitForSeconds(instructionTimers[instructionIndex]);
        NextInstruction();
    }

    /// <summary>
    /// Complete the current action.
    /// </summary>
    public void CompleteAction()
    {
        actionCompleted = true;
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
    #endregion
}
