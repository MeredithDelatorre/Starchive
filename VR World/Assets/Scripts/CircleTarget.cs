using UnityEngine;
using System;

[RequireComponent(typeof(Collider))]
public class CircleTarget : MonoBehaviour {
    [Header("Materials")]
    [Tooltip("Material you want while the circle is inactive / reset")]
    [SerializeField] Material defaultMaterial;        // drag your normal board material here

    [Tooltip("Material to use AFTER the first arrow hits")]
    [SerializeField] Material activeMaterial;         // drag the bright ActiveColor material here

    [Header("Renderer")]
    [SerializeField] Renderer circleRenderer;         // assign in the Inspector

    public bool IsActivated { get; private set; } = false;
    public event Action<CircleTarget> OnActivated;

    void Awake() {
        if (circleRenderer == null)
            circleRenderer = GetComponent<Renderer>();

        // If you forgot to set defaultMaterial, use whatever the renderer has in the scene
        if (defaultMaterial == null)
            defaultMaterial = circleRenderer.sharedMaterial;
    }

    public void Activate() {
        /* ---- first arrow: swap to the ActiveColor material ---- */
        if (!IsActivated) {
            IsActivated = true;
            circleRenderer.sharedMaterial = activeMaterial;
        }

        /* ---- every arrow (including repeats) notifies listeners ---- */
        OnActivated?.Invoke(this);
    }

    public void ResetVisual() {
        circleRenderer.sharedMaterial = defaultMaterial;
        IsActivated = false;
    }
}
