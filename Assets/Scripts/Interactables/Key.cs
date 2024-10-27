using UnityEngine;

public class Key : MonoBehaviour, IInteractable
{
    public string getToolTip()
    {
        return "Press e to pick up";
    }
    public void interact(Player user)
    {
        user.inventory.Keys++;
        Destroy(gameObject);
    }
}
