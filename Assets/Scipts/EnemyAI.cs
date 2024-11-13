using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float attackRange = 1.5f;
    public float chaseRange = 10.0f;
    public float attackCooldown = 2.0f;
    private Animator animator;
    private NavMeshAgent agent;
    private float lastAttackTime;

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (distanceToPlayer <= chaseRange)
        {
            agent.SetDestination(player.position);
            animator.SetBool("IsMoving", true);

            if (distanceToPlayer <= attackRange && Time.time >= lastAttackTime + attackCooldown)
            {
                AttackPlayer();
                lastAttackTime = Time.time;
            }
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }
        
        Debug.Log("Distance to Player: " + distanceToPlayer);
    }

    void AttackPlayer()
    {
        animator.SetTrigger("Attack");
        // Add player damage logic here
    }
}