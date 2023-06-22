using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewInstructions", menuName = "ScriptableObjects/Instructions")]
public class TutorialInstruction : ScriptableObject
{
    // Actions
    public delegate void InstructionAction();
    public List<InstructionAction> actions;

    // Instructions
    [TextArea]
    public List<string> instructions;

    // Timers
    public List<float> timers;

    // Active
    public bool isActive = false;


    // Controllers
    [SerializeField] protected int instructionIndex = -1;
    [SerializeField] protected int initialIndex = -1;
    protected bool actionCompleted = false;
    protected bool canCallNextInstruction = true;

    // Auxiliar
    private int index = -1;


    /// <summary>
    /// Start instruction method.
    /// </summary>
    /// <param name="index">Tutorial index.</param>
    public virtual void StartInstruction(int index)
    {
        // Setup index
        this.index = index;

        // Initial index setup
        initialIndex = instructionIndex;
        instructionIndex = -1;

        // Setup all tutorial actions
        SetupActions();
    }

    /// <summary>
    /// Update instruction.
    /// </summary>
    /// <returns>TRUE - If this instruction is active. FALSE - If not.</returns>
    public bool UpdateInstruction()
    {
        // If instruction index is not -1 or this instruction is active, check action update
        if(instructionIndex != -1 && isActive)
        {
            // Check action update
            CheckAction();
        }

        // Return isActive value
        return isActive;
    }

    /// <summary>
    /// Base setup actions. This class must be extended to work propertly.
    /// </summary>
    public virtual void SetupActions()
    {
        actions = new List<InstructionAction>();
        foreach(string instruction in instructions)
        {
            actions.Add(null);
        }
    }

    /// <summary>
    /// Get next instruction, if any.
    /// </summary>
    public void NextInstruction()
    {
        // If instruction index is -1
        if(instructionIndex == -1)
        {
            // If the initial index is also -1, close tutorial UI and exit
            if(initialIndex == -1)
            {
                TutorialManager.instance.UpdateUIDelay();
                return;
            }

            // If the initial index is valid
            else
            {
                // Set the instruction index to initialIndex
                instructionIndex = initialIndex;

                // Set instruction text
                TutorialManager.instance.UpdateUI(instructions[instructionIndex], true);

                // Allow action check to work
                canCallNextInstruction = true;

                // Set active to true
                isActive = true;

                // Exit
                return;
            }
        }

        // If there is an action to be executed and it's not completed, exit
        if(actions[instructionIndex] != null && !actionCompleted)
        {
            TutorialManager.instance.UpdateUIDelay();
            return;
        }

        // Increase instruction index
        instructionIndex++;

        // Set action completed to false
        actionCompleted = false;

        // If instruction index is out of bounds, set it to -1 and exit
        if(instructionIndex >= instructions.Count)
        {
            instructionIndex = -1;
            initialIndex = -1;
            isActive = false;
            TutorialManager.instance.UpdateUIDelay();
            return;
        }

        // If instruction index is valid, set instruction text
        TutorialManager.instance.UpdateUI(instructions[instructionIndex], true);

        // Allow action check to work
        canCallNextInstruction = true;
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
            // If can call next instruction, call it
            if(canCallNextInstruction)
            {
                canCallNextInstruction = false;
                TutorialManager.instance.NextInstructionDelay(timers[instructionIndex]);
            }
        }

        // If the action have NOT been completed, invoke it
        else
        {
            actions[instructionIndex].Invoke();
        }
    }
}
