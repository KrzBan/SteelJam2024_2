using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class Door : MonoBehaviour, IInteractable
{
    public TMP_Text RoomText;
    
    public RoomType roomType;
    public GameObject keyLock;
    public Transform lockSpawnPoint;
    
    [SerializeField] bool _interactable = false;
    public void interact(Player user)
    {
        if(_interactable == false)
            return;
        
        //if(keyLock != null && user.inventory.ConsumeKey() == false)
        //    return;
        
        if(keyLock != null)
            keyLock.GetComponent<PlayableDirector>().Play();
        
        // spawn new room
        _interactable = false;
        user.canInteract = false;
        RoomManager.Instance.SpawnRoomByType(roomType);

        StartCoroutine(TeleportCoroutine(user, RoomManager.Instance.GetPlayerSpawnPoint()));
    }

    public void Open()
    {
        _interactable = true;
    }
    
    IEnumerator TeleportCoroutine(Player user, Transform spawnPoint)
    {
        if (keyLock != null)
            yield return new WaitForSeconds(2.0f);
        
        float fadeTime = 3.0f;
        float fadeOutTime = 1.0f;
        
        // fade in
        Fade.Instance.Out(fadeTime);
        yield return new WaitForSeconds(fadeTime);
        
        // teleport
        user.Rigidbody.position = spawnPoint.position;
        user.Rigidbody.rotation = spawnPoint.rotation;
        
        // fade out
        Fade.Instance.In(fadeTime);
        yield return new WaitForSeconds(fadeOutTime);
        
        user.canInteract = true;
        RoomManager.Instance.SwapRooms();

        yield return null;
    }
}
