using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explain : MonoBehaviour
{
    public GameObject imageObject; // UI 이미지 GameObject를 여기에 연결하세요.

    void Update()
    {
        // F1 키 입력을 감지합니다.
        if (Input.GetKeyDown(KeyCode.F1))
        {
            // 이미지의 활성/비활성을 토글합니다.
            if (imageObject != null)
            {
                imageObject.SetActive(!imageObject.activeSelf);
            }
        }
    }
}
