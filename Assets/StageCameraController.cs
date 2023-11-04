using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class StageCameraController : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;

    private void Awake()
    {
        // CinemachineVirtualCamera 컴포넌트를 가져옵니다.
        virtualCamera = GetComponent<CinemachineVirtualCamera>();

        // Player를 찾아서 해당 오브젝트의 트랜스폼을 Follow 속성으로 설정합니다.
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && virtualCamera != null)
        {
            virtualCamera.Follow = player.transform;
        }
        else
        {
            Debug.LogError("Player 또는 CinemachineVirtualCamera를 찾을 수 없습니다!");
        }

        // 이 스크립트가 파괴되지 않도록 설정합니다.
        DontDestroyOnLoad(gameObject);
    }
}
