using UnityEngine;

public class Money : MonoBehaviour, IInteractable
{
    [SerializeField] private AudioClip pickUpSFX;
    public int Amount;
    public string getToolTip()
    {
        return $"Press e to pick up {Amount.ToString()}$";
    }
    public void interact(Player user)
    {
        user.inventory.Coins += Amount;
        AudioSource.PlayClipAtPoint(pickUpSFX, transform.position);
        Destroy(gameObject);
    }
}
