using UnityEngine;
using UnityEngine.AI;

public class EnemyFollowPlayer : MonoBehaviour
{
    public Transform player;        // Drag the player's Transform here in the Inspector
    public float followDistance = 10f; // Maximum distance to follow the player
    public float speed = 3.5f;      // Enemy movement speed

    private NavMeshAgent agent;     // Reference to NavMeshAgent component (optional)

    void Start()
    {
        // Try to get NavMeshAgent component if it's present
        agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.speed = speed;
            agent.stoppingDistance = 2f; // Enemy stops this far from the player
        }
    }

    void Update()
    {
        if (player == null)
        {
            Debug.LogWarning("Player Transform not assigned!");
            return;
        }

        // Calculate distance to the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Check if player is within follow distance
        if (distanceToPlayer <= followDistance)
        {
            // Move towards player if within follow distance
            if (agent != null)
            {
                // Use NavMeshAgent for smooth movement
                agent.SetDestination(player.position);
            }
            else
            {
                // Move directly towards player if NavMeshAgent is not available
                Vector3 direction = (player.position - transform.position).normalized;
                transform.position += direction * speed * Time.deltaTime;
            }

            // Make enemy face the player
            Vector3 lookDirection = player.position - transform.position;
            lookDirection.y = 0; // Keep enemy level
            Quaternion rotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5f);
        }
    }
}