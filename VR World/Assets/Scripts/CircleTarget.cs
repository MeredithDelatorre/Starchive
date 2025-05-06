using UnityEngine;
using System;

[RequireComponent(typeof(Collider))]
public class CircleTarget : MonoBehaviour {
    [Header("Visuals")]
    [SerializeField] Renderer circleRenderer;          // assign in Inspector
    [SerializeField] Color activatedColour = Color.cyan;
    [SerializeField] float glowIntensity = 4f;

    public bool IsActivated { get; private set; } = false;
    public event Action<CircleTarget> OnActivated;     // constellation manager subscribes

    MaterialPropertyBlock _props;
    static readonly int Emission = Shader.PropertyToID("_EmissionColor");
    Color _originalColour;

    void Awake() {
        if (circleRenderer == null) circleRenderer = GetComponent<Renderer>();
        _props = new MaterialPropertyBlock();
        circleRenderer.GetPropertyBlock(_props);
        _originalColour = _props.GetVector(Emission);
    }

    public void Activate() {
        if (IsActivated) return;                       // ignore duplicate arrows
        IsActivated = true;

        _props.SetColor(Emission, activatedColour * glowIntensity);
        circleRenderer.SetPropertyBlock(_props);

        OnActivated?.Invoke(this);
    }

    public void ResetVisual() {
        _props.SetColor(Emission, _originalColour);
        circleRenderer.SetPropertyBlock(_props);
        IsActivated = false;
    }
}
