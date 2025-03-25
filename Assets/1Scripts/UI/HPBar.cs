using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HPBar : MonoBehaviour
{
    [SerializeField] private Image hpFillImage;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private Transform target;

    private IDamageable damageable;


    void Start()
    {
        damageable = target.GetComponent<IDamageable>();
    }
    private void FixedUpdate()
    {
        if (target == null) return;

        float fillAmount = (float)damageable.CurrentHealth / damageable.MaxHealth;
        hpFillImage.fillAmount = Mathf.Clamp(fillAmount, 0f, 1f);


        hpText.text = $"{Mathf.Ceil(damageable.CurrentHealth)} / {damageable.MaxHealth}";
    }
}
