using UnityEngine;

public class Key : MonoBehaviour, IInteractable
{
    [SerializeField] private AudioClip pickUpSFX;
    
    public string getToolTip()
    {
        return "Press e to pick up";
    }
    public void interact(Player user)
    {
        user.inventory.Keys++;
        AudioSource.PlayClipAtPoint(pickUpSFX, transform.position);
        Destroy(gameObject);
    }
}
