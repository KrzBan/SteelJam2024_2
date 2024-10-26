using System;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
    [SerializeField] private Image leftArm;
    [SerializeField] private Image rightArm;
    [SerializeField] private Image leftLeg;
    [SerializeField] private Image rightLeg;

    private void Start()
    {
        Player.Instance.PlayerStatus.OnLimbStateChanged += UpdateLimbStatus;
    }

    private void UpdateLimbStatus(LimbState limbState)
    {
        leftArm.enabled = limbState.LeftArm;
        rightArm.enabled = limbState.RightArm;
        leftLeg.enabled = limbState.LeftLeg;
        rightLeg.enabled = limbState.RightLeg;
    }
}
