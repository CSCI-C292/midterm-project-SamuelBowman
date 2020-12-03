using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum LevelState
{
    START,
    PLAY,
    FINSIH,
}

public class LevelManager : MonoBehaviour
{
    [HideInInspector] public LevelState state = LevelState.START;
    int objectsDestroyed = 0;
    int totalDestroyables;
    float time = 0.0f;
    [SerializeField] string nextLevelName;

    [SerializeField] GameObject destroyables;
    [SerializeField] GameObject destroyedCountTextObject;
    TextMeshProUGUI destroyedCountText;
    [SerializeField] GameObject timerTextObject;
    TextMeshProUGUI timerText;
    [SerializeField] GameObject introText;
    [SerializeField] GameObject levelTextObject;
    TextMeshProUGUI levelText;
    [SerializeField] GameObject gameOverText;
    [SerializeField] GameObject timeTextObject;
    TextMeshProUGUI timeText;
    [SerializeField] GameObject thirdPersonCamera;
    [SerializeField] GameObject firstPersonCamera;

    // Start is called before the first frame update
    void Start()
    {
        totalDestroyables = destroyables.transform.childCount;
        destroyedCountText = destroyedCountTextObject.GetComponent<TextMeshProUGUI>();
        UpdateDestroyedCount();

        timerText = timerTextObject.GetComponent<TextMeshProUGUI>();
        UpdateTimer();

        levelText = levelTextObject.GetComponent<TextMeshProUGUI>();
        levelText.text = SceneManager.GetActiveScene().name;

        timeText = timeTextObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == LevelState.START)
        {
            if (Input.GetButtonDown("Jump"))
            {
                StartLevel();
            }
        }
        else if (state == LevelState.PLAY)
        {
            
            time += Time.deltaTime;
            UpdateTimer();
            if (Input.GetButtonDown("Toggle View"))
            {
                thirdPersonCamera.SetActive(!thirdPersonCamera.activeSelf);
                firstPersonCamera.SetActive(!firstPersonCamera.activeSelf);
            }
            if (Input.GetButtonDown("Submit"))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        else if (state == LevelState.FINSIH)
        {
            if (Input.GetButtonDown("Jump"))
            {
                NextLevel();
            }
        }
    }

    void StartLevel()
    {
        state = LevelState.PLAY;
        introText.SetActive(false);
    }

    void FinishLevel()
    {
        state = LevelState.FINSIH;
        gameOverText.SetActive(true);
        timeText.text = "Your Time: " + ((int)time / 60).ToString("d2") + ":" + ((int)time % 60).ToString("d2");
    }

    void NextLevel()
    {
        if (nextLevelName != "")
        {
            SceneManager.LoadScene(nextLevelName);
        }
        else
        {
            SceneManager.LoadScene("Main Menu");
        }
    }

    public void DestroyObject()
    {
        objectsDestroyed++;
        UpdateDestroyedCount();

        if (objectsDestroyed == totalDestroyables)
        {
            FinishLevel();
        }
    }

    void UpdateDestroyedCount()
    {
        destroyedCountText.text = "Destroyed: " + objectsDestroyed + "/" + totalDestroyables;
    }

    void UpdateTimer()
    {
        timerText.text = "Time: " + ((int) time / 60).ToString("d2") + ":" + ((int) time % 60).ToString("d2");
    }
}
