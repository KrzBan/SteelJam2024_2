using UnityEngine;

public class Money : MonoBehaviour, IInteractable
{
    public int Amount;
    public string getToolTip()
    {
        return $"Press e to pick up {Amount.ToString()}$";
    }
    public void interact(Player user)
    {
        user.inventory.Coins += Amount;
        Destroy(gameObject);
    }
}
