using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    int objectsDestroyed = 0;
    int totalDestroyables;
    [SerializeField] GameObject destroyedCountTextObject;
    TextMeshProUGUI destroyedCountText;

    Boolean gameRunning = true;
    float time = 0.0f;
    [SerializeField] GameObject timerTextObject;
    TextMeshProUGUI timerText;

    void Start()
    {
        frontLeftWheelCollider = frontLeftWheelColliderObject.GetComponent<WheelCollider>();
        frontRightWheelCollider = frontRightWheelColliderObject.GetComponent<WheelCollider>();
        rearLeftWheelCollider = rearLeftWheelColliderObject.GetComponent<WheelCollider>();
        rearRightWheelCollider = rearRightWheelColliderObject.GetComponent<WheelCollider>();

        rigidbody = GetComponent<Rigidbody>();

        totalDestroyables = destroyables.transform.childCount;
        destroyedCountText = destroyedCountTextObject.GetComponent<TextMeshProUGUI>();
        UpdateDestroyedCount();

        timerText = timerTextObject.GetComponent<TextMeshProUGUI>();
        UpdateTimer();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (gameRunning)
        {
            time += Time.deltaTime;
            UpdateTimer();
        }
    }

    void FixedUpdate()
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.parent.gameObject == destroyables)
        {
            Destroy(other.gameObject);
            objectsDestroyed++;
            UpdateDestroyedCount();

            if (objectsDestroyed == totalDestroyables)
            {
                gameRunning = false;
            }
        }
    }

    private void UpdateDestroyedCount()
    {
        destroyedCountText.text = "Destroyed: " + objectsDestroyed + "/" + totalDestroyables;
    }

    private void UpdateTimer()
    {
        timerText.text = "Time: " + ((int) time / 60).ToString("d2") + ":" + ((int) time % 60).ToString("d2");
    }
}
