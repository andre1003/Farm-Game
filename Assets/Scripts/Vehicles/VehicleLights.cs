using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VehicleLights : MonoBehaviour
{
    // Light
    public List<GameObject> lights;
    public int lightsOnTime = 20;
    public int lightsOffTime = 5;


    // Light
    private bool isLightsOn = false;
    private float intensity = 0.5f;


    // Update is called once per frame
    void Update()
    {
        // If lights are off
        if(!isLightsOn)
        {
            SetLights(lightsOnTime, true);
        }

        // If lights are on
        else
        {
            SetLights(lightsOffTime, false);
        }
    }

    /// <summary>
    /// Set vehicle lights.
    /// </summary>
    /// <param name="lightsTime">Lights time.</param>
    /// <param name="value">On or off?</param>
    private void SetLights(int lightsTime, bool value)
    {
        // If the hour is equal the lightsTime
        if(TimeManager.instance.hour == lightsTime)
        {
            // Set maximum intensity
            float maxIntensity = 0f;
            if(value)
            {
                maxIntensity = 2f;
            }

            else
            {
                maxIntensity = 0.5f;
            }
                
            // Loop lights
            foreach(GameObject light in lights)
            {
                // Activate light
                light.SetActive(value);

                // Lerp intensity
                intensity = Mathf.Lerp(intensity, maxIntensity + 0.2f, 2f * Time.deltaTime);

                // Set intensity
                light.GetComponent<Light>().intensity = intensity;
            }

            // If everything is corret, set lights on or off.
            if((value && intensity >= maxIntensity) || (!value && intensity <= maxIntensity))
            {
                isLightsOn = value;
            }
        }
    }
}
