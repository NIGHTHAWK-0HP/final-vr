using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    public float speed = 50f;
    public float lifetime = 1f;
    public float damage = 50f; // ความเสียหายของกระสุน
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            // ตรวจสอบว่าศัตรูมีคอมโพเนนต์ EnemyController หรือไม่
            if (collision.collider.TryGetComponent(out EnemyController enemy))
            {
                enemy.TakeDamage(damage); // ลด HP ของศัตรู
            }
            Destroy(gameObject); // ทำลายกระสุน
        }
    }
}
