using UnityEngine;

public class Syringe : MonoBehaviour, IInteractable
{
    [SerializeField] private int cost = 100;
    
    public void interact(Player user)
    {
        if (!CanUse())
        {
            return;
        }

        user.inventory.Coins -= cost;
        user.PlayerStatus.Health = user.MaxHealth;
    }

    private bool CanUse()
    {
        return Player.Instance.inventory.Coins >= cost;
    }

    public string getToolTip()
    {
        if (!CanUse())
        {
            return $"You cannot use syringe ({cost.ToString()}$)";
        }

        return $"Use ({cost.ToString()}$)";
    }
}
