using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAxeItem :  IItem
{
    public ItemSO ItemSO;
    private Vector3 OverlapBoxOffset;

    public FireAxeItem()
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

      
        User.StartCoroutine(Attack(User));

    }
    IEnumerator Attack(Player User)
    {
        User.AttackingMoveDebuff = true;
        OverlapBoxOffset = new Vector3(0, -0.5f, 0) + (User.transform.forward * 0.5f);
        Debug.Log("MI BEING USED :D ");


        yield return new WaitForSeconds(1f);

        var enemies = new HashSet<Enemy>();

        Collider[] hitColliders = Physics.OverlapBox(User.transform.position + OverlapBoxOffset, User.transform.localScale, Quaternion.identity);
        foreach (var col in hitColliders)
        {
            Debug.Log("Hit : " + col.name);

            var objTransform = col.transform;
            while (objTransform != null)
            {
                if (objTransform.TryGetComponent<IDamagable>(out var zombie))
                {
                    if (zombie is Enemy e)
                    {
                        enemies.Add(e);
                    }
                }
                
                objTransform = objTransform.parent;
            }
        }
        
        foreach (var enemy in enemies)
        {
            enemy.TakeDamage(ItemSO.Damage);
        }
        User.AttackingMoveDebuff = false;
    }
}
