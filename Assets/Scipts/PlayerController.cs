using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float maxHealth = 100f; // HP สูงสุดของผู้เล่น
    private float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth; // ตั้งค่า HP เริ่มต้น
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"Player took {damage} damage. Remaining HP: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player has died.");
        // จัดการเมื่อผู้เล่นตาย เช่น Restart Level หรือแสดง Game Over
    }
}