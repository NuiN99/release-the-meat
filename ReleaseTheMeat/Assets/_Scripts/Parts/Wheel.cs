using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    WheelJoint2D wheelJoint;
    [SerializeField] float speed;
    [SerializeField] float maxSpeed;
    [SerializeField] float wheelSpeedDecay;
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
        float xAxis = Input.GetAxis("Horizontal");
        var motor = wheelJoint.motor;
        motor.motorSpeed += xAxis * speed;


        motor.motorSpeed -= (Time.fixedDeltaTime * wheelSpeedDecay) * Mathf.Sign(motor.motorSpeed);

        if (Mathf.Abs(motor.motorSpeed) > maxSpeed)
        {
            motor.motorSpeed = maxSpeed * Mathf.Sign(motor.motorSpeed);
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            wheelJoint.useMotor = true;
        }
        else
        {
            wheelJoint.useMotor = false;
        }


        wheelJoint.motor = motor;
    }
}


    

