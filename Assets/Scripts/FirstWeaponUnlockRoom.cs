using System;
using UnityEngine;

public class FirstWeaponUnlockRoom : MonoBehaviour
{
    [SerializeField] private Door door;

    private void OnDestroy()
    {
        door.Open();
    }
}
