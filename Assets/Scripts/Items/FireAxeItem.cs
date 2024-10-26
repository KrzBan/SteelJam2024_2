using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAxeItem : IItem
{
    public ItemSO ItemSO;

    public FireAxeItem()
    {

    }

    public GameObject GetInHandPrefab()
    {
        return ItemSO.InHandPrefab;

    }

    public ItemSO getItemSO()
    {
        return ItemSO;
    }

    

    public GameObject GetWorldPrefab()
    {
        return ItemSO.WorldPrefab;
    }

    public void setItemSO(ItemSO itemSO_)
    {
        ItemSO = itemSO_;
    }

    public void Use(Player User)
    {
        ItemUtility.DualWeaponAttack(User, ItemSO);
    }
   
}