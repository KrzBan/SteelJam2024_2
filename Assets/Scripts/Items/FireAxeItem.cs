using System.Collections;
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


        Collider[] hitColliders = Physics.OverlapBox(User.transform.position + OverlapBoxOffset, User.transform.localScale, Quaternion.identity);
        int i = 0;
        while (i < hitColliders.Length)
        {
            Debug.Log("Hit : " + hitColliders[i].name + i);

            IDamagable zombie;
            if (hitColliders[i].TryGetComponent<IDamagable>(out zombie))
            {
                zombie.TakeDamage(ItemSO.Damage);
            }
            i++;
        }
        User.AttackingMoveDebuff = false;
    }
}
