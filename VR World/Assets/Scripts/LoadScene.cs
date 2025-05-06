using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour {
    [Header("Scene Settings")]
    public string sceneNameToLoad;

    private void OnTriggerEnter(Collider other) {
        // Optional: Only trigger if the player collides
        if (other.CompareTag("Player")) {
            Debug.Log("Player collided, loading scene: " + sceneNameToLoad);
            SceneManager.LoadScene(sceneNameToLoad);
        }
    }

    public void LoadSceneWithButton(string sceneName = "") {
        string target = string.IsNullOrWhiteSpace(sceneName) ? sceneNameToLoad : sceneName;

        Debug.Log($"Button pressed, loading scene: {target}");
        SceneManager.LoadScene(target);
    }
}