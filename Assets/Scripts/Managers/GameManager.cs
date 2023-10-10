using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public HashSet<EnemyAI> enemies;
    public int score = 0;
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
