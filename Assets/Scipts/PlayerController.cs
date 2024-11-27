using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float maxHealth = 100f; // HP สูงสุดของผู้เล่น
    private float currentHealth;
    public AudioSource audioSource; // Reference to the AudioSource component
    public AudioClip moveSound; // Audio clip to play when moving

    private Vector3 lastPosition;

    private void Start()
    {
        currentHealth = maxHealth; // ตั้งค่า HP เริ่มต้น
        lastPosition = transform.position; // บันทึกตำแหน่งเริ่มต้น
    }

    private void Update()
    {
        // ตรวจสอบว่าเปลี่ยนตำแหน่งหรือไม่
        if (transform.position != lastPosition)
        {
            PlayMoveSound(); // เล่นเสียงเมื่อมีการเคลื่อนที่
            lastPosition = transform.position; // อัปเดตตำแหน่งล่าสุด
        }
        else
        {
            StopMoveSound(); // หยุดเสียงเมื่อไม่เคลื่อนไหว
        }
    }

    private void PlayMoveSound()
    {
        if (audioSource != null && moveSound != null && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(moveSound); // เล่นเสียงเพียงครั้งเดียว
        }
    }

    private void StopMoveSound()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop(); // หยุดเสียง
        }
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
    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void Die()
    {
        Debug.Log("Player has died.");
        RestartScene();
    }
}