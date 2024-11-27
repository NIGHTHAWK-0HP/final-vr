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
        // เคลื่อนที่ไปยังเป้าหมาย
        Vector3 direction = (lastPlayerPosition - transform.position).normalized; // ใช้ตำแหน่งล่าสุดของผู้เล่น
        transform.position += direction * followSpeed * Time.deltaTime;

        // หมุนหน้าไปทางเป้าหมาย
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void StopMoving()
    {
        // หยุดการเคลื่อนที่
        Debug.Log("Stopping: No clear path to target");
    }

    void RandomWalk()
    {
        // หากถึงเวลาที่ต้องเปลี่ยนตำแหน่งใหม่
        if (Time.time >= randomWalkTimer)
        {
            // สุ่มตำแหน่งในระยะที่กำหนด
            Vector2 randomCircle = Random.insideUnitCircle * randomWalkRadius;
            randomDestination = new Vector3(
                transform.position.x + randomCircle.x,
                transform.position.y,
                transform.position.z + randomCircle.y
            );

            // ตั้งเวลาใหม่สำหรับการเดินครั้งต่อไป
            randomWalkTimer = Time.time + randomWalkDelay;
        }

        // เดินไปยังตำแหน่งสุ่ม
        Vector3 direction = (randomDestination - transform.position).normalized;
        transform.position += direction * (followSpeed * 0.5f) * Time.deltaTime; // ลดความเร็วลงเล็กน้อย

        // หมุนหน้าไปทางจุดหมาย
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

        // หยุดเมื่อถึงจุดหมายปลายทาง
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
        if (target == null) return; // หากไม่มีเป้าหมาย ไม่ต้องหันหน้า
    
        // คำนวณทิศทางไปยังเป้าหมาย
        Vector3 directionToTarget = (target.position - transform.position).normalized;
    
        // ลบแกน Y เพื่อป้องกันการเอียง
        directionToTarget.y = 0;
    
        // คำนวณการหมุนใหม่
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
    
        // ปรับการหมุนอย่างนุ่มนวล
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

}
