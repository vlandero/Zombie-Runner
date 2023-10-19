using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float hp = 100f;
    [SerializeField] private float maxHp = 100f;
    [SerializeField] private EnemyHealthBar healthBar;

    private bool initialized = false;

    public void InstantiateStart()
    {
        Init();
        healthBar.HealthUpdate(hp, maxHp);
        healthBar.InstantiateStart();
    }

    public void Init()
    {
        if(initialized) return;
        healthBar = GetComponentInChildren<EnemyHealthBar>();
        initialized = true;
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        healthBar.HealthUpdate(hp, maxHp);
        if(hp <= 0)
        {
            Die();
        }
    }

    public float GetHp()
    {
        return hp;
    }
    public float GetMaxHp()
    {
        return maxHp;
    }

    public void SetMaxHp(float maxHp)
    {
        this.hp = maxHp;
        this.maxHp = maxHp;
    }


    private void Die()
    {
        EnemyAI enemy = GetComponent<EnemyAI>();
        GetComponent<Animator>().SetTrigger("die");
        enemy.Die();
        GameManager.instance.RemoveEnemy(enemy);
        ObjectPooler.instance.Destroy(gameObject, 1.1f);
    }
}
