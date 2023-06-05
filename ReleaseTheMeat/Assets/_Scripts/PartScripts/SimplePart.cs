using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePart : MonoBehaviour
{

    void Start()
    {
        
    }

    void Update()
    {
        
    }


    public void SetWheelJoint(Rigidbody2D connectedBody)
    {
        if (PartSelection.instance.selectedPart == null) return;
        if (PartButtons.instance.selectedPartType == PartSelection.PartType.WHEEL && PartSelection.instance.selectedPart.GetComponent<SimplePart>()) return;

        WheelJoint2D joint = GetComponent<WheelJoint2D>();

        joint.breakForce = CartController.instance.wheelBreakForce;

        joint.connectedBody = connectedBody;
        joint.anchor = transform.InverseTransformPoint(transform.position);
        joint.connectedAnchor = connectedBody.transform.InverseTransformPoint(transform.position);
    }
}
