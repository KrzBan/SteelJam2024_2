using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public RoomType roomType;

    private bool _interactable = true;
    public void interact(Player user)
    {
        if(_interactable == false)
            return;
        
        Debug.Log("Door interacted with");
        
        // check if player has key
        
        // spawn new room
        _interactable = false;
        user.canInteract = false;
        RoomManager.Instance.SpawnRoomByType(roomType);

        StartCoroutine(TeleportCoroutine(user, RoomManager.Instance.GetPlayerSpawnPoint()));
    }
    
    IEnumerator TeleportCoroutine(Player user, Transform spawnPoint)
    {
        float fadeTime = 3.0f;
        
        // fade in
        Fade.Instance.Out(fadeTime);
        yield return new WaitForSeconds(fadeTime);
        
        // teleport
        user.Rigidbody.position = spawnPoint.position;
        user.Rigidbody.rotation = spawnPoint.rotation;
        
        // fade out
        Fade.Instance.In(fadeTime);
        yield return new WaitForSeconds(fadeTime);
        
        user.canInteract = true;
        RoomManager.Instance.SwapRooms();

        yield return null;
    }
}
