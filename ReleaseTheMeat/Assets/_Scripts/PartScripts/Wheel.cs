using UnityEngine;

public class Wheel : MonoBehaviour
{
    public WheelJoint2D wheelJoint;


    Rigidbody2D rb;
    SimplePart simplePart;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        simplePart = GetComponent<SimplePart>();
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
        motor.motorSpeed += xAxis * simplePart.wheelAcceleration;

        if(xAxis == 0)
            motor.motorSpeed = -rb.angularVelocity * Mathf.Sign(motor.motorSpeed);

        if (Mathf.Abs(motor.motorSpeed) > simplePart.wheelMaxSpeed)
            motor.motorSpeed = simplePart.wheelMaxSpeed * Mathf.Sign(motor.motorSpeed);

        wheelJoint.motor = motor;

        if (xAxis == 0)
            wheelJoint.useMotor = false;
    }
}


    

