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

    // Start is called before the first frame update
    private void Start()
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

        // 처음에는 UI를 활성화
        statusCanvas.SetActive(true);
    }

    // Update is called once per frame
    private void Update()
{
    // U 키를 누르면 Status UI의 활성화 상태를 체크하고 토글합니다.
    if (Input.GetKeyDown(KeyCode.U))
    {
        // UI의 활성화 상태를 체크
        bool isStatusCanvasActive = statusCanvas.activeSelf;

        // UI의 활성화 상태를 토글
        statusCanvas.SetActive(!isStatusCanvasActive);

        // UI의 활성화 상태를 출력 (Debug용)
        Debug.Log("Status UI is now " + (isStatusCanvasActive ? "active" : "inactive"));
    }

    // UI가 활성화된 경우에만 업데이트 수행
    if (statusCanvas.activeSelf)
    {
        if (playerDamageable != null && healthBar != null)
        {
            // UI가 활성화되고 플레이어의 Damageable 컴포넌트와 HealthBar 스크립트가 있다면 HP 업데이트
            healthText.text = healthBar.playerDamageable.Health + " / " + healthBar.playerDamageable.MaxHealth;
        }

        if (playerAttack != null)
        {
            // Attack 값을 업데이트합니다.
            attackText.text = "" + playerAttack.attackDamage;
        }
    }
}

}
