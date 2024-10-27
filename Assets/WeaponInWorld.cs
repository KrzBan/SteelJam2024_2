using UnityEngine;

public class WeaponInWorld : MonoBehaviour, IInteractable
{
    [SerializeField] ItemSO Item;
    [SerializeField] private AudioClip pickUpSFX;

    public string getToolTip()
    {
        if ((!Player.Instance.PlayerStatus.RightArm || !Player.Instance.PlayerStatus.LeftArm) &&
            Item.HandRequirement == HandRequirement.DualHanded)
        {
            return "This weapon requires two hands";
        }
        if (!Player.Instance.PlayerStatus.RightArm)
        {
            return "No hands";
        }
        return "Press e to take";
    }
    public void interact(Player user)
    {
        if (ItemUtility.ShouldRefuseInteract(user, Item))
        {
            return;
        }
        
        AudioSource.PlayClipAtPoint(pickUpSFX, transform.position);

        user.inventory.ItemSlot = new DualHandItem();

        user.inventory.ItemSlot.setItemSO(Item);
        user.PlaceInHand(Item);
        Destroy(this.gameObject);
    }

}
