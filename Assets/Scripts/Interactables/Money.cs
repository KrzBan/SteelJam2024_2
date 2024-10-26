using UnityEngine;

public class Money : MonoBehaviour, IInteractable
{
    public int Amount;
    
    public void interact(Player user)
    {
        user.inventory.Coins += Amount;
        Destroy(gameObject);
    }
}
