using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

enum GameState
{
    GAME_INTRO,
    GAME_PLAY,
    GAME_OVER,
}

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

    GameState gameState = GameState.GAME_INTRO;
    float time = 0.0f;
    [SerializeField] GameObject timerTextObject;
    TextMeshProUGUI timerText;
    [SerializeField] GameObject introText;
    [SerializeField] GameObject gameOverText;
    [SerializeField] GameObject timeTextObject;
    TextMeshProUGUI timeText;
    [SerializeField] GameObject thirdPersonCamera;
    [SerializeField] GameObject firstPersonCamera;

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

        timeText = timeTextObject.GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            gameState = GameState.GAME_INTRO;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (Input.GetButtonDown("Toggle View"))
        {
            thirdPersonCamera.SetActive(!thirdPersonCamera.activeSelf);
            firstPersonCamera.SetActive(!firstPersonCamera.activeSelf);
        }
        if (gameState == GameState.GAME_PLAY)
        {
            time += Time.deltaTime;
            UpdateTimer();
        }
        else if (gameState == GameState.GAME_INTRO)
        {
            if (Input.GetButtonDown("Jump"))
            {
                StartGame();
            }
        }
    }

    void FixedUpdate()
    {
        if (gameState == GameState.GAME_PLAY)
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
            //StartCoroutine(DestroyObject(other.gameObject));
            Destroy(other.gameObject);
            objectsDestroyed++;
            UpdateDestroyedCount();

            if (objectsDestroyed == totalDestroyables)
            {
                GameOver();
            }
        }
    }

    IEnumerator DestroyObject(GameObject destroyedObject)
    {
        yield return new WaitForSeconds(2.0f);
        Destroy(destroyedObject);
        objectsDestroyed++;
        UpdateDestroyedCount();

        if (objectsDestroyed == totalDestroyables)
        {
            GameOver();
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

    private void StartGame()
    {
        introText.SetActive(false);
        gameState = GameState.GAME_PLAY;
    }

    private void GameOver()
    {
        timeText.text = "Your Time: " + ((int)time / 60).ToString("d2") + ":" + ((int)time % 60).ToString("d2");
        gameOverText.SetActive(true);
        gameState = GameState.GAME_OVER;
    }
}
