using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;

public class XRSceneLoader : MonoBehaviour {
    public string sceneName = "SceneName"; // The name of the scene to load

    private void Start() {
        // Get the Button component and add the XR-compatible click listener
        Button button = GetComponent<Button>();
        if (button != null) {
            button.onClick.AddListener(LoadScene);
        } else {
            Debug.LogWarning("No Button component found on this GameObject.");
        }

        // Ensure the button has an XRUIInputModule in the EventSystem
        EnsureXRInputModule();
    }

    public void LoadScene() {
        Debug.Log("Loading Scene: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }

    private void EnsureXRInputModule() {
        // Find or create an EventSystem with XR support
        GameObject eventSystem = GameObject.Find("EventSystem");
        if (eventSystem == null) {
            eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
            eventSystem.AddComponent<UnityEngine.XR.Interaction.Toolkit.UI.XRUIInputModule>();
            Debug.Log("Created an EventSystem with XRUIInputModule.");
        }
    }
}
