using UnityEngine;


public interface IItem
{
    ItemSO getItemSO();
    GameObject GetPrefab();
    void Use(Player User);
}
