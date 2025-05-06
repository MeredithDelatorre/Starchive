using UnityEngine;
using System.Collections;

public class ConstellationProjectile : MonoBehaviour {
    [SerializeField] private float skyHeight = 25f;      // Y position at which to “stick to sky”
    [SerializeField] private float fadeTime = 1.5f;      // seconds to fade the arrow away

    private ArrowLauncher launcher;
    private Transform constellation;                     // assigns itself in Start

    private void Start() {
        launcher = GetComponent<ArrowLauncher>();
        constellation = GetComponentInChildren<LineRenderer>()?.transform.parent; // root we built
        if (constellation == null) enabled = false;       // arrow with no constellation: do nothing
    }

    private void Update() {
        if (transform.position.y >= skyHeight) {
            // 1) freeze the constellation in world space
            constellation.SetParent(null, true);

            // 2) nudge it so it faces the player’s forward when finalised 
            constellation.rotation = Quaternion.LookRotation(Camera.main.transform.forward);

            // 3) fade arrow trail / mesh, then destroy arrow
            StartCoroutine(FadeAndDestroy());
            enabled = false;
        }
    }

    private IEnumerator FadeAndDestroy() {
        float t = 0;
        Renderer[] rends = GetComponentsInChildren<Renderer>();
        Color[] start = new Color[rends.Length];
        for (int i = 0; i < rends.Length; i++) start[i] = rends[i].material.color;

        while (t < 1) {
            t += Time.deltaTime / fadeTime;
            for (int i = 0; i < rends.Length; i++) {
                var c = start[i]; c.a = Mathf.Lerp(start[i].a, 0, t);
                rends[i].material.color = c;
            }
            yield return null;
        }
        Destroy(gameObject);
    }
}
