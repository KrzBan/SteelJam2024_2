using UnityEngine;

public class FireAxeWeaponWorld : MonoBehaviour, IInteractable
{
    [SerializeField] ItemSO Item;


    public void interact(Player user)
    {

        if(ItemUtility.ShouldRefuseInteract(user, Item)) return;

        user.inventory.ItemSlot = new FireAxeItem();

        user.inventory.ItemSlot.setItemSO(Item);
        user.PlaceInHand(Item);
        Destroy(this.gameObject);
    }

}
