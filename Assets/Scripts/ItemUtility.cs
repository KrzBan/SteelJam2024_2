using System.Collections.Generic;
using UnityEngine;

public class ItemUtility
{
  public static bool ShouldRefuseInteract(Player user, ItemSO Item)
    {

        switch (Item.HandRequirement)
        {
            case HandRequirement.SingeHanded:
                if (!user.PlayerStatus.RightArm && !user.PlayerStatus.RightArm) return true;
                break;
            case HandRequirement.DualHanded:
                if (!user.PlayerStatus.RightArm || !user.PlayerStatus.RightArm) return true;
                break;
        };
        user.DropItem(); 
        return false;
    }

    public static void SingleWieldedAttack(Player User, ItemSO ItemSO)
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

    public static void DualWeaponAttack(Player User, ItemSO ItemSO)
    {

        User.AttackingMoveDebuff = true;
        Vector3 OverlapBoxOffset = new Vector3(0, -0.5f, 0) + (User.transform.forward * 0.5f);
        Debug.Log("MI BEING USED :D ");



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

