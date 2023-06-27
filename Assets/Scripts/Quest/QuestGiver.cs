using UnityEngine;


public class QuestGiver : MonoBehaviour
{
    // Questline
    public Questline questline;


    /// <summary>
    /// Give questline to player.
    /// </summary>
    public void GiveQuest()
    {
        QuestManager.instance.AddQuestline(questline);
    }
}
