using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour
{
    public Button playGame, instructions, credits, exit;

    private void Start()
    {
        Screen.SetResolution(1920, 1080, FullScreenMode.ExclusiveFullScreen, new RefreshRate() { numerator = 60, denominator = 1 });
        playGame.GetComponent<Button>().onClick.AddListener(() => changeScene(1));
        instructions.onClick.AddListener(() => changeScene(4));
        credits.onClick.AddListener(() => changeScene(3));
        exit.onClick.AddListener(() => changeScene(-1));
    }
    
    private void changeScene(int scene)
    {
        if(scene == -1)
            Application.Quit();
        else
            SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }
}
