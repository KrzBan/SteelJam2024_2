using UnityEngine;

public class WeaponInWorld : MonoBehaviour, IInteractable
{
    [SerializeField] ItemSO Item;

    public string getToolTip()
    {
        if ((!Player.Instance.PlayerStatus.RightArm || !Player.Instance.PlayerStatus.LeftArm) &&
            Item.HandRequirement == HandRequirement.DualHanded)
        {
            return "You have only one hand";
        }
        if (!Player.Instance.PlayerStatus.RightArm)
        {
            return "No hands";
        }
        return "Press e to take";
    }
    public void interact(Player user)
    {

        if (ItemUtility.ShouldRefuseInteract(user, Item)) return;

        user.inventory.ItemSlot = new DualHandItem();

        user.inventory.ItemSlot.setItemSO(Item);
        user.PlaceInHand(Item);
        Destroy(this.gameObject);
    }

}
