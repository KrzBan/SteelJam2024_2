using UnityEngine;

public class BatItemWorld : MonoBehaviour, IInteractable
{
    [SerializeField] ItemSO Item;

    public string getToolTip()
    {
        if (!Player.Instance.PlayerStatus.RightArm)
        {
            return "You need more hands to wield this item!";
        }
        return "Press e to take";
    }
    public void interact(Player user)
    {
        if (ItemUtility.ShouldRefuseInteract(user, Item)) return;


        user.inventory.ItemSlot = new FireAxeItem();

        user.inventory.ItemSlot.setItemSO(Item);
        user.PlaceInHand(Item);
        Destroy(this.gameObject);
    }

}