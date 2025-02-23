using System.Collections.Generic;
using UnityEngine;

public class HitTracker : MonoBehaviour {
    public LineRenderer lineRenderer; // Assign in the Inspector
    private List<Vector3> hitPositions = new List<Vector3>();

    public void RegisterHit(Vector3 hitPosition) {
        hitPositions.Add(hitPosition);
        UpdateLine();
    }

    private void UpdateLine() {
        if (hitPositions.Count < 2) return; // Not enough points to draw a line

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, hitPositions[hitPositions.Count - 2]); // Previous hit
        lineRenderer.SetPosition(1, hitPositions[hitPositions.Count - 1]); // Most recent hit
    }
}
