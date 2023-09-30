using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private Transform followTarget;
    private float chaseRange;
    private float walkSpeed;
    private float provokedSpeed;
    private float attackRange;
    private float turnSpeed = 5f;

    NavMeshAgent navMeshAgent;
    private float distanceToTarget = Mathf.Infinity;
    private bool lostAggresion = false;
    private bool isProvoked = false;
    private bool isRandomWalking = false;
    private bool isDead = false;
    private bool isRandomWalkingCoroutineActive = false;
    private Animator animator;

    void Start()
    {
        chaseRange = BalanceManager.instance.GetChaseRange();
        walkSpeed = BalanceManager.instance.GetZombieWalkSpeed();
        provokedSpeed = BalanceManager.instance.GetZombieChaseSpeed();
        attackRange = BalanceManager.instance.GetZombieAttackRange();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = walkSpeed;
        navMeshAgent.stoppingDistance = attackRange;
        animator = GetComponent<Animator>();
        followTarget = PlayerManager.instance.playerObject.transform;
        StartCoroutine(StartRandomWalk());
    }

    void Update()
    {
        distanceToTarget = Vector3.Distance(transform.position, followTarget.position);
        if (isProvoked)
        {
            EngageTarget();
        }
        else if (distanceToTarget <= chaseRange)
        {
            Provoke();
        }
        else if (isRandomWalking)
        {
            StartRandomWalkingCoroutine();
        }
    }

    public void EngageTarget()
    {
        isRandomWalking = false;
        FaceTarget();
        if (distanceToTarget >= navMeshAgent.stoppingDistance)
        {
            ChaseTarget();
        }
        else
        {
            AttackTarget();
        }
    }

    public void Provoke()
    {
        if (lostAggresion) return;
        navMeshAgent.speed = provokedSpeed;
        isProvoked = true;
    }

    public void Die()
    {
        isDead = true;
    }


    private void FaceTarget()
    {
        Vector3 direction = (followTarget.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }

    private void ChaseTarget()
    {
        animator.SetBool("attack", false);
        animator.SetTrigger("move");
        if(navMeshAgent.enabled)
            navMeshAgent.SetDestination(followTarget.position);
    }

    private void AttackTarget()
    {
        animator.SetBool("attack", true);
    }

    private void ResetNavMeshDestination()
    {
        if (!isDead)
        {
            navMeshAgent.SetDestination(transform.position);
        }
    }

    public void SetChaseRange(float range)
    {
        chaseRange = range;
    }

    public void SetAttackRange(float range)
    {
        navMeshAgent.stoppingDistance = range;
    }

    private IEnumerator StartRandomWalk()
    {
        isRandomWalkingCoroutineActive = true;

        while (!isDead && !isProvoked)
        {
            yield return new WaitForSecondsRealtime(Random.Range(3f, 7f));
            if (!isDead)
            {
                animator.SetBool("walking", true);
                Vector3 randomDestination = SpawnerManager.instance.GetRandomPoint();
                navMeshAgent.SetDestination(randomDestination);
                yield return new WaitUntil(() => ((navMeshAgent.hasPath && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)) || isDead || isProvoked);
                ResetNavMeshDestination();
                animator.SetBool("walking", false);
            }
        }
        isRandomWalkingCoroutineActive = false;
    }

    private void StartRandomWalkingCoroutine()
    {
        if (!isRandomWalkingCoroutineActive && !isProvoked && !isDead)
            StartCoroutine(StartRandomWalk());
    }

    public void LoseAggresion(float t)
    {
        bool isWalking = animator.GetBool("walking");
        lostAggresion = true;
        StartCoroutine(RegainAggresion(t));
        if (!isWalking)
        {
            isProvoked = false;
            navMeshAgent.speed = walkSpeed;
            animator.ResetTrigger("move");
            animator.SetTrigger("idle");
            animator.SetBool("walking", false);
            ResetNavMeshDestination();
            StartRandomWalkingCoroutine();
        }
        
    }

    private IEnumerator RegainAggresion(float t)
    {
        yield return new WaitForSecondsRealtime(t);
        lostAggresion = false;
    }

    public void RegainInstantAggresion()
    {
        lostAggresion = false;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }

}
