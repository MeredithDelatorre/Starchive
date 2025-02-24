using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PopupHandler : MonoBehaviour {
    public GameObject popup;

    private void Start() {
        popup.SetActive(false); 
    }

    private void OnEnable() {
        XRBaseInteractable interactable = GetComponent<XRBaseInteractable>();
        interactable.hoverEntered.AddListener(OnHoverEnter);
        interactable.hoverExited.AddListener(OnHoverExit);
    }

    private void OnDisable() {
        XRBaseInteractable interactable = GetComponent<XRBaseInteractable>();
        interactable.hoverEntered.RemoveListener(OnHoverEnter);
        interactable.hoverExited.RemoveListener(OnHoverExit);
    }

    private void OnHoverEnter(HoverEnterEventArgs args) {
        popup.SetActive(true);
    }

    private void OnHoverExit(HoverExitEventArgs args) {
        popup.SetActive(false);
    }

}
