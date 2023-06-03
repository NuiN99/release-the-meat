using UnityEngine;

public class Rope : MonoBehaviour
{
    public Transform startTransform;
    public Transform endTransform;
    public int segmentCount = 10;
    public float ropeWidth = 0.1f;

    private LineRenderer lineRenderer;
    private Rigidbody2D endRigidbody;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = ropeWidth;
        lineRenderer.endWidth = ropeWidth;

        GenerateRope();
    }

    private void GenerateRope()
    {
        Vector3 startPosition = startTransform.position;
        Vector3 endPosition = endTransform.position;
        Vector3 ropeVector = endPosition - startPosition;
        float segmentLength = ropeVector.magnitude / segmentCount;
        Vector3 segmentDirection = ropeVector.normalized;

        Rigidbody2D previousRigidbody = null;

        for (int i = 0; i <= segmentCount; i++)
        {
            Vector3 segmentPosition = startPosition + segmentDirection * segmentLength * i;

            GameObject segment = new GameObject("RopeSegment_" + i);
            segment.transform.position = segmentPosition;

            Rigidbody2D rigidbody = segment.AddComponent<Rigidbody2D>();
            rigidbody.gravityScale = 0;
            rigidbody.mass = 0.1f;

            HingeJoint2D hingeJoint = segment.AddComponent<HingeJoint2D>();
            hingeJoint.autoConfigureConnectedAnchor = false;

            if (previousRigidbody != null)
            {
                hingeJoint.connectedBody = previousRigidbody;
                hingeJoint.anchor = Vector2.zero;
                hingeJoint.connectedAnchor = -segmentDirection * segmentLength;
            }
            else
            {
                hingeJoint.connectedAnchor = Vector2.zero;
            }

            previousRigidbody = rigidbody;

            if (i == segmentCount)
            {
                hingeJoint.connectedBody = endTransform.GetComponent<Rigidbody2D>();
                hingeJoint.connectedAnchor = endTransform.InverseTransformPoint(segmentPosition);
                endRigidbody = endTransform.GetComponent<Rigidbody2D>();
            }

            if (i > 0)
            {
                LineRenderer segmentLineRenderer = segment.AddComponent<LineRenderer>();
                segmentLineRenderer.startWidth = ropeWidth;
                segmentLineRenderer.endWidth = ropeWidth;
                segmentLineRenderer.positionCount = 2;
            }
        }
    }

    private void Update()
    {
        UpdateLineRendererPositions();
    }

    private void UpdateLineRendererPositions()
    {
        lineRenderer.SetPosition(0, startTransform.position);
        lineRenderer.SetPosition(segmentCount + 1, endTransform.position);

        for (int i = 1; i <= segmentCount; i++)
        {
            Transform segment = transform.GetChild(i - 1);
            LineRenderer segmentLineRenderer = segment.GetComponent<LineRenderer>();
            segmentLineRenderer.SetPosition(0, segment.position);
            segmentLineRenderer.SetPosition(1, startTransform.position + (segment.position - startTransform.position).normalized * (i * (endTransform.position - startTransform.position).magnitude / (segmentCount + 1)));
        }
    }
}