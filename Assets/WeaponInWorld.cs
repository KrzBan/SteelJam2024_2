using UnityEngine;

public class WeaponInWorld : MonoBehaviour, IInteractable
{
    [SerializeField] IItem Item;

    private void Awake()
    {
        Item = new DualHandItem();
    }
    public void interact(Player user)
    {
        user.inventory.ItemSlot = Item;
        user.PlaceInHand(Item.getItemSO());
        Destroy(this.gameObject);
    }

}
