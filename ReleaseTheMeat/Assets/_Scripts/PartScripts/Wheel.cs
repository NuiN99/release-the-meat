using UnityEngine;

public class Wheel : MonoBehaviour
{
    public WheelJoint2D wheelJoint;
    [SerializeField] float acceleration;
    [SerializeField] float maxSpeed;

    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
            motor.motorSpeed = -rb.angularVelocity * Mathf.Sign(motor.motorSpeed);

        if (Mathf.Abs(motor.motorSpeed) > maxSpeed)
            motor.motorSpeed = maxSpeed * Mathf.Sign(motor.motorSpeed);

        wheelJoint.motor = motor;

        if (xAxis == 0)
            wheelJoint.useMotor = false;
    }
}


    

