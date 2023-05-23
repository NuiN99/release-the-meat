using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    WheelJoint2D wheelJoint;
    [SerializeField] float speed;
    void Start()
    {
        wheelJoint= GetComponent<WheelJoint2D>();
    }

    void FixedUpdate()
    {
        MoveWheel();
    }

    void MoveWheel()
    {
        float xAxis = Input.GetAxis("Horizontal");
        var motor = wheelJoint.motor;
        motor.motorSpeed = xAxis * speed;

        wheelJoint.motor = motor;

        
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            wheelJoint.useMotor = true;
        }
        else
        {
            wheelJoint.useMotor = false;
        }
    }
}
