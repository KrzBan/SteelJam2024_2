using UnityEngine;

public class FireAxeWeaponWorld : MonoBehaviour, IInteractable
{
    [SerializeField] ItemSO Item;


    public void interact(Player user)
    {
        if (!(user.PlayerStatus.RightArm && user.PlayerStatus.RightArm)) return;

        user.inventory.ItemSlot = new FireAxeItem();

        user.inventory.ItemSlot.setItemSO(Item);
        user.PlaceInHand(Item);
        Destroy(this.gameObject);
    }

}
