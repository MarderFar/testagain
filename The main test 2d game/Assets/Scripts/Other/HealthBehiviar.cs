using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBehiviar : MonoBehaviour
{
    public Slider slider;
    public Color Low;
    public Color High;
    public Vector3 Offset;
    public void SetHelth(float health, float maxHealth)
    {
        slider.gameObject.SetActive(health < maxHealth);
        slider.value = health;
        slider.maxValue = maxHealth;

        slider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(Low, High, slider.normalizedValue);

    }
    void Update()
    {
        slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + new Vector3(0,2,0));
    }
}
