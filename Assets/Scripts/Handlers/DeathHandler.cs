using Proyecto26;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class UpdateScoreDTO
{
    public string username; public int score;
    public UpdateScoreDTO(string username, int score)
    {
        this.username = username;
        this.score = score;
    }
}

public class DeathHandler : MonoBehaviour
{
    [SerializeField] private Canvas gameOverCanvas;
    void Start()
    {
        gameOverCanvas.enabled = false;
    }

    public void UpdateHighscore()
    {
        int score = GameManager.instance.GetScore();
        string username = PlayerPrefs.GetString("username");
        RestClient.Post(Secrets.lambdaUrl + "/update-highscore", new UpdateScoreDTO(username, score)).Then(response =>
        {
            HttpRes res = JsonUtility.FromJson<HttpRes>(response.Text);
            if (res.error)
            {
                throw new Exception(res.payload);
            }
            UserDTO user = JsonUtility.FromJson<UserDTO>(res.payload);
            Debug.Log(user);
        }).Catch(error =>
        {
            Debug.Log(error.Message);
        });
    }

    public void HandleDeath()
    {
        gameOverCanvas.enabled = true;
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        UpdateHighscore();
    }
}
