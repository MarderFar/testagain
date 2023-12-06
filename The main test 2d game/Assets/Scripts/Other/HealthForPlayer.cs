using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthForPlayer : MonoBehaviour
{
    Image healthBar;
    public float maxHealth = 100f;
    public float currentHealth;

    void Start()
    {
        healthBar = GetComponent<Image>();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = currentHealth / maxHealth;
    }
}
