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
            var objTransform = hit.transform;
            while (objTransform != null)
            {
                var isDamageble = objTransform.TryGetComponent<IDamagable>(out var damagable);

                if (isDamageble)
                {
                    damagable.TakeDamage(ItemSO.Damage);
                    if (damagable is Enemy e)
                    {
                        e.DrawBloodHit(hit.point);
                    }
                }
                
                objTransform = objTransform.parent;
            }
        }

    }
}
