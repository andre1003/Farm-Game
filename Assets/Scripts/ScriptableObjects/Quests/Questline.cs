using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "NewQuestline", menuName = "ScriptableObjects/Quests/Questline")]
public class Questline : ScriptableObject
{
    // ID
    [Header("ID")]
    public string id;

    // Localization
    [Header("Localization")]
    public LocalizedStringTable questlinesLocalization;
    public LocalizedStringTable questTitlesLocalization;
    public LocalizedStringTable questDescriptionsLocalization;
    public LocalizedStringTable questShortDescriptionsLocalization;

    // Quest actions
    public delegate void QuestAction();
    public List<QuestAction> questActions;

    // Quests
    [Header("Quests")]
    public List<Quest> quests = new List<Quest>();

    // Controller
    [Header("Controllers")]
    public bool completed = false;


    // Controller
    [SerializeField] protected int questIndex = -1;
    [SerializeField] protected int initialIndex = -1;
    protected bool actionCompleted = false;
    protected bool canCallNextQuest = true;


    // Index
    public int index;


    #region Start
    /// <summary>
    /// Questline initial setups.
    /// </summary>
    public virtual void StartQuestline(int index)
    {
        // Set questline index on list
        SetIndex(index);

        // Setup quest initial index
        SetupInitialIndex();

        // Setup questline actions
        SetupActions();
    }

    /// <summary>
    /// Set questline list index.
    /// </summary>
    /// <param name="index">Questline list index.</param>
    public void SetIndex(int index)
    {
        this.index = index;
    }

    /// <summary>
    /// Base initial index setup. Can be extended.
    /// </summary>
    protected virtual void SetupInitialIndex()
    {
        // If this quest is completed, set both index to -1, remove from questlines list and exit
        if(completed)
        {
            initialIndex = -1;
            questIndex = -1;
            QuestManager.instance.RemoveQuestline(index);
            return;
        }

        // If the quest index is -1, set the initial to 0
        if(questIndex == -1)
        {
            initialIndex = 0;
        }

        // If not, set initial index to quest index
        else
        {
            initialIndex = questIndex;
        }
    }

    /// <summary>
    /// Base questline actions setup. This method must be extended to work propertly.
    /// </summary>
    public virtual void SetupActions()
    {
        // Set can call next question
        canCallNextQuest = true;

        // Create quest actions list
        questActions = new List<QuestAction>();
        foreach(Quest quest in quests)
        {
            questActions.Add(null);
        }

        questActions[0] += TestOnlyMethod;
    }
    #endregion

    #region Update
    /// <summary>
    /// Update questline method.
    /// </summary>
    /// <returns>TRUE - If this questline is active. FALSE - If not.</returns>
    public void UpdateQuestline()
    {
        // If quest index is not -1 or this quest is not complete, check action update
        if(questIndex != -1 && !completed)
        {
            // Check action update
            CheckAction();
        }
    }

    /// <summary>
    /// Check for action update.
    /// </summary>
    private void CheckAction()
    {
        // If there is no quest left, exit
        if(questIndex == -1)
        {
            return;
        }

        // If there is no action to be made, exit
        if(questActions[questIndex] == null)
        {
            // If can get next quest, get it
            if(canCallNextQuest)
            {
                canCallNextQuest = false;
                NextQuest();
            }

            return;
        }

        // If the action have been completed
        if(actionCompleted)
        {
            // If can get next quest, get it
            if(canCallNextQuest)
            {
                canCallNextQuest = false;
                NextQuest();
            }
        }

        // If action have NOT been completed, invoke it
        else
        {
            questActions[questIndex].Invoke();
        }
    }
    #endregion

    #region Next Quest
    /// <summary>
    /// Get next quest.
    /// </summary>
    public void NextQuest()
    {
        // If quest index is invalid
        if(questIndex == -1)
        {
            // Check if initial index is also invalid. If it is, clear UI, remove from list and exit
            if(initialIndex == -1)
            {
                QuestManager.instance.UpdateUI(index);
                QuestManager.instance.RemoveQuestline(index);
                completed = true;
                return;
            }

            // If initial index is valid, set current index to it, update UI and exit.
            else
            {
                // Set quest index to initial index
                questIndex = initialIndex;

                // Localize quest strings
                GetInfo();

                // Set can call next quest to true
                canCallNextQuest = true;

                // Exit
                return;
            }
        }

        // If the current quest has an action and it has not been completed yet,
        // clear UI and exit
        if(questActions[questIndex] != null && !actionCompleted)
        {
            GetInfo();
            return;
        }

        // Give quest rewards
        GiveRewards(questIndex);

        // Increment quest index and reset action completed controller
        questIndex++;
        actionCompleted = false;

        // If the quest index is out of bounds, complete questline and exit
        if(questIndex >= quests.Count)
        {
            // Set indexes to -1 (invalid)
            questIndex = -1;
            initialIndex = -1;

            // Deactivate questline and mark as completed
            completed = true;

            // Clear UI and remove questline from list
            //QuestManager.instance.UpdateUI(index);
            QuestManager.instance.RemoveQuestline(index);

            // Display questline complete UI
            string questlineName = StringLocalizer.instance.Localize(questlinesLocalization, id);
            QuestManager.instance.QuestlineCompleted(questlineName);

            // Exit
            return;
        }

        // Localize quest strings
        GetInfo();

        // Allow action check
        canCallNextQuest = true;
    }

    /// <summary>
    /// Give quest rewards to player.
    /// </summary>
    public void GiveRewards(int index)
    {
        // Give XP and money
        PlayerDataManager.instance.SetXp(quests[index].xp);
        PlayerDataManager.instance.SetMoney(quests[index].money);

        // Get items and amounts list
        int length = quests[index].items.Count;
        List<Item> items = quests[index].items;
        List<int> amounts = quests[index].amounts;

        // Give to player
        for(int i = 0; i < length; i++)
        {
            Inventory.instance.Add(items[i], amounts[i]);
        }
    }
    #endregion

    /// <summary>
    /// Get 
    /// </summary>
    public void GetInfo()
    {
        // If there is no quest, exit
        if(quests == null || quests.Count <= 0)
        {
            return;
        }

        // Get table key
        string key = quests[questIndex].id;

        // Localize quest strings
        string title = StringLocalizer.instance.Localize(questTitlesLocalization, key);
        string description = StringLocalizer.instance.Localize(questDescriptionsLocalization, key);
        string shortDescription = StringLocalizer.instance.Localize(questShortDescriptionsLocalization, key);
        QuestManager.instance.UpdateUI(index, title, description, shortDescription, true);
    }


    private void TestOnlyMethod()
    {
        actionCompleted = MovementController.instance.IsCharacterMoving();
    }
}
