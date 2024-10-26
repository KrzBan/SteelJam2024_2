using UnityEngine;

public class Inventory
{
    public int Keys { get; set; }
    public int Coins { get; set; }
    
    public IItem ItemSlot;

    public bool ConsumeKey()
    {
        if (Keys == 0) return false;

        --Keys;
        return true;
    }
}
