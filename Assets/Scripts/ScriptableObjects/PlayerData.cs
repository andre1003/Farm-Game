using UnityEngine;


[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 2)]
public class PlayerData : ScriptableObject
{
    // Name
    public new string name;

    // Level
    public int level;
    public int xp;
    public int nextLvlXp;

    // Money
    public float money;
}
