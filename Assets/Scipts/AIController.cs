using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public float detectionRadius = 10f; // ระยะที่ AI สามารถตรวจจับผู้เล่นได้
    public string targetTag = "Player"; // กำหนด Tag ของเป้าหมาย
    public float followSpeed = 3f; // ความเร็วในการติดตามผู้เล่น
    public LayerMask obstacleLayer; // กำหนด Layer ของสิ่งกีดขวาง เช่น กำแพง
    public float randomWalkRadius = 5f; // ระยะการเดินแบบสุ่ม
    public float randomWalkDelay = 3f; // เวลาหยุดรอก่อนเดินครั้งต่อไป
    private Transform target; // เก็บข้อมูลของเป้าหมายที่ตรวจจับได้
    private Vector3 randomDestination; // จุดหมายปลายทางสุ่ม
    private float randomWalkTimer; // ตัวจับเวลาสำหรับเดินแบบสุ่ม
    private Vector3 lastPlayerPosition; // เก็บตำแหน่งล่าสุดของผู้เล่น

    void Update()
    {
        FindTarget();
    
        if (target != null)
        {
            lastPlayerPosition = target.position; // อัปเดตตำแหน่งล่าสุดของผู้เล่น
            RotateTowardsTarget(); // หันหน้าไปทางผู้เล่น
    
            if (!IsObstructed())
            {
                FollowTarget(); // ติดตามเป้าหมาย
            }
            else
            {
                StopMoving(); // หยุดเมื่อมีสิ่งกีดขวาง
            }
        }
        else
        {
            RandomWalk(); // เดินแบบสุ่มเมื่อไม่มีเป้าหมาย
        }
    }


    void FindTarget()
    {
        // ค้นหา Collider ทั้งหมดในระยะที่กำหนด
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag(targetTag))
            {
                target = hit.transform; // เก็บเป้าหมาย
                return; // หยุดการค้นหาเมื่อเจอเป้าหมาย
            }
        }
        target = null; // ถ้าไม่มีเป้าหมายในระยะ
    }

    bool IsObstructed()
    {
        if (target == null) return true;

        // ยิง Ray จาก AI ไปยังเป้าหมาย
        Vector3 directionToTarget = target.position - transform.position;
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        // ตรวจสอบ Raycast สำหรับสิ่งกีดขวาง
        if (Physics.Raycast(transform.position, directionToTarget.normalized, out RaycastHit hit, distanceToTarget, obstacleLayer))
        {
            Debug.Log("Obstructed by: " + hit.collider.name);
            return true; // ถ้ามีสิ่งกีดขวาง
        }
        return false; // ถ้าไม่มีสิ่งกีดขวาง
    }

    void FollowTarget()
    {
        if (target == null) return;

        Vector3 direction = (lastPlayerPosition - transform.position).normalized;

        // ตรวจสอบสิ่งกีดขวางด้วย Raycast
        if (!Physics.Raycast(transform.position, direction, Vector3.Distance(transform.position, lastPlayerPosition), obstacleLayer))
        {
            transform.position += direction * followSpeed * Time.deltaTime;

            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
        else
        {
            Debug.Log("Path to target obstructed by Props");
        }
    }


    void StopMoving()
    {
        // หยุดการเคลื่อนที่
        Debug.Log("Stopping: No clear path to target");
    }

    void RandomWalk()
    {
        if (Time.time >= randomWalkTimer)
        {
            for (int i = 0; i < 10; i++) // ลองสุ่มจุดหมายปลายทางไม่เกิน 10 ครั้ง
            {
                Vector2 randomCircle = Random.insideUnitCircle * randomWalkRadius;
                Vector3 potentialDestination = new Vector3(
                    transform.position.x + randomCircle.x,
                    transform.position.y,
                    transform.position.z + randomCircle.y
                );

                // ตรวจสอบด้วย Raycast เพื่อดูว่ามีสิ่งกีดขวางใน Layer Props หรือไม่
                if (!Physics.Raycast(transform.position, (potentialDestination - transform.position).normalized,
                        Vector3.Distance(transform.position, potentialDestination), obstacleLayer))
                {
                    randomDestination = potentialDestination; // ใช้ตำแหน่งที่ผ่านเงื่อนไข
                    break;
                }
            }

            randomWalkTimer = Time.time + randomWalkDelay; // ตั้งเวลาใหม่
        }

        // เดินไปยังตำแหน่งสุ่ม
        Vector3 direction = (randomDestination - transform.position).normalized;
        transform.position += direction * (followSpeed * 0.5f) * Time.deltaTime;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

        if (Vector3.Distance(transform.position, randomDestination) < 0.5f)
        {
            Debug.Log("Random destination reached");
        }
    }

    private void OnDrawGizmosSelected()
    {
        // วาดวงกลมแสดงระยะการตรวจจับใน Scene View
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        // วาดวงกลมแสดงระยะการเดินแบบสุ่ม
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, randomWalkRadius);

        // วาดเส้นทางไปยังจุดสุ่ม
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, randomDestination);
    }
    
    void RotateTowardsTarget()
    {
        if (target == null) return; // หากไม่มีเป้าหมาย ไม่ต้องหมุน
    
        // คำนวณทิศทางไปยังเป้าหมาย
        Vector3 directionToTarget = target.position - transform.position;
    
        // ลบแกน Y เพื่อป้องกันการเอียงขึ้น/ลง
        directionToTarget.y = 0;
    
        // ตรวจสอบว่ามีทิศทางที่ชัดเจน
        if (directionToTarget.sqrMagnitude > 0.01f)
        {
            // คำนวณการหมุนใหม่
            Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
    
            // ปรับการหมุนอย่างนุ่มนวล
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }


}
