using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScreen : MonoBehaviour
{
    public void OnClickPlay()
    {
        SceneManager.LoadScene(2);
    }

    public void OnClickLeaderboard()
    {
        PlayerPrefs.SetInt("prevSceneIndex", SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(3);
    }

    public void OnClickLogout()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(0);
    }
}
