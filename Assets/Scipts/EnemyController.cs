using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float maxHealth = 100f; // HP สูงสุดของศัตรู
    public float attackDamage = 20f; // ความเสียหายจากการโจมตี
    public float attackRange = 2f; // ระยะโจมตี
    public float attackCooldown = 1.5f; // เวลา cooldown ระหว่างการโจมตี
    public string targetTag = "Player"; // แท็กของเป้าหมาย
    private float currentHealth;
    private float lastAttackTime;

    private void Start()
    {
        currentHealth = maxHealth; // ตั้งค่า HP เริ่มต้น
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. Remaining HP: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        Destroy(gameObject); // ทำลายศัตรู
    }

    private void Update()
    {
        FindAndAttackPlayer();
    }

    private void FindAndAttackPlayer()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, attackRange);

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag(targetTag))
            {
                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    Attack(hit.gameObject);
                    lastAttackTime = Time.time;
                }
                break;
            }
        }
    }

    private void Attack(GameObject target)
    {
        Debug.Log($"{gameObject.name} attacks {target.name}");
        if (target.TryGetComponent(out PlayerController player))
        {
            player.TakeDamage(attackDamage); // ลด HP ของผู้เล่น
        }
    }

    private void OnDrawGizmosSelected()
    {
        // แสดงระยะโจมตีใน Scene View
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
