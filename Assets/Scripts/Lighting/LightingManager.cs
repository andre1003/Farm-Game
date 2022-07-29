using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingManager : MonoBehaviour {
    public ObjectsManager manager;

    [SerializeField] private Light directionalLight;
    [SerializeField] private LightPreset preset;
    [SerializeField, Range(0, 24)] private float timeOfDay;

    private float clock;
    private float baseClock;
    private float hour;

    void Awake() {
        if(directionalLight != null)
            return;

        if(RenderSettings.sun != null)
            directionalLight = RenderSettings.sun;
        else {
            Light[] lights = FindObjectsOfType<Light>();
            foreach(Light light in lights) {
                if(light.type == LightType.Directional) {
                    directionalLight = light;
                    return;
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start() {
        // This formula was achieved solving the following condition:
        // The clock seconds was defined by 10, so each hour have 10 seconds
        // So te best time to update 0.05 unit of light is after 0.5 seconds
        baseClock = (0.5f * InGameSaves.GetClockSeconds()) / 10f;
        Debug.Log(baseClock);
        clock = baseClock;
        timeOfDay = (float)manager.hour;
    }

    // Update is called once per frame
    void Update() {
        if(preset == null)
            return;
        
        // Time set
        hour = (float)manager.hour;
        clock -= Time.deltaTime;

        // If a clock has completed, update light
        if(clock < 0) {
            clock = baseClock;
            timeOfDay += 0.05f;
            timeOfDay %= 24f;
            UpdateLighting(timeOfDay / 24);
        }

        // If some clock desync happens, correct it
        if(timeOfDay < hour) {
            timeOfDay = hour;
            UpdateLighting(timeOfDay / 24);
        }
    }

    private void UpdateLighting(float timePercent) {
        RenderSettings.ambientLight = preset.ambientColor.Evaluate(timePercent);
        RenderSettings.fogColor = preset.fogColor.Evaluate(timePercent);

        if(directionalLight != null)
            directionalLight.color = preset.directionalColor.Evaluate(timePercent);
    }
}
