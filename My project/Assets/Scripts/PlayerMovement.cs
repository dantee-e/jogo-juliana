using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] WheelCollider frontRight;
    [SerializeField] WheelCollider frontLeft;
    [SerializeField] WheelCollider backRight;
    [SerializeField] WheelCollider backLeft;

    [SerializeField] float acceleration = 500f;
    [SerializeField] float breakingForce = 1000f;
    [SerializeField] float maxTurnAngle = 15f;

    public float alcool_no_sangue = 0.0f;
    public float carMaxSpeed = 100f;
    public float carCurrentSpeed = 0f;

    private Rigidbody rb;

    float currentAcceleration = 0;
    float currentBreakForce = 0;
    float currentTurnAngle = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    IEnumerator updateMovement(float currentAcceleration, float currentBreakForce, float currentTurnAngle)
    {
        yield return new WaitForSeconds(alcool_no_sangue); // alterar para definir o efeito do álcool

        frontRight.motorTorque = currentAcceleration;
        frontLeft.motorTorque = currentAcceleration;
        backRight.motorTorque = currentAcceleration;
        backLeft.motorTorque = currentAcceleration;

        frontRight.brakeTorque = currentBreakForce;
        frontLeft.brakeTorque = currentBreakForce;
        backRight.brakeTorque = currentBreakForce;
        backLeft.brakeTorque = currentBreakForce;

        frontLeft.steerAngle = currentTurnAngle;
        frontRight.steerAngle = currentTurnAngle;
    }

    void FixedUpdate()
    {
        carCurrentSpeed = (rb.velocity.magnitude * 3.6f) / carMaxSpeed; // velocidade atual em relação à velocidade máxima
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (verticalInput > 0)
        {
            currentBreakForce = 0;
            currentAcceleration = acceleration * verticalInput;
        }
        else if (verticalInput < 0)
        {
            currentAcceleration = 0;
            currentBreakForce = -breakingForce * verticalInput;
        }
        else
        {
            currentAcceleration = currentBreakForce = 0;
        }
        currentTurnAngle = maxTurnAngle * horizontalInput;

        StartCoroutine(updateMovement(currentAcceleration, currentBreakForce, currentTurnAngle));
    }
}
