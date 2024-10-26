using UnityEngine;


public interface IItem
{
    void setItemSO(ItemSO itemSO);
    ItemSO getItemSO();
    GameObject GetInHandPrefab();
    GameObject GetWorldPrefab();
    void Use(Player User);
}
