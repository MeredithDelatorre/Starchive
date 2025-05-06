using System.Collections.Generic;
using UnityEngine;

/// <summary> Listens to HitTracker, draws the live line, and can “finalise” it. </summary>
public class ConstellationBuilder : MonoBehaviour {
    [Header("References")]
    [SerializeField] private HitTracker hitTracker;        
    [SerializeField] private Transform bowTip;             
    [SerializeField] private GameObject starPrefab;        
    [SerializeField] private float bowDisplayScale = .05f; 

    public GameObject CurrentConstellation { get; set; } = null;
    private readonly List<Vector3> points = new();

    private void Awake() {
        hitTracker.StarCreated += OnStarCreated;
    }

    private void OnDestroy() {
        hitTracker.StarCreated -= OnStarCreated;
    }

    private void OnStarCreated(Vector3 p) => points.Add(p);


    public void Finalise() {
        if (CurrentConstellation != null || points.Count < 2) return;

        // 1) build a constellation root
        CurrentConstellation = new GameObject("Constellation");
        LineRenderer lr = CurrentConstellation.AddComponent<LineRenderer>();
        lr.useWorldSpace = false;
        lr.widthMultiplier = .02f;
        lr.positionCount = points.Count;
        lr.material = hitTracker.lineRenderer.material;

        // 2) convert each world‑space point into the constellation’s local space
        Vector3 centre = GetCentre(points);
        for (int i = 0; i < points.Count; i++) {
            Vector3 localPos = points[i] - centre;
            lr.SetPosition(i, localPos);
            Instantiate(starPrefab, localPos, Quaternion.identity, CurrentConstellation.transform);
        }

        // 3) park it on the bow tip, shrunken
        CurrentConstellation.transform.SetParent(bowTip);
        CurrentConstellation.transform.localPosition = Vector3.zero;
        CurrentConstellation.transform.localRotation = Quaternion.identity;
        CurrentConstellation.transform.localScale = Vector3.one * bowDisplayScale;

        // 4) reset for a new round
        points.Clear();
        hitTracker.lineRenderer.positionCount = 0;   // hide the “drawing” line
    }

    private static Vector3 GetCentre(IReadOnlyList<Vector3> vs) {
        Vector3 sum = Vector3.zero;
        foreach (var v in vs) sum += v;
        return sum / vs.Count;
    }
}
