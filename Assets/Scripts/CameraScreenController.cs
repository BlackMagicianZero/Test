using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class CameraScreenController : MonoBehaviour
{
    public float screenYWhileSKeyPressed = 0.3f; // S 키를 누를 때의 Screen Y 값
    public float originalScreenY = 0.65f; // 원래의 Screen Y 값
    public float transitionDuration = 0.5f;

    private CinemachineVirtualCamera virtualCamera;

    private void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        
        // 초기 Screen Y 값을 저장
        originalScreenY = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            ChangeScreenY(screenYWhileSKeyPressed);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            ChangeScreenY(originalScreenY);
        }
    }

    private void ChangeScreenY(float screenY)
    {
        // CinemachineVirtualCamera의 Body 설정을 변경하여 Screen Y 값을 조절
        virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY = screenY;
    }
}
