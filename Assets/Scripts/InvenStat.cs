using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InvenStat : MonoBehaviour
{
    public TMP_Text healthText;
    public TMP_Text AtkText;
    private int atk1 = 10;
    private int atk2 = 15;
    private int atk3 = 25;
    public GameObject player;
    private PlayerController playercontroller;
    private Damageable playerDamageable;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if(player == null)
        {
            Debug.Log("No player found in the scene. Make sure it has tag 'Player'");
        }
        playercontroller = FindObjectOfType<PlayerController>();
        playerDamageable = player.GetComponent<Damageable>();
    }
    void Start()
    {
        UpdateHealthUI();
        UpdateATKUI();
    }
    void Update()
    {
        // 실시간으로 체력 정보 업데이트
        UpdateHealthUI();
        // 실시간으로 데미지 정보 업데이트
        UpdateATKUI();
    }

    private void UpdateHealthUI()
    {
        healthText.text = playerDamageable.Health + " / " + playerDamageable.MaxHealth;
    }

    private void UpdateATKUI()
    {
        GameObject swordAttack1 = player.transform.Find("SwordAttack1").gameObject;
        GameObject swordAttack2 = player.transform.Find("SwordAttack2").gameObject;
        GameObject swordAttack3 = player.transform.Find("SwordAttack3").gameObject;
        Attack attack1 = swordAttack1.GetComponent<Attack>();
        Attack attack2 = swordAttack2.GetComponent<Attack>();
        Attack attack3 = swordAttack3.GetComponent<Attack>();

        AtkText.text = atk1 + "(+" + (attack1.attackDamage - atk1) + ")" + atk2 + "(+" + (attack2.attackDamage - atk2) + ")" + atk3 + "(+" + (attack3.attackDamage - atk3) + ")";
    }
}
