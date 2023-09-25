using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float hp = 100f;
    [SerializeField] private float maxHp = 100f;
    [SerializeField] private EnemyHealthBar healthBar;

    private void Start()
    {
        healthBar = GetComponentInChildren<EnemyHealthBar>();
        healthBar.HealthUpdate(hp, maxHp);
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
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<Animator>().SetTrigger("die");
        GetComponent<EnemyAI>().Die();
        Destroy(gameObject, 1.1f);
    }
}
