using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum MaterialType
{
    Grass,
    Wood
}

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
    private bool isDead = false;
    private bool isRandomWalkingCoroutineActive = false;
    private Animator animator;

    private float timeElapsedSinceLastCheck = 0f;

    [SerializeField] private AudioSource walkOnGrassAudio;
    [SerializeField] private AudioSource walkOnWoodAudio;
    [SerializeField] private AudioSource runOnGrassAudio;
    [SerializeField] private AudioSource runOnWoodAudio;
    [SerializeField] private AudioSource dieAudio;
    [SerializeField] private AudioSource attackAudio;

    private MaterialType currentMaterial = MaterialType.Grass;
    private bool currentIsRunning = false;
    private AudioSource currentAudioSource = null;
    private bool initialized = false;

    void Start()
    {
        Init();
    }

    public void Init()
    {
        if(initialized) return;
        chaseRange = BalanceManager.instance.GetChaseRange();
        walkSpeed = BalanceManager.instance.GetZombieWalkSpeed();
        provokedSpeed = BalanceManager.instance.GetZombieChaseSpeed();
        attackRange = BalanceManager.instance.GetZombieAttackRange();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = walkSpeed;
        navMeshAgent.stoppingDistance = attackRange;
        animator = GetComponent<Animator>();
        followTarget = PlayerManager.instance.playerObject.transform;
        walkOnGrassAudio.maxDistance = BalanceManager.instance.zombieSoundRange;
        walkOnWoodAudio.maxDistance = BalanceManager.instance.zombieSoundRange;
        runOnGrassAudio.maxDistance = BalanceManager.instance.zombieSoundRange;
        runOnWoodAudio.maxDistance = BalanceManager.instance.zombieSoundRange;
        dieAudio.maxDistance = BalanceManager.instance.zombieSoundRange;
        attackAudio.maxDistance = BalanceManager.instance.zombieSoundRange;
        initialized = true;
    }

    public void InstantiateStart()
    {
        Init();
        navMeshAgent.enabled = true;
        navMeshAgent.speed = walkSpeed;
        navMeshAgent.stoppingDistance = attackRange;
        distanceToTarget = Mathf.Infinity;
        lostAggresion = false;
        isProvoked = false;
        isDead = false;
        isRandomWalkingCoroutineActive = false;
        currentAudioSource = null;
        currentIsRunning = false;
        currentMaterial = MaterialType.Grass;
        timeElapsedSinceLastCheck = 0f;
        GetComponent<Collider>().enabled = true;
        animator.ResetTrigger("die");
        animator.ResetTrigger("move");
        animator.ResetTrigger("idle");
        animator.SetBool("walking", false);
        StartCoroutine(StartRandomWalk());
    }

    void Update()
    {
        distanceToTarget = Vector3.Distance(transform.position, followTarget.position);
        CheckMaterialUnderFeet();
        if (isProvoked)
        {
            EngageTarget();
        }
        else if (distanceToTarget <= chaseRange)
        {
            Provoke();
        }
        else if (!isProvoked)
        {
            StartRandomWalkingCoroutine();
        }

        if(timeElapsedSinceLastCheck > BalanceManager.instance.timeToLoseAggression)
        {
            if(isProvoked && distanceToTarget > chaseRange)
            {
                float randomNumber = Random.Range(0f, 1f);
                if(randomNumber < BalanceManager.instance.loseAggressionProbability / 100)
                {
                    LoseAggresion(3f);
                }
            }
            timeElapsedSinceLastCheck = 0f;
        }
        timeElapsedSinceLastCheck += Time.deltaTime;
    }

    private void CheckMaterialUnderFeet()
    {
        bool isWalking = animator.GetBool("walking");
        if((!isWalking && !isProvoked) || isDead)
        {
            if (currentAudioSource != null)
            {
                currentAudioSource.Stop();
            }
            return;
        }
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1.0f))
        {
            if (hit.collider.CompareTag("Wood"))
            {
                ChangeMaterial(MaterialType.Wood, isProvoked);
            }
            else if (hit.collider.CompareTag("Terrain"))
            {
                ChangeMaterial(MaterialType.Grass, isProvoked);
            }
        }
    }

    private void ChangeSound(MaterialType newMaterial, bool isRunning)
    {
        if (currentAudioSource != null)
        {
            currentAudioSource.Stop();
        }

        currentMaterial = newMaterial;
        currentIsRunning = isRunning;

        if (isRunning)
        {
            if (currentMaterial == MaterialType.Wood)
            {
                currentAudioSource = runOnWoodAudio;
            }
            else if (currentMaterial == MaterialType.Grass)
            {
                currentAudioSource = runOnGrassAudio;
            }
        }
        else
        {
            if (currentMaterial == MaterialType.Wood)
            {
                currentAudioSource = walkOnWoodAudio;
            }
            else if (currentMaterial == MaterialType.Grass)
            {
                currentAudioSource = walkOnGrassAudio;
            }
        }

        if (currentAudioSource != null)
        {
            currentAudioSource.Play();
        }
    }

    private void ChangeMaterial(MaterialType newMaterial, bool isRunning)
    {
        if (newMaterial != currentMaterial || isRunning != currentIsRunning || (currentAudioSource != null && !currentAudioSource.isPlaying))
        {
            ChangeSound(newMaterial, isRunning);
        }
    }


    public void EngageTarget()
    {
        UiManager.instance.immuneUI.SetImmune(false);
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

    public bool IsProvoked()
    {
        return isProvoked;
    }

    public void Provoke()
    {
        if (lostAggresion || isProvoked) return;
        UiManager.instance.engagedZombiesUI.EngagedZombiesUpdate(++PlayerManager.instance.engagedZombies);
        navMeshAgent.speed = provokedSpeed;
        isProvoked = true;
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

    public void PlayAttackAudio()
    {
        attackAudio.Play();
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
        lostAggresion = true;
        StartCoroutine(RegainAggresion(t));
        if (isProvoked)
        {
            UiManager.instance.engagedZombiesUI.EngagedZombiesUpdate(--PlayerManager.instance.engagedZombies);
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

    public void Die()
    {
        GetComponent<NavMeshAgent>().enabled = false;
        isDead = true;
        if(currentAudioSource != null) currentAudioSource.Stop();
        currentAudioSource = null;
        dieAudio.loop = false;
        dieAudio.Play();
        if(isProvoked) UiManager.instance.engagedZombiesUI.EngagedZombiesUpdate(--PlayerManager.instance.engagedZombies);
        GetComponent<Collider>().enabled = false;
        StopAllCoroutines();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }

}
