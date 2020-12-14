using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum LevelState
{
    START,
    PLAY,
    PAUSE,
    FINSIH,
}

public class LevelManager : MonoBehaviour
{
    [HideInInspector] public LevelState state = LevelState.START;
    public RuntimeData runtimeData;
    int objectsDestroyed = 0;
    int totalDestroyables;
    float time = 0.0f;
    [SerializeField] string nextLevelName;

    [SerializeField] GameObject destroyables;

    [SerializeField] GameObject levelCanvas;
    TextMeshProUGUI destroyedCountText;
    TextMeshProUGUI timerText;

    GameObject introMenu;
    TextMeshProUGUI levelText;

    GameObject pausedMenu;
    Slider musicVolumeSlider;
    Slider sfxVolumeSlider;

    GameObject levelOverMenu;
    TextMeshProUGUI timeText;

    GameObject gameOverMenu;

    [SerializeField] GameObject forklift;
    GameObject thirdPersonCamera;
    GameObject firstPersonCamera;
    AudioSource musicAudioSource;

    [SerializeField] AudioClip backgroundMusic;
    [SerializeField] AudioClip winMusic;
    [SerializeField] AudioClip loseMusic;

    void Start()
    {
        destroyedCountText = levelCanvas.transform.Find("Destroyed Count Text").GetComponent<TextMeshProUGUI>();
        timerText = levelCanvas.transform.Find("Timer Text").GetComponent<TextMeshProUGUI>();

        introMenu = levelCanvas.transform.Find("Intro Menu").gameObject;
        levelText = introMenu.transform.Find("Level Text").GetComponent<TextMeshProUGUI>();

        pausedMenu = levelCanvas.transform.Find("Paused Menu").gameObject;
        musicVolumeSlider = pausedMenu.transform.Find("Music Volume Slider").GetComponent<Slider>();
        sfxVolumeSlider = pausedMenu.transform.Find("SFX Volume Slider").GetComponent<Slider>();

        levelOverMenu = levelCanvas.transform.Find("Level Over Menu").gameObject;
        timeText = levelOverMenu.transform.Find("Time Text").GetComponent<TextMeshProUGUI>();

        gameOverMenu = levelCanvas.transform.Find("Game Over Menu").gameObject;

        thirdPersonCamera = forklift.transform.Find("Third Person Camera").gameObject;
        firstPersonCamera = forklift.transform.Find("First Person Camera").gameObject;
        musicAudioSource = forklift.GetComponent<AudioSource>();

        totalDestroyables = destroyables.transform.childCount;
        UpdateDestroyedCount();
        UpdateTimer();
        levelText.text = SceneManager.GetActiveScene().name;
        musicAudioSource.velocityUpdateMode = AudioVelocityUpdateMode.Dynamic;
        musicAudioSource.volume = runtimeData.musicVolume;
    }

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
            if (Input.GetButtonDown("Cancel"))
            {
                PauseLevel();
            }
        }
        else if (state == LevelState.PAUSE)
        {
            if (Input.GetButtonDown("Cancel"))
            {
                ResumeLevel();
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
        introMenu.SetActive(false);
        musicAudioSource.clip = backgroundMusic;
        musicAudioSource.Play();
    }

    void PauseLevel()
    {
        state = LevelState.PAUSE;
        pausedMenu.SetActive(true);
        musicVolumeSlider.normalizedValue = runtimeData.musicVolume;
        sfxVolumeSlider.normalizedValue = runtimeData.sfxVolume;
        Time.timeScale = 0;
    }

    public void ResumeLevel()
    {
        state = LevelState.PLAY;
        pausedMenu.SetActive(false);
        Time.timeScale = 1;
    }

    void FinishLevel()
    {
        state = LevelState.FINSIH;
        musicAudioSource.clip = winMusic;
        musicAudioSource.loop = false;
        musicAudioSource.Play();
        levelOverMenu.SetActive(true);
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
            levelOverMenu.SetActive(false);
            gameOverMenu.SetActive(true);
        }
    }

    public void QuitLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main Menu");
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

    public void MusicVolumeChanged()
    {
        runtimeData.musicVolume = musicVolumeSlider.normalizedValue;
        musicAudioSource.volume = runtimeData.musicVolume;
    }

    public void SfxVolumeChanged()
    {
        runtimeData.sfxVolume = sfxVolumeSlider.normalizedValue;
    }
}
