using UnityEngine;

public class Arm : MonoBehaviour
{
    [SerializeField] private Animator animator;
    
    public void Attack(string triggerName)
    {
        if (Player.Instance == null)
        {
            return;
        }

        if (Player.Instance.inventory.ItemSlot == null)
        {
            return;
        }

        animator.SetTrigger(triggerName);
    }

    public void UseItem()
    {
        if (Player.Instance == null)
        {
            return;
        }
        
        Player.Instance.inventory.ItemSlot.Use(Player.Instance);
    }
}
