using System.Collections.Generic;
using UnityEngine;

public class HitTracker : MonoBehaviour {
    public LineRenderer lineRenderer;
    public GameObject uiPopup; 
    public Transform player; 

    private List<Vector3> hitPositions = new List<Vector3>();
    private HashSet<GameObject> hitSpheres = new HashSet<GameObject>(); // To track unique spheres
    private GameObject[] allSpheres;

    private void Start() {
        allSpheres = GameObject.FindGameObjectsWithTag("Sphere"); // Find all spheres at start
        if (uiPopup != null) {
            uiPopup.SetActive(false); // Hide UI at start
        }
    }

    public void RegisterHit(Vector3 hitPosition, GameObject sphere) {
        if (!hitSpheres.Contains(sphere)) // Only register if the sphere hasn't been hit before
        {
            hitSpheres.Add(sphere);
        }

        hitPositions.Add(hitPosition);
        UpdateLine();

        Debug.Log($"Spheres hit: {hitSpheres.Count}/{allSpheres.Length}");

        // Check if all spheres are hit
        if (hitSpheres.Count >= allSpheres.Length) {
            ShowPopup();
        }
    }


    private void UpdateLine() {
        if (hitPositions.Count < 2) return;

        lineRenderer.positionCount = hitPositions.Count;
        for (int i = 0; i < hitPositions.Count; i++) {
            lineRenderer.SetPosition(i, hitPositions[i]);
        }
    }

    private void ShowPopup() {
        Debug.Log("All spheres hit! Showing UI Popup...");

        if (uiPopup != null) {
            uiPopup.SetActive(true);
        }
    }
}
