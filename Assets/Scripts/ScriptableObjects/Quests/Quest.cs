using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : ScriptableObject
{
    // Info
    public new string name;
    public string description;

    // Rewards
    public int xp;
    public int money;
    public List<Item> items;
}
