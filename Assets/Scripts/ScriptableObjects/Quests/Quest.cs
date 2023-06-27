using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "NewQuest", menuName = "ScriptableObjects/Quests/Quest", order = 1)]
public class Quest : ScriptableObject
{
    // Info
    public string id;

    // Rewards
    public int xp;
    public int money;
    public List<Item> items;
    public List<int> amounts;
}
