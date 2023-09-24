using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float attackDamage = 10f;

    private PlayerHealth target;

    public void Start()
    {
        target = FindObjectOfType<PlayerHealth>();
    }
    public void AttackHitEvent()
    {
        PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
        }
    }

    public void SetAttackDamage(float attackDamage)
    {
        GetComponentInChildren<EnemyDamageUI>().SetDamage(attackDamage);
        this.attackDamage = attackDamage;
    }
}
