using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TreasureRoomReward : MonoBehaviour
{
    [SerializeField] private List<GameObject> rewards;

    private void Start()
    {
        PlaceRandomReward();
    }

    private void PlaceRandomReward()
    {
        var obj = rewards[Random.Range(0, rewards.Count)];
        Instantiate(obj, transform.position, Quaternion.identity);
    }
}
