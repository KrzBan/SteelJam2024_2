using UnityEngine;

public class DualHandItem : IItem
{
    public ItemSO ItemSO;

    public DualHandItem()
    {
       // ItemSO
    }

    public GameObject GetPrefab()
    {
        throw new System.NotImplementedException();
    }

    public void Use(PlayerManager User)
    {
        
    }
}
