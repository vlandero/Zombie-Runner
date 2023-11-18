using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Proyecto26;
using System;
using Unity.VisualScripting;
using TMPro;

public class LeaderboardResponse
{
    public UserDTO[] users;
}

public class Leaderboard : MonoBehaviour
{
    public GameObject cellPrefab;
    public ScrollRect scrollRect;

    private void SetCellText(GameObject cell, int place, string name, int score)
    {
        GameObject number = cell.transform.Find("Number").gameObject;
        GameObject username = cell.transform.Find("Name").gameObject;
        GameObject scoreObject = cell.transform.Find("Score").gameObject;
        number.GetComponent<TextMeshProUGUI>().text = place.ToString() + ".";
        username.GetComponent<TextMeshProUGUI>().text = name;
        scoreObject.GetComponent<TextMeshProUGUI>().text = score.ToString();
    }

    private void Start()
    {
        RestClient.Get(Secrets.lambdaUrl + "/get-leaderboard").Then(response =>
        {
            HttpRes httpRes = JsonUtility.FromJson<HttpRes>(response.Text);
            if (httpRes.error)
            {
                throw new Exception(httpRes.payload);
            }
            LeaderboardResponse users = JsonUtility.FromJson<LeaderboardResponse>("{\"users\":" + httpRes.payload + "}");
            for (int i = 0; i < users.users.Length; i++)
            {
                var user = users.users[i];
                GameObject cell = Instantiate(cellPrefab, transform);
                SetCellText(cell, i + 1, user.username, user.highscore);
                cell.transform.SetParent(transform, false);
            }
        }).Catch(err =>
        {
            Debug.Log(err.Message);
        });
    }
}
