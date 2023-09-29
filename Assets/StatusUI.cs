using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StatusUI : MonoBehaviour
{
    public GameObject statusCanvas; // Status UI의 Canvas GameObject를 여기에 연결하세요.
    public TMP_Text healthText; // HP를 표시할 TMP_Text 컴포넌트를 여기에 연결하세요.
    public HealthBar healthBar; // HealthBar 스크립트에 접근하기 위한 변수
    Damageable playerDamageable;
    Attack playerAttack;
    public TMP_Text attackText;

    void Start()
    {
        // Player를 찾아 Damageable 컴포넌트 가져오기
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerDamageable = player.GetComponent<Damageable>();
            playerAttack = player.GetComponent<Attack>();
        }
        else
        {
            Debug.LogError("Player not found. Make sure it has tag 'Player'");
        }

        // 처음에는 UI를 비활성화
        statusCanvas.SetActive(false);
    }

    void Update()
    {
        // U 키를 누르면 Status UI를 열거나 닫습니다.
        if (Input.GetKeyDown(KeyCode.U))
        {
            // UI를 활성화 또는 비활성화
            statusCanvas.SetActive(!statusCanvas.activeSelf);

            if (statusCanvas.activeSelf && playerDamageable != null && healthBar != null)
            {
                // UI가 활성화되고 플레이어의 Damageable 컴포넌트와 HealthBar 스크립트가 있다면 HP 업데이트
                UpdateHealthText();
            }
            if (statusCanvas.activeSelf && playerDamageable != null && playerAttack != null)
            {
                UpdateAttackText(); // Attack 값을 업데이트합니다.
            }
        }
    }

    void UpdateHealthText()
    {
        if (playerDamageable != null && healthBar != null)
        {
            // HP를 TMP_Text로 표시
            healthText.text = healthBar.playerDamageable.Health + " / " + healthBar.playerDamageable.MaxHealth;
        }
    }

    void UpdateAttackText()
{
    if (playerAttack != null)
    {
        Debug.Log("Player Attack Script Found!"); // 디버그 로그 추가
        attackText.text = "" + playerAttack.attackDamage;
    }
    else
    {
        Debug.LogWarning("Player Attack Script Not Found!"); // 디버그 로그 추가
    }
}
}