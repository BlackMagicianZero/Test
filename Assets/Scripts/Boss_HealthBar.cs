using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Boss_HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public TMP_Text healthBarText;

    private Damageable ThunderRamGDamageable;

    private void Awake()
    {
        GameObject ThunderRamG = GameObject.FindGameObjectWithTag("BOSS");
        ThunderRamGDamageable = ThunderRamG.GetComponent<Damageable>();
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
        healthSlider.value = CalculateSliderPercentage(ThunderRamGDamageable.Health, ThunderRamGDamageable.MaxHealth);
        healthBarText.text = "HP " + ThunderRamGDamageable.Health + " / " + ThunderRamGDamageable.MaxHealth;
    }

    private float CalculateSliderPercentage(float currentHealth, float maxHealth)
    {
        return currentHealth / maxHealth;
    }
}
