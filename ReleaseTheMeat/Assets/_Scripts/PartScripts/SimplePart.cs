using UnityEngine;

public class SimplePart : MonoBehaviour
{
    [Header("Dependencies")]
    PartSelection partSelection;
    CartController cartController;
    PartButtons partButtons;

    public bool attached;

    void Awake()
    {
        partSelection = FindObjectOfType<PartSelection>();
        cartController = FindObjectOfType<CartController>();
        partButtons = FindObjectOfType<PartButtons>();
    }

    public void SetWheelJoint(Rigidbody2D connectedBody)
    {
        if (partSelection.selectedPart == null) return;
        if (partButtons.selectedPartType == PartSelection.PartType.WHEEL && partSelection.selectedPart.GetComponent<SimplePart>()) return;

        WheelJoint2D joint = GetComponent<WheelJoint2D>();

        joint.breakForce = cartController.wheelBreakForce;

        joint.connectedBody = connectedBody;
        joint.anchor = transform.InverseTransformPoint(transform.position);
        joint.connectedAnchor = connectedBody.transform.InverseTransformPoint(transform.position);

        attached = true;
    }
}
