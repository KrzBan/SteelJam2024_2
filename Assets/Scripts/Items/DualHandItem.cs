using UnityEngine;

public class DualHandItem : IItem
{
    public ItemSO ItemSO;

    public DualHandItem()
    {
       
    }

    public ItemSO getItemSO()
    {
        return ItemSO;
    }

    public GameObject GetPrefab()
    {
        return ItemSO.Prefab;
    }

    public void Use(Player User)
    {
        
    }
}
