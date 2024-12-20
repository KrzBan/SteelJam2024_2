using UnityEngine;

public interface IInteractable
{
    public void interact(Player user);
    public string getToolTip();
}
