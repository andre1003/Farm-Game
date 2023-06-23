using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class TutorialManager : MonoBehaviour
{
    #region Singleton
    // Instance
    public static TutorialManager instance;

    // Awake method
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    #endregion


    // Instructions
    public List<TutorialInstruction> instructions = new List<TutorialInstruction>();

    // UI
    public GameObject instructionCanvas;
    public TextMeshProUGUI instructionText;

    // Current tutorial index
    public int currentIndex = -1;


    // Start method
    void Start()
    {
        // Call Start method of each instruction
        for(int i = 0; i < instructions.Count; i++)
        {
            instructions[i].StartInstruction(i);
        }
    }

    // Update method
    void Update()
    {
        // Reset current index
        currentIndex = -1;

        // Call Start method of each instruction
        for(int i = 0; i < instructions.Count; i++)
        {
            // Call update for each instruction and check if it's active.
            // If this instruction is active, set currentIndex to it and exit.
            bool isActive = instructions[i].UpdateInstruction();
            if(isActive)
            {
                currentIndex = i;
                return;
            }
        }
    }

    /// <summary>
    /// Update UI with delay. Default value is to close tutorial UI.
    /// </summary>
    /// <param name="seconds">Seconds to wait for update UI. Default is 0.</param>
    /// <param name="text">Instruction text. Default is an empty string.</param>
    /// <param name="isActive">Activate UI canvas? Default is false.</param>
    public void UpdateUIDelay(float seconds = 0f, string text = "", bool isActive = false)
    {
        StartCoroutine(WaitForUpdateUI(seconds, text, isActive));
    }

    /// <summary>
    /// Wait for seconds to update UI.
    /// </summary>
    private IEnumerator WaitForUpdateUI(float seconds, string text, bool isActive)
    {
        yield return new WaitForSeconds(seconds);
        UpdateUI(text, isActive);
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
    /// Call next instructin with a delay.
    /// </summary>
    /// <param name="seconds">Seconds to wait before call next instruction.</param>
    public void NextInstructionDelay(float seconds)
    {
        StartCoroutine(WaitForNextInstruction(seconds));
    }

    /// <summary>
    /// Wait seconds for next instruction.
    /// </summary>
    /// <param name="seconds">Seconds to wait before call next instruction.</param>
    private IEnumerator WaitForNextInstruction(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        instructions[currentIndex].NextInstruction();
    }

    /// <summary>
    /// Call next instruction, if possible.
    /// </summary>
    public void NextInstruction()
    {
        // If index is invalid, exit
        if(currentIndex == -1)
        {
            return;
        }

        // Call the active instruction NextInstruction method
        instructions[currentIndex].NextInstruction();
    }

    /// <summary>
    /// Try get tutorial at a given index.
    /// </summary>
    /// <param name="tutorialKey">Tutorial key.</param>
    /// <returns>TRUE - If could start the tutorial. FALSE - If couldn't.</returns>
    public bool GetTutorial(string tutorialKey)
    {
        // Define tutorial index and make sure that tutorial key is in lower case
        tutorialKey = tutorialKey.ToLower();
        int tutorialIndex = TutorialKeyToIndex(tutorialKey);

        // If there is no active tutorial, and the index is valid, call next instruction and return true
        if(currentIndex == -1 && tutorialIndex != -1)
        {
            Debug.Log("Calling next instruction");
            instructions[tutorialIndex].NextInstruction();
            return true;
        }

        // Else, return false
        return false;
    }

    /// <summary>
    /// Pause the current tutorial.
    /// </summary>
    public void PauseTutorial()
    {
        // If there is an active tutorial, pause it
        if(currentIndex != -1)
        {
            instructions[currentIndex].isActive = false;
        }
    }

    /// <summary>
    /// Try to convert a given tutorial key to index.
    /// </summary>
    /// <param name="tutorialKey">Tutorial key to convert.</param>
    /// <returns>Tutorial list index. It returns -1 if the key is invalid.</returns>
    private int TutorialKeyToIndex(string tutorialKey)
    {
        // Tutorial index
        int tutorialIndex;

        // Get list index by key
        switch(tutorialKey)
        {
            // Initial tutorial
            case "initial":
                tutorialIndex = 0;
                break;

            // Store tutorial
            case "store":
                tutorialIndex = 1;
                break;

            // Workbench tutorial
            case "workbench":
                tutorialIndex = 2;
                break;

            // Invalid key
            default:
                tutorialIndex = -1;
                break;
        }

        // Return tutorial index
        return tutorialIndex;
    }

    // On application quit method
    void OnApplicationQuit()
    {
        // Deactivate all tutorials, except the first one (initial tutorial)
        for (int i = 1; i < instructions.Count; i++)
        {
            instructions[i].isActive = false;
        }
    }
}