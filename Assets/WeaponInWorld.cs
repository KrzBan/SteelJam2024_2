using UnityEngine;

public class WeaponInWorld : MonoBehaviour, IInteractable
{
    [SerializeField] ItemSO Item;

    public string getToolTip()
    {
        return "xd";
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
