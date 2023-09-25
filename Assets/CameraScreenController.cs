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
        // CinemachineVirtualCamera 컴포넌트를 가져옵니다.
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        
        // 초기 Screen Y 값을 저장합니다.
        originalScreenY = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY;
    }

    private void Update()
    {
        // "S" 키를 누르면 Screen Y 값을 변경합니다.
        if (Input.GetKey(KeyCode.S))
        {
            // Screen Y 값을 변경합니다.
            ChangeScreenY(screenYWhileSKeyPressed);
        }
        else
        {
            // "S" 키를 떼면 Screen Y 값을 원래 값으로 돌려놓습니다.
            ChangeScreenY(originalScreenY);
        }
    }

    private void ChangeScreenY(float screenY)
    {
        // CinemachineVirtualCamera의 Body 설정을 변경하여 Screen Y 값을 조절합니다.
        virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY = screenY;
    }
}
