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
    private bool noTooltip = false; 

    public string getToolTip()
    {
        if (noTooltip)
        {
            return "";
        }
        
        if (!_interactable)
        {
            return "Locked";
        }

        if (keyLock != null)
        {
            return "You need a key to open this";
        }
        
        return "Press e to move further";
    }

    public void interact(Player user)
    {
        if(_interactable == false)
            return;
        
        //if(keyLock != null && user.inventory.ConsumeKey() == false)
        //    return;

        if (keyLock != null)
        {
            if (Player.Instance.inventory.Keys <= 0)
            {
                return;
            }

            Player.Instance.inventory.Keys--;
            keyLock.GetComponent<PlayableDirector>().Play();
        }

        noTooltip = true;
        
        // spawn new room
        _interactable = false;
        user.canInteract = false;
        user.canMove = false;
        user.Move(Vector2.zero);

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
        
        float fadeTime = 3.2f;
        
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
        user.canMove = true;

        RoomManager.Instance.SwapRooms();

        
        yield return null;
    }
}
