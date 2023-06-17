using UnityEngine;


[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 2)]
public class PlayerData : ScriptableObject
{
    // Name
    [Header("Player Information")]
    [Space(5)]
    public new string name;

    // Level
    [Header("Level System")]
    [Space(5)]
    public int level;
    public int xp;
    public int nextLvlXp;

    // Money
    [Header("Money")]
    [Space(5)]
    public float money;

    // Tutorial
    [Header("Tutorial")]
    [Space(5)]
    public int instructionIndex;
}
