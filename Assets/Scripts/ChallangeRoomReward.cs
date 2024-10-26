using System;
using UnityEngine;

public class ChallangeRoomReward : MonoBehaviour
{
    [SerializeField] private GameObject KeyPrefab;
    
    private void Start()
    {
        RoomManager.Instance.OnDoorsUnlocked += DropReward;
    }

    private void DropReward()
    {
        Instantiate(KeyPrefab, transform.position, Quaternion.identity);
    }
}
