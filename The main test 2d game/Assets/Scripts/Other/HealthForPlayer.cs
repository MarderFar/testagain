using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthForPlayer : MonoBehaviour
{
    Image healthBar;
    public float maxHealthPlayer = 100f;
    public float currentHealthPlayer;

    void Start()
    {
        healthBar = GetComponent<Image>();
        currentHealthPlayer = maxHealthPlayer;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = currentHealthPlayer / maxHealthPlayer;
    }
}
