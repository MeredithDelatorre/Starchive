using System.Collections.Generic;
using UnityEngine;

public class HitTracker : MonoBehaviour {
    public LineRenderer lineRenderer;
    private List<Vector3> hitPositions = new List<Vector3>();

    public void RegisterHit(Vector3 hitPosition) {
        hitPositions.Add(hitPosition);
        Debug.Log("Registered hit at: " + hitPosition);
        UpdateLine();
    }


    private void UpdateLine() {
        if (hitPositions.Count < 2) return;

        // Increase the number of positions instead of limiting to 2
        lineRenderer.positionCount = hitPositions.Count;

        // Assign all hit positions to the LineRenderer
        for (int i = 0; i < hitPositions.Count; i++) {
            lineRenderer.SetPosition(i, hitPositions[i]);
        }

        Debug.Log($"Total line points: {lineRenderer.positionCount}");
    }

}
