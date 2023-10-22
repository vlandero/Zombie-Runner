using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public HashSet<EnemyAI> enemies;

    private int score = 0;
    private bool isScoreOnMap = false;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        enemies = new HashSet<EnemyAI>();
    }

    private void Update()
    {
        if (!isScoreOnMap)
        {
            isScoreOnMap = true;
            SpawnerManager.instance.RandomSpawnScore();
        }
    }

    public void SetIsScoreOnMap(bool s)
    {
        isScoreOnMap = s;
    }

    public void SetScore(int s)
    {
        score = s;
        UiManager.instance.scoreUI.SetScore(score);
    }

    public int GetScore()
    {
        return score;
    }

    public void ResetGame()
    {
        score = 0;
    }

    public void AddEnemy(EnemyAI enemy)
    {
        enemies.Add(enemy);
    }

    public void RemoveEnemy(EnemyAI enemy)
    {
        enemies.Remove(enemy);
    }

    public int GetNumberOfEnemies()
    {
        return enemies.Count;
    }
}
