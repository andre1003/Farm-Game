using UnityEngine;


[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 2)]
public class PlayerData : ScriptableObject
{
    // Name
    [Header("Player Information")]
    public new string name;

    // Level
    [Header("Level System")]
    public int level;
    public int xp;
    public int nextLvlXp;

    // Money
    [Header("Money")]
    public float money;
}
