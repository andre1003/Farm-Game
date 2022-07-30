using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour {
    #region Singleton
    public static TimeManager instance;

    void Awake() {
        if(instance != null) {
            return;
        }

        instance = this;
    }
    #endregion

    // Time
    public TextMeshProUGUI time;

    // Date
    public int hour = 12;
    public int day = 1;
    public int daysToChangeSeason = 10;

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
        time.text = time.text.Split(' ')[0] + " " + hour.ToString("00");
        clockSeconds -= Time.deltaTime;

        if(clockSeconds < 0) {
            clockSeconds = baseClockSeconds;
            hour++;

            if(hour > 23) {
                day++;
                if(day % daysToChangeSeason == 0)
                    season++;
                hour = 0;
            }
                
        }

        SetSeason();
    }

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
}
