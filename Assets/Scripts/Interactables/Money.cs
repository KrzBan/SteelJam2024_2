using UnityEngine;

public class Money : MonoBehaviour, IInteractable
{
    public int Amount;
    public string getToolTip()
    {
        return "xd";
    }
    public void interact(Player user)
    {
        user.inventory.Coins += Amount;
        Destroy(gameObject);
    }
}
