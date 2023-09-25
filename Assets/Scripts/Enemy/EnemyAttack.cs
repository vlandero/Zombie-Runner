using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float attackDamage = 10f;

    private GameObject target;

    public void Start()
    {
        target = PlayerManager.instance.playerObject;
    }
    public void AttackHitEvent()
    {
        if (target.TryGetComponent<PlayerHealth>(out var playerHealth))
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
