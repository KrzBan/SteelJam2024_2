using UnityEngine;

public class Key : MonoBehaviour, IInteractable
{
    public string getToolTip()
    {
        return "xd";
    }
    public void interact(Player user)
    {
        user.inventory.Keys++;
        Destroy(gameObject);
    }
}
