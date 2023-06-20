using UnityEngine;

public class SimplePart : MonoBehaviour
{
    [Header("Dependencies")]
    CartController cartController;

    [SerializeField] float wheelFrequency;
    [SerializeField] float wheelMaxMotorForce;
    [SerializeField] float wheelDampingRatio;

    public GameObject attachedObj;

    public bool attached;
    void OnEnable()
    {
        cartController = FindObjectOfType<CartController>();
    }

    void Start()
    {
        
    }

    public void SetWheelJoint(Rigidbody2D connectedBody)
    {
        //if (partSelection.selectedPart == null) return;
        //if (partButtons.selectedPartType == PartSelection.PartType.WHEEL && partSelection.selectedPart.GetComponent<SimplePart>()) return;

        if(TryGetComponent(out Wheel wheel))
        {
            WheelJoint2D joint = gameObject.AddComponent<WheelJoint2D>();
            wheel.wheelJoint = joint;

            var suspension = joint.suspension;
            suspension.frequency = wheelFrequency;
            suspension.dampingRatio = wheelDampingRatio;
            joint.suspension = suspension;

            joint.breakForce = cartController.wheelBreakForce;
            joint.connectedBody = connectedBody;
            joint.anchor = transform.InverseTransformPoint(transform.position);
            joint.connectedAnchor = connectedBody.transform.InverseTransformPoint(transform.position);

            attachedObj = connectedBody.gameObject;

            attached = true;
        }
    }
}
