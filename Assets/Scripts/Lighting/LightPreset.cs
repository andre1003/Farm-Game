using UnityEngine;


[System.Serializable]
[CreateAssetMenu(fileName = "Light Preset", menuName = "ScriptableObjects/Light Presets")]
public class LightPreset : ScriptableObject {
    public Gradient ambientColor;
    public Gradient directionalColor;
    public Gradient fogColor;
}
