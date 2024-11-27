using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // ตรวจสอบว่าตัวที่ชนเป็นมือ VR หรือ Object ที่เราต้องการ
        if (other.CompareTag("Key"))
        {
            ChangeScene();
        }
    }
    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void ChangeScene()
    {
        RestartScene();
    }
}
