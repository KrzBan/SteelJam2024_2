using UnityEngine;
using UnityEngine.UI;

public class EquipmentUI : UIElementBase
{
    [SerializeField] private Image head;
    [SerializeField] private Image lArm;
    [SerializeField] private Image rArm;
    [SerializeField] private Image lLeg;
    [SerializeField] private Image rLeg;
    [SerializeField] private Color missingColor;
    
    public override void Show()
    {
        if (Player.Instance == null)
        {
            return;
        }

        var playerStatus = Player.Instance.PlayerStatus;

        head.color = playerStatus.Head ? Color.white : missingColor;
        lArm.color = playerStatus.LeftArm ? Color.white : missingColor;
        rArm.color = playerStatus.RightArm ? Color.white : missingColor;
        lLeg.color = playerStatus.LeftLeg ? Color.white : missingColor;
        rLeg.color = playerStatus.RightLeg ? Color.white : missingColor;
        
        gameObject.SetActive(true);
    }

    public override void Hide()
    {
        gameObject.SetActive(false);
    }
}
