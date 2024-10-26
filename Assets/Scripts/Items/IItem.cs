using UnityEngine;


public interface IItem
{
    void setItemSO(ItemSO itemSO);
    ItemSO getItemSO();
    GameObject GetPrefab();
    void Use(Player User);
}
