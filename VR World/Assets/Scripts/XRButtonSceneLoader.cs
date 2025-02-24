using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class XRButtonSceneLoader : MonoBehaviour {
    public string targetScene = "TargetScene"; // Set in the Inspector
    public float delayBeforeSceneLoad = 1.5f; // Delay before scene transition

    private XRSimpleInteractable simpleInteractable;

    private void Start() {
        simpleInteractable = GetComponent<XRSimpleInteractable>();
        simpleInteractable.selectEntered.AddListener(OnButtonPressed);
    }

    private void OnButtonPressed(SelectEnterEventArgs args) {
        StartCoroutine(DelayedSceneLoad());
    }

    private IEnumerator DelayedSceneLoad() {
        // Wait for the specified delay
        yield return new WaitForSeconds(delayBeforeSceneLoad);

        // Load the next scene
        SceneManager.LoadScene(targetScene);
    }
}