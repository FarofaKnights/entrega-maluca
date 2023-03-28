using UnityEngine;

public class Controlador : MonoBehaviour
{
    public WheelCollider frontLeftWheel;
    public WheelCollider frontRightWheel;
    public WheelCollider rearLeftWheel;
    public WheelCollider rearRightWheel;

    public float maxTorque = 200f;
    public float maxSteerAngle = 30f;
    public float brakeTorque = 1000f;

    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        ApplySteering(horizontalInput);
        ApplyAcceleration(verticalInput);
        ApplyBraking();
    }

    private void ApplySteering(float steer)
    {
        float steerAngle = maxSteerAngle * steer;
        frontLeftWheel.steerAngle = steerAngle;
        frontRightWheel.steerAngle = steerAngle;
    }

    private void ApplyAcceleration(float throttle)
    {
        float torque = maxTorque * throttle;

        rearLeftWheel.motorTorque = torque;
        rearRightWheel.motorTorque = torque;
    }

    private void ApplyBraking()
    {
        bool isBraking = Input.GetKey(KeyCode.Space);

        if (isBraking)
        {
            rearLeftWheel.brakeTorque = brakeTorque;
            rearRightWheel.brakeTorque = brakeTorque;
        }
        else
        {
            rearLeftWheel.brakeTorque = 0;
            rearRightWheel.brakeTorque = 0;
        }
    }
}
