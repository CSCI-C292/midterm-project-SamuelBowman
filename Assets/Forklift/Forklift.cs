using System;
using System.Collections;
using UnityEngine;

public class Forklift : MonoBehaviour
{
    [SerializeField] float maxSteeringAngle;
    [SerializeField] float forwardAcceleration;
    [SerializeField] float steeringTorque;
    [SerializeField] float maxForwardVelocity;
    [SerializeField] float maxAngularVelocity;

    [SerializeField] GameObject frontLeftWheelColliderObject;
    [SerializeField] GameObject frontRightWheelColliderObject;
    [SerializeField] GameObject rearLeftWheelColliderObject;
    [SerializeField] GameObject rearRightWheelColliderObject;
    WheelCollider frontLeftWheelCollider;
    WheelCollider frontRightWheelCollider;
    WheelCollider rearLeftWheelCollider;
    WheelCollider rearRightWheelCollider;

    Rigidbody rigidbody;

    [SerializeField] GameObject destroyables;

    LevelManager levelManager;

    void Start()
    {
        levelManager = GameObject.Find("Level Manager").GetComponent<LevelManager>();

        frontLeftWheelCollider = frontLeftWheelColliderObject.GetComponent<WheelCollider>();
        frontRightWheelCollider = frontRightWheelColliderObject.GetComponent<WheelCollider>();
        rearLeftWheelCollider = rearLeftWheelColliderObject.GetComponent<WheelCollider>();
        rearRightWheelCollider = rearRightWheelColliderObject.GetComponent<WheelCollider>();

        rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (levelManager.state == LevelState.PLAY)
        {
            if (Input.GetAxis("Vertical") != 0) {
                if (rigidbody.velocity.magnitude < maxForwardVelocity)
                {
                    rigidbody.AddForce(transform.forward * Input.GetAxis("Vertical") * forwardAcceleration, ForceMode.Acceleration);
                }
                else
                {
                    rigidbody.velocity = transform.forward * Input.GetAxis("Vertical") * maxForwardVelocity;
                }

                if (rigidbody.angularVelocity.magnitude < maxAngularVelocity)
                {
                    rigidbody.AddTorque(transform.up * Input.GetAxis("Horizontal") * steeringTorque, ForceMode.Acceleration);
                }
                else
                {
                    rigidbody.angularVelocity = transform.up * Input.GetAxis("Horizontal") * maxAngularVelocity;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.parent != null && other.gameObject.transform.parent.gameObject == destroyables)
        {
            Destroy(other.gameObject);
            levelManager.DestroyObject();
        }
    }
}
