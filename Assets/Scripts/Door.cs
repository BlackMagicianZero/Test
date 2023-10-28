using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    // 다음으로 이동할 씬의 이름
    public string nextSceneName;

    private bool hasPlayerEntered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasPlayerEntered && collision.CompareTag("Player"))
        {
            hasPlayerEntered = true;
            // 다음 씬의 이름으로 이동
            SceneManager.LoadScene(nextSceneName);
            Debug.Log("씬 이동!");
        }
    }

    private void Update()
    {
        // 현재 씬에 플레이어가 있는지 확인하고 RespawnArea로 이동
        if (hasPlayerEntered)
        {
            GameObject respawnArea = GameObject.FindGameObjectWithTag("RespawnArea");

            // RespawnArea를 찾았으면 플레이어를 해당 위치로 이동
            if (respawnArea != null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                player.transform.position = respawnArea.transform.position;
                Debug.Log("RespawnArea로 이동!");
            }
            else
            {
                Debug.LogWarning("RespawnArea를 찾을 수 없습니다!");
            }
            hasPlayerEntered = false;
        }
    }
}
