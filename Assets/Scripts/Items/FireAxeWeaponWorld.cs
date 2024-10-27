using UnityEngine;

public class FireAxeWeaponWorld : MonoBehaviour, IInteractable
{
    [SerializeField] ItemSO Item;
    [SerializeField] private AudioClip pickUpSFX;

    public string getToolTip()
    {
        if ((!Player.Instance.PlayerStatus.RightArm || !Player.Instance.PlayerStatus.LeftArm) &&
            Item.HandRequirement == HandRequirement.DualHanded)
        {
            return "You lack a hand";
        }
        if (!Player.Instance.PlayerStatus.RightArm)
        {
            return "You lack hands";
        }
        return "Press e to take";
    }
    public void interact(Player user)
    {

        if(ItemUtility.ShouldRefuseInteract(user, Item)) return;
        
        AudioSource.PlayClipAtPoint(pickUpSFX, transform.position);

        user.inventory.ItemSlot = new FireAxeItem();

        user.inventory.ItemSlot.setItemSO(Item);
        user.PlaceInHand(Item);
        Destroy(this.gameObject);
    }

}
