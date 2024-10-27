using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
    [SerializeField] private Image leftArm;
    [SerializeField] private Image rightArm;
    [SerializeField] private Image leftLeg;
    [SerializeField] private Image rightLeg;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private TMP_Text keysText;

    private void Start()
    {
        Player.Instance.PlayerStatus.OnLimbStateChanged += UpdateLimbStatus;
        Player.Instance.PlayerStatus.OnHealthReduced += UpdateHealthStatus;
        Player.Instance.PlayerStatus.OnHealthGained += UpdateHealthStatus;
        Player.Instance.inventory.OnKeysChanged += UpdateKeys;
        Player.Instance.inventory.OnCoinsChanged += UpdateMoney;
    }

    private void UpdateLimbStatus(LimbState limbState)
    {
        leftArm.enabled = limbState.LeftArm;
        rightArm.enabled = limbState.RightArm;
        leftLeg.enabled = limbState.LeftLeg;
        rightLeg.enabled = limbState.RightLeg;
    }

    private void UpdateHealthStatus(float health)
    {
        healthText.text = ((int)Mathf.Ceil(health)).ToString();
    }
    
    private void UpdateKeys(int keys)
    {
        keysText.text = $"Keys: {keys.ToString()}";
    }

    private void UpdateMoney(int coins)
    {
        moneyText.text = $"{coins.ToString()}$";
    }
}
