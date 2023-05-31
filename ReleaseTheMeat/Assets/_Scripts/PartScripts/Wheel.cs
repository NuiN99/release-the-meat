using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    WheelJoint2D wheelJoint;
    [SerializeField] float acceleration;
    [SerializeField] float maxSpeed;
    void Start()
    {
        wheelJoint = GetComponent<WheelJoint2D>();
    }

    void FixedUpdate()
    {
        MoveWheel();
    }

    void MoveWheel()
    {
        if (wheelJoint == null) return;

        float xAxis = Input.GetAxisRaw("Horizontal");

        var motor = wheelJoint.motor;
        motor.motorSpeed += xAxis * acceleration;

        if(xAxis == 0)
            motor.motorSpeed = -GetComponent<Rigidbody2D>().angularVelocity * Mathf.Sign(motor.motorSpeed);

        if (Mathf.Abs(motor.motorSpeed) > maxSpeed)
            motor.motorSpeed = maxSpeed * Mathf.Sign(motor.motorSpeed);

        wheelJoint.motor = motor;

        if (xAxis == 0)
            wheelJoint.useMotor = false;
    }
}


    

