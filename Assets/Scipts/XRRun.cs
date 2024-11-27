using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class XRRunWithLStick : MonoBehaviour
{
    public ActionBasedContinuousMoveProvider moveProvider; // ตัวควบคุมการเคลื่อนที่
    public InputActionAsset inputActions; // Input Action Asset
    public float walkSpeed = 2.0f; // ความเร็วเดิน
    public float runSpeed = 6.0f; // ความเร็ววิ่ง

    private InputAction runAction; // Action สำหรับการวิ่ง

    void Start()
    {
        // ค้นหา Action Map และ Action
        var actionMap = inputActions.FindActionMap("Player");
        if (actionMap != null)
        {
            runAction = actionMap.FindAction("Run");
            if (runAction != null)
                runAction.Enable(); // เปิดใช้งาน Action
        }
        else
        {
            Debug.LogError("Player Action Map not found in Input Actions!");
        }
    }

    void Update()
    {
        if (runAction != null && runAction.IsPressed()) // เมื่อกด L Stick
        {
            moveProvider.moveSpeed = runSpeed;
        }
        else // เมื่อปล่อย L Stick
        {
            moveProvider.moveSpeed = walkSpeed;
        }
    }
}