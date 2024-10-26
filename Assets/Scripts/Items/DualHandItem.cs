using UnityEngine;

public class DualHandItem : IItem
{
    public ItemSO ItemSO;

    public DualHandItem()
    {
       
    }

    public GameObject GetPrefab()
    {
        return ItemSO.Prefab;
    }

    public void Use(PlayerManager User)
    {
        
    }
}
