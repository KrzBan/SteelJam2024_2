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

    public GameObject GetPrefab()
    {
        return ItemSO.Prefab;
    }

    public void setItemSO(ItemSO itemSO_)
    {
        ItemSO = itemSO_;
    }

    public void Use(Player User)
    {
        Debug.Log("MI BEING USED :D ");

        RaycastHit hit;
        if (Physics.Raycast(User.headTransform.position, User.headTransform.TransformDirection(Vector3.forward), out hit, ItemSO.Range))
        {
            IDamagable damagable;
            bool isDamageble = hit.collider.TryGetComponent<IDamagable>(out damagable);

            if(isDamageble)
                 damagable.TakeDamage(1);
        }

    }
}
