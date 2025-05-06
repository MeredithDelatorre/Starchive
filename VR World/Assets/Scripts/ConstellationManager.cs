using System.Collections.Generic;
using UnityEngine;

public class ConstellationManager : MonoBehaviour {
    public static ConstellationManager Instance { get; private set; }

    [Header("Scene references")]
    [SerializeField] Transform boardRoot;                   // parent of circles
    [SerializeField] Transform skyRoot;                     // empty object at “origin of sky”
    [SerializeField] GameObject starPrefab;                 // glowing star mesh / VFX
    [SerializeField] GameObject linePrefab;                 // prefab with a LineRenderer
    [SerializeField] float skyScale = 5f;                   // spread constellation in sky

    readonly List<CircleTarget> _picked = new();
    readonly List<GameObject> _boardLines = new();

    bool _waitingForSkyArrow = false;

    void Awake() {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;

        // hook every circle's event
        foreach (var circle in boardRoot.GetComponentsInChildren<CircleTarget>())
            circle.OnActivated += HandleCircleActivated;
    }

    /* ---------- on‑board phase ---------- */

    void HandleCircleActivated(CircleTarget circle) {
        if (_waitingForSkyArrow) return;                    // ignore if pattern already finalised
        if (_picked.Contains(circle)) return;

        _picked.Add(circle);

        if (_picked.Count > 1)                              // draw line to previous
            _boardLines.Add(DrawLine(_picked[^2].transform.position,
                                     _picked[^1].transform.position,
                                     parent: boardRoot));
    }

    GameObject DrawLine(Vector3 a, Vector3 b, Transform parent) {
        GameObject g = Instantiate(linePrefab, parent);
        var lr = g.GetComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.SetPosition(0, a);
        lr.SetPosition(1, b);
        return g;
    }

    /* ---------- UI button ---------- */

    public void FinaliseBoardPattern()                      // hooked up to your XR button
    {
        Debug.Log("<color=lime>FinaliseBoardPattern() called</color>");
        _waitingForSkyArrow = true;
        Debug.Log("Constellation finalised — shoot into the sky to place it!");
    }

    /* ---------- arrow hits sky ---------- */

    public void PlaceConstellation(Vector3 hitPoint) {
        if (!_waitingForSkyArrow) return;

        // parent to keep hierarchy tidy
        var constellationParent = new GameObject("Constellation").transform;
        constellationParent.position = hitPoint;
        constellationParent.SetParent(skyRoot, true);

        // 1. spawn stars
        var starWorldPos = new List<Vector3>();
        foreach (var circle in _picked) {
            Vector3 localOnBoard = boardRoot.InverseTransformPoint(circle.transform.position);
            Vector3 worldPos = constellationParent.TransformPoint(localOnBoard * skyScale);
            starWorldPos.Add(worldPos);
            Instantiate(starPrefab, worldPos, Quaternion.identity, constellationParent);
        }

        // 2. spawn connecting lines
        for (int i = 1; i < starWorldPos.Count; ++i)
            DrawLine(starWorldPos[i - 1], starWorldPos[i], constellationParent);

        Debug.Log($"Constellation placed with {_picked.Count} stars");

        // 3. tidy up board for next pattern
        ResetBoard();
    }

    void ResetBoard() {
        foreach (var circle in _picked) circle.ResetVisual();
        foreach (var l in _boardLines) Destroy(l);

        _picked.Clear();
        _boardLines.Clear();
        _waitingForSkyArrow = false;
    }
}
