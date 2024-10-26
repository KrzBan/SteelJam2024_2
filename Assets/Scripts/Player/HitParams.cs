using UnityEngine;

public class HitParams
{
    public float Damage { get; }
    public float LibLossChance { get; }

    public HitParams(float damage, float libLossChance)
    {
        Damage = damage;
        LibLossChance = libLossChance;
    }
}
