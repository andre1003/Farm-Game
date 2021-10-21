using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 2)]
public class PlayerData : ScriptableObject {
    public new string name;

    public int level;
    public int xp;

    public float money;
}
