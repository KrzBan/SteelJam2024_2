using UnityEngine;

public class WeaponInWorld : MonoBehaviour, IInteractable
{
    [SerializeField] ItemSO Item;
    

    public void interact(Player user)
    {
        user.inventory.ItemSlot = new DualHandItem();

        user.inventory.ItemSlot.setItemSO(Item);
        user.PlaceInHand(Item);
        Destroy(this.gameObject);
    }

}
