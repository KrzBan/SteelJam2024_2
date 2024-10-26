using UnityEngine;

public class Gillotine : AnimationInteractableBase
{
    public override void interact(Player user)
    {
        base.interact(user);

        if (Player.Instance != null)
        {
            Player.Instance.RemoveLimbOrdered();
        }
    }
}
