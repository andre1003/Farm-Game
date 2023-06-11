using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour {
    #region Singleton
    public TimeSystem timeSystem;
    public static TimeManager instance;

    void Awake() {
        if(instance != null)
        {
            return;
        }

        instance = this;

        // Load saved time
        if(timeSystem != null)
        {
            day = timeSystem.day;
            hour = timeSystem.hour;
            daysToChangeSeason = timeSystem.daysToChangeSeason;
            baseClockSeconds = timeSystem.baseClockSeconds;
            season = timeSystem.season;
        }
        
    }
    #endregion

    // Time
    public TextMeshProUGUI time;

    // Date
    public int hour = 12;
    public int day = 1;
    public List<int> daysToChangeSeason;

    // Clock
    public float baseClockSeconds = 10f;

    // Seasons
    [Range(0, 3)] public int season = 0;
    public SeasonsLocalizedStrings seasons;
    

    // Clock sencods
    private float clockSeconds;

    // Start is called before the first frame update
    void Start() {
        clockSeconds = baseClockSeconds;
        time.text += hour.ToString("00");
        SetSeason();
    }

    // Update is called once per frame
    void Update() {
        // Set time text
        time.text = time.text.Split(' ')[0] + " " + hour.ToString("00");

        // Decrease clock
        clockSeconds -= Time.deltaTime;

        // If a clock cycle has been completed
        if(clockSeconds < 0) {
            // Reset clockSeconds
            clockSeconds = baseClockSeconds;
            
            // Increase hour
            hour++;

            // If hour is bigger then 23
            if(hour > 23) {
                // Increase day
                day++;

                // Check season change
                if(day % daysToChangeSeason[season] == 0)
                    season = season >= 3 ? 0 : season + 1;

                // Reset hour
                hour = 0;
            }
                
        }

        // Set season
        SetSeason();
    }

    // Set season accordingly to season variable
    private void SetSeason() {
        switch(season) {
            case 0:
                seasons.tableName = "Winter";
                break;

            case 1:
                seasons.tableName = "Spring";
                break;

            case 2:
                seasons.tableName = "Summer";
                break;

            case 3:
                seasons.tableName = "Autumn";
                break;
        }
    }

    /// <summary>
    /// Called when quit application.
    /// </summary>
    void OnApplicationQuit()
    {
        // If there is no time system, quit
        if(timeSystem == null)
        {
            return;
        }

        // Save current time
        timeSystem.day = day;
        timeSystem.hour = hour;
        timeSystem.daysToChangeSeason = daysToChangeSeason;
        timeSystem.season = season;
    }
}
