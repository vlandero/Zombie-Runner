using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float attackDamage = 10f;

    public void AttackHitEvent()
    {
        PlayerManager.instance.TakeDamage(attackDamage);
    }

    public void SetAttackDamage(float attackDamage)
    {
        GetComponentInChildren<EnemyDamageUI>().SetDamage(attackDamage);
        this.attackDamage = attackDamage;
    }
}
