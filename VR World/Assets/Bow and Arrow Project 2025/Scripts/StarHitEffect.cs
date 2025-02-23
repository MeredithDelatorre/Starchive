using UnityEngine;
using System.Collections;

public class StarHitEffect : MonoBehaviour {
    private Renderer sphereRenderer;
    public GameObject newObject; // New object with VFX that will replace sphere 
    private Material sphereMaterial;
    public HitTracker hitTracker; // Reference to the HitTracker script

    private void Start() {
        sphereRenderer = GetComponent<Renderer>();
        if (sphereRenderer != null) {
            sphereMaterial = sphereRenderer.material;
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Arrow")) // Check if the collider has the tag "Arrow"
        {
            if (newObject != null) {
                newObject.SetActive(true); // Activate new object before fading starts
            }

            if (sphereMaterial != null) {
                StartCoroutine(FadeOut());
            }

            // Register this hit in the HitTracker to draw the line
            if (hitTracker != null) {
                hitTracker.RegisterHit(transform.position);
            }
        }
    }

    private IEnumerator FadeOut() {
        for (float alpha = 1f; alpha > 0; alpha -= 0.05f) {
            Color newColor = sphereMaterial.color;
            newColor.a = alpha;
            sphereMaterial.color = newColor;
            yield return new WaitForSeconds(0.05f);
        }
        gameObject.GetComponent<Collider>().enabled = false; // Disable sphere's collider 
    }
}
