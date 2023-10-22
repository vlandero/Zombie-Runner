using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float attackDamage = 10f;

    public void AttackHitEvent()
    {
        PlayerManager.instance.TakeDamage(attackDamage);
        GetComponentInChildren<EnemyAI>().PlayAttackAudio();
    }

    public void SetAttackDamage(float attackDamage)
    {
        GetComponentInChildren<EnemyDamageUI>().SetDamage(attackDamage);
        this.attackDamage = attackDamage;
    }

    public float GetAttackDamage()
    {
        return attackDamage;
    }
}
