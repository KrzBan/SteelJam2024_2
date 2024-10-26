using UnityEngine;

public class WeaponInWorld : MonoBehaviour, IInteractable
{
    [SerializeField] ItemSO ItemSO;
    public void interact(Player user)
    {
        user.inventory.ItemSlot = ItemSO;
        user.PlaceInHand(ItemSO);
        Destroy(this.gameObject);
    }

}
