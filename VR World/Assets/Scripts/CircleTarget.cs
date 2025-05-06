using UnityEngine;
using System;

[RequireComponent(typeof(Collider))]
public class CircleTarget : MonoBehaviour {

    [SerializeField] Material defaultMaterial;    
    [SerializeField] Material activeMaterial;        
    [SerializeField] Renderer circleRenderer;  

    public bool IsActivated { get; private set; } = false;
    public event Action<CircleTarget> OnActivated;

    void Awake() {
        if (circleRenderer == null)
            circleRenderer = GetComponent<Renderer>();

        // If theres no set defaultMaterial, use whatever the renderer has in the scene
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
