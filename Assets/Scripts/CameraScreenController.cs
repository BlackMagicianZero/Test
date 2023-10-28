using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraScreenController : MonoBehaviour
{
    public float screenYWhileSKeyPressed = 0.4f; // S 키를 누를 때의 Screen Y 값
    public float originalScreenY = 0.5f; // 원래의 Screen Y 값

    private CinemachineVirtualCamera virtualCamera;

    private void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        
        // 초기 Screen Y 값을 저장
        originalScreenY = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY;
    }

    private void Update()
    {
        // "X" 키를 누르면 Screen Y 값을 변경
        if (Input.GetKey(KeyCode.X))
        {
            // Screen Y 값을 변경
            ChangeScreenY(screenYWhileSKeyPressed);
        }
        else
        {
            // "X" 키를 떼면 Screen Y 값을 원래 값으로
            ChangeScreenY(originalScreenY);
        }
    }

    private void ChangeScreenY(float screenY)
    {
        // CinemachineVirtualCamera의 Body 설정을 변경하여 Screen Y 값을 조절
        virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY = screenY;
    }
}
