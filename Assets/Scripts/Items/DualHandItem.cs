using UnityEngine;

public class DualHandItem :  IItem
{
    public ItemSO ItemSO;

    public DualHandItem()
    {
       
    }

    public ItemSO getItemSO()
    {
        return ItemSO;
    }

    public GameObject GetInHandPrefab()
    {
        return ItemSO.InHandPrefab;
    }
    public GameObject GetWorldPrefab()
    {
        return ItemSO.InHandPrefab;
    }
    public void setItemSO(ItemSO itemSO_)
    {
        ItemSO = itemSO_;
    }

    public void Use(Player User)
    {
        ItemUtility.SingleWieldedAttack(User, ItemSO);
    }
}
