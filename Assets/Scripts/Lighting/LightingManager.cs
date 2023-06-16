using UnityEngine;


public class LightingManager : MonoBehaviour
{
    // Light
    [SerializeField] private Light directionalLight;
    [SerializeField] private LightPreset preset;

    // Time of day
    [SerializeField, Range(0, 24)] private float timeOfDay;

    // Clock
    private float clock;
    private float baseClock;
    private float hour;


    void Awake()
    {
        // If there is no directional light, exit
        if(directionalLight != null)
        {
            return;
        }

        // If there is a sun, set it to the directional light
        if(RenderSettings.sun != null)
        {
            directionalLight = RenderSettings.sun;
        }

        // If there is no sun
        else
        {
            // Find all lights and loop it
            Light[] lights = FindObjectsOfType<Light>();
            foreach(Light light in lights)
            {
                // If there is any directional light, get it
                if(light.type == LightType.Directional)
                {
                    directionalLight = light;
                    return;
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // This formula was achieved solving the following condition:
        //     - The clock seconds was defined by 10, so each hour have 10 seconds
        //     - So te best time to update 0.05 unit of light is after 0.5 seconds
        baseClock = (0.5f * TimeManager.instance.baseClockSeconds) / 10f;
        clock = baseClock;
        timeOfDay = (float)TimeManager.instance.hour;
    }

    // Update is called once per frame
    void Update()
    {
        // If there is no preset, exit()
        if(preset == null)
        {
            return;
        }
            

        // Set time
        hour = (float)TimeManager.instance.hour;
        clock -= Time.deltaTime;

        // If a clock have been completed, update the light
        if(clock < 0)
        {
            clock = baseClock;
            timeOfDay += 0.05f;
            timeOfDay %= 24f;
            UpdateLighting(timeOfDay / 24);
        }

        // If a clock desync happens, correct it
        if(timeOfDay < hour)
        {
            timeOfDay = hour;
            UpdateLighting(timeOfDay / 24);
        }
    }

    /// <summary>
    /// Update the lighting, based on the preset.
    /// </summary>
    /// <param name="timePercent">Percent of the day.</param>
    private void UpdateLighting(float timePercent)
    {
        // Set the ambient ligh and the fog color
        RenderSettings.ambientLight = preset.ambientColor.Evaluate(timePercent);
        RenderSettings.fogColor = preset.fogColor.Evaluate(timePercent);

        // If there is a directional light, set its color
        if(directionalLight != null)
        {
            directionalLight.color = preset.directionalColor.Evaluate(timePercent);
        }
    }
}
