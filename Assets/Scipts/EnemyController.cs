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

    private AudioSource audioSource; // AudioSource component
    private Vector3 lastPosition; // ตัวแปรเก็บตำแหน่งก่อนหน้า

    private void Start()
    {
        currentHealth = maxHealth; // ตั้งค่า HP เริ่มต้น
        audioSource = GetComponent<AudioSource>(); // หาคอมโพเนนต์ AudioSource
        lastPosition = transform.position; // เก็บตำแหน่งเริ่มต้น
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
        HandleMovementAudio(); // เรียกใช้ฟังก์ชันตรวจสอบการเคลื่อนไหว
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

    private void HandleMovementAudio()
    {
        // ตรวจสอบว่าตำแหน่งปัจจุบันต่างจากตำแหน่งก่อนหน้าหรือไม่
        if (transform.position != lastPosition)
        {
            if (!audioSource.isPlaying) // ถ้าเสียงยังไม่เล่น
            {
                audioSource.Play(); // เล่นเสียง
            }
        }
        else
        {
            if (audioSource.isPlaying) // ถ้าเสียงกำลังเล่น
            {
                audioSource.Stop(); // หยุดเสียง
            }
        }

        // อัพเดตตำแหน่งก่อนหน้า
        lastPosition = transform.position;
    }

    private void OnDrawGizmosSelected()
    {
        // แสดงระยะโจมตีใน Scene View
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
