using UnityEngine;

public class SkyboxSwitcher : MonoBehaviour {
    public Material[] skyboxes; // Array to store skybox materials
    private int currentSkyboxIndex = 0;

    void Start() {
        if (skyboxes.Length > 0) {
            RenderSettings.skybox = skyboxes[currentSkyboxIndex];
        }
    }

    public void NextSkybox() {
        currentSkyboxIndex = (currentSkyboxIndex + 1) % skyboxes.Length;
        RenderSettings.skybox = skyboxes[currentSkyboxIndex];
        DynamicGI.UpdateEnvironment(); // Updates lighting
    }

    public void PreviousSkybox() {
        currentSkyboxIndex = (currentSkyboxIndex - 1 + skyboxes.Length) % skyboxes.Length;
        RenderSettings.skybox = skyboxes[currentSkyboxIndex];
        DynamicGI.UpdateEnvironment(); // Updates lighting
    }
}
