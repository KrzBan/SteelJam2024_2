using UnityEngine;

public class Key : MonoBehaviour, IInteractable
{
    public void interact(Player user)
    {
        user.inventory.Keys++;
        Destroy(gameObject);
    }
}
