using UnityEngine;

public class RopeVisual : MonoBehaviour
{
    [Header("Rope Visual Settings")]
    [SerializeField] private Material ropeMaterial;       
    [SerializeField] private float ropeWidth = 0.1f;      
    [SerializeField] private int ropeResolution = 10;     
    [SerializeField] private float ropeSlack = 0.1f;       
    
    private ConfigurableJoint joint;
    private LineRenderer lineRenderer;
    private Vector3 anchorPoint;

    private void Start()
    {
        joint = GetComponent<ConfigurableJoint>();
        anchorPoint = joint.connectedBody.transform.position;

        SetupLineRenderer();
    }

    private void SetupLineRenderer()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();

        lineRenderer.material = ropeMaterial ? ropeMaterial : new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startWidth = ropeWidth;
        lineRenderer.endWidth = ropeWidth;

        lineRenderer.positionCount = ropeResolution;

        lineRenderer.sortingOrder = 1;
    }

    private void Update()
    {
        UpdateRopeVisual();
    }

    private void UpdateRopeVisual()
    {
        Vector3 start = anchorPoint;
        Vector3 end = transform.position;

        // Calculate the rope points using a catenary curve
        for (int i = 0; i < ropeResolution; i++)
        {
            float t = i / (float)(ropeResolution - 1);

            // Create a slight curve in the rope using quadratic interpolation
            float height = 1f - (4f * t * (1f - t));

            // Interpolate between start and end points
            Vector3 point = Vector3.Lerp(start, end, t);

            // Add some sag to the rope
            point.y -= height * ropeSlack;

            lineRenderer.SetPosition(i, point);
        }
    }
}
