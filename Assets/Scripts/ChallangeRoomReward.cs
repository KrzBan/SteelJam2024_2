using System;
using System.Collections;
using UnityEngine;

public class ChallangeRoomReward : MonoBehaviour
{
    [SerializeField] private GameObject KeyPrefab;
    [SerializeField] private Light _light;
    [SerializeField] private AudioClip keyAppearSFX;
    
    private void Start()
    {
        RoomManager.Instance.OnDoorsUnlocked += DropReward;
    }

    private void DropReward()
    {
        Instantiate(KeyPrefab, transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(keyAppearSFX, transform.position);
        StartCoroutine(EnableLight());
    }

    IEnumerator EnableLight()
    {
        var timer = 3f;
        while (timer > 0f)
        {
            _light.intensity = 3f - timer * 9f;
            timer -= Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
    }
}
