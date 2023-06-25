using System;
using UnityEngine;

public class SimplePart : MonoBehaviour
{
    
    [Header("Dependencies")]
    CartController cartController;


    public bool attached;
    [NonSerialized] public GameObject attachedObj;

    [Header("Wheel Properties")]
    [SerializeField] float wheelFrequency;
    [SerializeField] float wheelDampingRatio;
    public float wheelMaxMotorForce;
    public float wheelAcceleration;
    public float wheelMaxSpeed;
    
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

            var motor = joint.motor;
            motor.maxMotorTorque = wheelMaxMotorForce;
            joint.motor = motor;

            joint.breakForce = cartController.wheelBreakForce;
            joint.connectedBody = connectedBody;
            joint.anchor = transform.InverseTransformPoint(transform.position);
            joint.connectedAnchor = connectedBody.transform.InverseTransformPoint(transform.position);

            attachedObj = connectedBody.gameObject;

            attached = true;
        }
    }
}
