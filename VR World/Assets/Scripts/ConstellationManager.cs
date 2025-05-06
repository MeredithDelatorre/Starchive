using System.Collections.Generic;
using UnityEngine;

public class ConstellationManager : MonoBehaviour {
    public static ConstellationManager Instance { get; private set; }

    [Header("Scene references")]
    [SerializeField] Transform boardRoot;                   // parent of circles
    [SerializeField] Transform skyRoot;                     // empty object at “origin of sky”
    [SerializeField] GameObject starPrefab;                 // glowing star mesh / VFX
    [SerializeField] GameObject boardLinePrefab;            // thin linerenderer prefab
    [SerializeField] GameObject skyLinePrefab;              // wide linerenderer prefab
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
        if (_waitingForSkyArrow) return;

        /* 1. append this hit even if the same circle was used before */
        _picked.Add(circle);

        /* 2. draw a segment from the previous node to the new one */
        int n = _picked.Count;
        if (n > 1)
            _boardLines.Add(DrawLine(_picked[n - 2].transform.position,
                                     _picked[n - 1].transform.position,
                                     boardRoot));
    }

    GameObject DrawLine(Vector3 a, Vector3 b, Transform parent) {
        // choose thin or wide depending on where we’re drawing
        GameObject prefab = (parent == boardRoot) ? boardLinePrefab : skyLinePrefab;

        GameObject g = Instantiate(prefab, parent);
        var lr = g.GetComponent<LineRenderer>();

        lr.useWorldSpace = true;
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

    // ConstellationManager.cs
    public void PlaceConstellation(Vector3 hitPoint) {
        if (!_waitingForSkyArrow) return;

        /* ---------- create a tidy parent at the impact point ---------- */
        var constellationParent = new GameObject("Constellation").transform;
        constellationParent.position = hitPoint;
        constellationParent.SetParent(skyRoot, true);            // keeps hierarchy clean

        /* ---------- build a tangent basis that matches the BOARD’s axes ---------- */
        Vector3 domeCenter = skyRoot.position;                    // skyRoot = centre of dome
        Vector3 radial = (hitPoint - domeCenter).normalized;  // normal of the dome here
        float radius = (hitPoint - domeCenter).magnitude;   // dome radius

        // Project boardRight / boardUp onto the tangent plane so X & Y keep the board’s orientation
        Vector3 axisX = boardRoot.right - radial * Vector3.Dot(boardRoot.right, radial);
        if (axisX.sqrMagnitude < 1e-6f)                           // board was (nearly) radial
            axisX = Vector3.Cross(Vector3.up, radial);            // pick any perpendicular
        axisX.Normalize();

        Vector3 axisY = boardRoot.up - radial * Vector3.Dot(boardRoot.up, radial);
        axisY.Normalize();

        /* ---------- 1. centroid of picked circles (board‑local) ---------- */
        Vector3 centroidLocal = Vector3.zero;
        foreach (var c in _picked) centroidLocal += c.transform.localPosition;
        centroidLocal /= _picked.Count;

        /* ---------- 2. spawn the stars ---------- */
        var starWorldPos = new List<Vector3>();

        foreach (var circle in _picked) {
            Vector3 local = circle.transform.localPosition - centroidLocal; // ← shape centred

            // Map board X,Y onto the tangent basis and scale it
            Vector3 worldOffset = axisX * local.x * skyScale +
                                  axisY * local.y * skyScale;

            Vector3 pos = hitPoint + worldOffset;

            // Snap each star exactly onto the inner surface of the sphere
            pos = domeCenter + (pos - domeCenter).normalized * radius;

            starWorldPos.Add(pos);

            Instantiate(starPrefab,
                        pos,
                        Quaternion.LookRotation(radial),         // star faces viewer
                        constellationParent);
        }

        /* ---------- 3. draw the connecting lines ---------- */
        for (int i = 1; i < starWorldPos.Count; ++i) {
            GameObject g = Instantiate(skyLinePrefab);             // create with NO parent first
            var lr = g.GetComponent<LineRenderer>();

            lr.useWorldSpace = true;                             // points are in global coords
            lr.positionCount = 2;
            lr.SetPosition(0, starWorldPos[i - 1]);
            lr.SetPosition(1, starWorldPos[i]);

            g.transform.SetParent(constellationParent, true);    // keep hierarchy tidy
        }

        /* ---------- 4. reset the board for the next pattern ---------- */
        ResetBoard();
    }


    void ResetBoard() {
        /* Ensure each circle is reset only once even if it appears many times */
        foreach (var c in new HashSet<CircleTarget>(_picked))
            c.ResetVisual();

        foreach (var l in _boardLines) Destroy(l);

        _picked.Clear();
        _boardLines.Clear();
        _waitingForSkyArrow = false;
    }
}
