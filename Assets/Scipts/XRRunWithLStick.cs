using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class XRRunWithLStick : MonoBehaviour
{
    public ActionBasedContinuousMoveProvider moveProvider; // ตัวควบคุมการเคลื่อนที่
    public InputActionAsset inputActions; // Asset ของ Input
    public string runActionName = "Run"; // ชื่อของ Action สำหรับ "วิ่ง"
    public float walkSpeed = 2.0f; // ความเร็วเดิน
    public float runSpeed = 6.0f; // ความเร็ววิ่ง

    private InputAction runAction; // Action สำหรับ "วิ่ง"

    void Start()
    {
        if (inputActions == null)
        {
            Debug.LogError("Input Action Asset ไม่ได้ถูกเชื่อมต่อใน Inspector!");
            return;
        }

        // ดึง Action โดยตรงจาก Asset
        runAction = inputActions.FindAction(runActionName);

        if (runAction == null)
        {
            Debug.LogError($"Input Action '{runActionName}' ไม่พบใน Input Action Asset!");
            return;
        }

        // เปิดใช้งาน Action
        runAction.Enable();
    }

    void Update()
    {
        if (runAction == null || moveProvider == null)
            return;

        // เปลี่ยนความเร็วตามสถานะปุ่ม
        moveProvider.moveSpeed = runAction.IsPressed() ? runSpeed : walkSpeed;
    }
}