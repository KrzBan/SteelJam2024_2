using System;
using UnityEngine;

public class Inventory
{
    public Action<int> OnKeysChanged { get; set; }
    public Action<int> OnCoinsChanged { get; set; }

    public int Keys
    {
        get => keys;
        set
        {
            keys = value;
            OnKeysChanged?.Invoke(value);
        }
    }

    public int Coins
    {
        get => coins;
        set
        {
            coins = value;
            OnCoinsChanged?.Invoke(value);
        }
    }
    
    public IItem ItemSlot;

    private int keys = 0;
    private int coins = 0;

    public bool ConsumeKey()
    {
        if (Keys == 0) return false;

        --Keys;
        return true;
    }
}
