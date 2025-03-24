using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HPBar : MonoBehaviour
{
    [SerializeField] private Image hpFillImage;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private Player player;

    private void Update()
    {
        if (player == null) return;

        // HP 바 채우기 업데이트
        float fillAmount = player.CurrentHealth / player.MaxHealth;
        hpFillImage.fillAmount = fillAmount;

        // HP 텍스트 업데이트 (현재 체력/최대 체력)
        hpText.text = $"{Mathf.Ceil(player.CurrentHealth)} / {player.MaxHealth}";
    }
}
