using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject creditsTextObject;

    public void StartButtonPressed()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void CreditsButtonPressed()
    {
        creditsTextObject.SetActive(true);
    }

    public void QuitButtonPressed()
    {
        Application.Quit();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            creditsTextObject.SetActive(false);
        }
    }
}
