using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSound : MonoBehaviour
{
    public float minSpeed, maxSpeed;
    public float minPitch, maxPitch;
    Rigidbody rb;
    AudioSource motor;
    public float pitchDoCarro;
    float currentSpeed;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        motor = GetComponent<AudioSource>();
    }
    private void Update()
    {
        MotorSound();
    }
    void MotorSound()
    {
        currentSpeed = rb.velocity.magnitude;
        pitchDoCarro = rb.velocity.magnitude / 50f;
        if(currentSpeed < minSpeed)
        {
            motor.pitch = minPitch;
        }
        if(currentSpeed > minSpeed && currentSpeed < maxSpeed)
        {
            motor.pitch = minPitch + pitchDoCarro;
        }
        if(currentSpeed > maxSpeed)
        {
            motor.pitch = maxPitch;
        }
    }
}
