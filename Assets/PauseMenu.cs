using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public bool paused = false;

    public GameObject camera;

    private GameObject playerLifeBarUI;
    private GameObject weaponUI;
    private GameObject pauseMenuUI;

    public Button ResumeButton;
    public Button MenuButton;

    void Start()
    {
        weaponUI = camera.transform.GetChild(0).gameObject as GameObject;
        pauseMenuUI = gameObject.transform.GetChild(0).gameObject as GameObject;

        MenuButton.GetComponent<Button>().onClick.AddListener(() => Resume(true));
        ResumeButton.onClick.AddListener(() => Resume(false));
    }

    void Update()
    {
        if(camera.transform.childCount == 2)
        {
            playerLifeBarUI = camera.transform.GetChild(1).gameObject as GameObject;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
                Resume(false);
            else
                Pause();
        }
    }

    private void Resume(bool MainMenu)
    {
        pauseMenuUI.SetActive(false);
        weaponUI.SetActive(true);
        playerLifeBarUI.SetActive(true);
        Time.timeScale = 1f;
        paused = false;
        if(MainMenu)
            SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        weaponUI.SetActive(false);
        playerLifeBarUI.SetActive(false);
        Time.timeScale = 0f;
        paused = true;
    }
}
