using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Time", menuName = "ScriptableObjects/TimeSystem")]
public class TimeSystem : ScriptableObject
{
    // Date
    public int hour;
    public int day;
    public List<int> daysToChangeSeason;

    // Clock
    public float baseClockSeconds;

    // Seasons
    [Range(0, 3)] public int season;
}
