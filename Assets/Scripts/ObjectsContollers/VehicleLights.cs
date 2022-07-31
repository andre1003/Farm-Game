using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleLights : MonoBehaviour {
    public List<GameObject> lights;
    public int lightsOnTime = 20;
    public int lightsOffTime = 5;

    private bool isLightsOn = false;
    private float intensity = 0.5f;


    // Update is called once per frame
    void Update() {
        if(!isLightsOn)
            SetLights(lightsOnTime, true);
        else
            SetLights(lightsOffTime, false);
        
    }

    private void SetLights(int lightsTime, bool value) {
        if(TimeManager.instance.hour == lightsTime) {
            float maxIntensity = 0f;
            if(value)
                maxIntensity = 2f;
            else
                maxIntensity = 0.5f;

            foreach(GameObject light in lights) {
                light.SetActive(value);
                intensity = Mathf.Lerp(intensity, maxIntensity + 0.2f, 2f * Time.deltaTime);
                light.GetComponent<Light>().intensity = intensity;
            }

            if((value && intensity >= maxIntensity) || (!value && intensity <= maxIntensity))
                isLightsOn = value;
        }
    }
}
