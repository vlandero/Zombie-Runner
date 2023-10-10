using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private int sceneNumber = 0;
    public void ReloadGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneNumber);
        UiManager.instance.timeElapsedHandler.ResetTimer();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
