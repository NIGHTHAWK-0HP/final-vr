using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour
{
    public float damage = 50f;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
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
        }
    }
}