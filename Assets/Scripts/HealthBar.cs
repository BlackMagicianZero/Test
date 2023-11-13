using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public TMP_Text healthBarText;

    public Damageable playerDamageable;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if(player == null)
        {
            Debug.Log("No player found in the scene. Make sure it has tag 'Player'");
        }

        playerDamageable = player.GetComponent<Damageable>();
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateHealthUI();
    }
    void Update()
    {
        // 실시간으로 체력 정보 업데이트
        UpdateHealthUI();
    }

    private void OnPlayerHealthChanged(int newHealth, int maxHealth)
    {
        healthSlider.value = CalculateSliderPercentage(newHealth, maxHealth);
        healthBarText.text = "HP " + newHealth + " / " + maxHealth;
    }

    private void UpdateHealthUI()
    {
        healthSlider.value = CalculateSliderPercentage(playerDamageable.Health, playerDamageable.MaxHealth);
        healthBarText.text = "HP " + playerDamageable.Health + " / " + playerDamageable.MaxHealth;
    }

    // Calculates with floats so value is returned as a decimal value
    private float CalculateSliderPercentage(float currentHealth, float maxHealth)
    {
        return currentHealth / maxHealth;
    }
}
