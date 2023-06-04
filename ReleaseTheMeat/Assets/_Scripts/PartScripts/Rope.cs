using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.iOS;
using static UnityEngine.Rendering.HableCurve;

public class Rope : MonoBehaviour
{
    [SerializeField] float segmentDistance;
    [SerializeField] float length;

    [SerializeField] GameObject segmentPrefab;

    List<GameObject> segments = new List<GameObject>();
    void Start()
    {
        GenerateRope();
    }

    public void GenerateRope()
    {
        for (float i = 0; i < length; i += segmentDistance)
        {
            GameObject segment = Instantiate(segmentPrefab);
            segments.Add(segment);
        }

        for(int i = 0; i < segments.Count; i++)
        {
            segments[i].transform.parent = gameObject.transform;
            segments[i].name = $"RopeSegment{i}";

            if (i == 0) 
            {
                segments[i].GetComponent<HingeJoint2D>().enabled = false;
                continue;
            }

            segments[i].transform.position = segments[i - 1].transform.position + (Vector3.right * segmentDistance);
            
            HingeJoint2D joint = segments[i].GetComponent<HingeJoint2D>();

            joint.connectedBody = segments[i - 1].GetComponent<Rigidbody2D>();
            joint.connectedAnchor = joint.connectedBody.transform.InverseTransformPoint(segments[i].transform.position);

        }
    }

    void Update()
    {
        
    }

}