using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public enum RoomType
{
    None,
    Normal,
    Shop,
    Sacrifice,
    Boss,
    Challange,
    Special
}

[Serializable]
public class RoomLayer
{
    public List<RoomType> roomTypes;
}
public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance;
    private void Awake()
    {
        Instance = this;
        
        if(roomLayers.Count == 0) 
            Debug.LogError("No room layers defined");
    }

    public List<RoomLayer> roomLayers;
    private int _currentRoomLayer = 0;
    
    public Transform enemyTarget;
    public Transform roomParent;
    
    public Material floorMaterial;
    public Material wallMaterial;
    public Material ceilingMaterial;

    public GameObject sceneryObject;
    public GameObject lightObject;
    public GameObject enemyObject;
    public GameObject doorObject;

    [Header("Rooms")]
    public List<Room> bossRoomTemplates;
    public List<Room> normalRoomTemplates;
    public List<Room> shopRoomTemplates;
    public List<Room> sacrificeRoomTemplates;
    public List<Room> challangeRoomTemplates;
    public List<Room> specialRoomTemplates;

    private Room _currentRoom;
    private Room _newRoom;

    private float _roomOffset = 100.0f;

    public void SpawnRoomByType(RoomType type)
    {
        switch (type)
        {
            case RoomType.Normal:
                SpawnRandomRoom(normalRoomTemplates);
                break;
            case RoomType.Shop:
                SpawnRandomRoom(shopRoomTemplates);
                break;
            case RoomType.Sacrifice:
                SpawnRandomRoom(sacrificeRoomTemplates);
                break;
            case RoomType.Challange:
                SpawnRandomRoom(challangeRoomTemplates);
                break;
            case RoomType.Boss:
                SpawnRandomRoom(bossRoomTemplates);
                break;
            case RoomType.None:
                Debug.LogWarning("Tried spawning room type: None?");
                break;
            case RoomType.Special:
                SpawnRandomRoom(specialRoomTemplates);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    public Transform GetPlayerSpawnPoint()
    {
        return _newRoom.GetPlayerSpawnPoint();
    }
    
    public void SpawnRandomRoom(List<Room> roomTemplates)
    {
        _newRoom = Instantiate( roomTemplates[Random.Range(0, roomTemplates.Count)], 
            new Vector3(_roomOffset, 0.0f, 0f), Quaternion.identity, roomParent);
        _roomOffset += 50.0f;
        
        StartCoroutine(SpawnRandomRoomCouroutine());
    }

    public void DeleteCurrentRoom()
    {
        if(_currentRoom != null)
        {
            Destroy(_currentRoom.gameObject);
        }
    }
    
    IEnumerator SpawnRandomRoomCouroutine()
    {
        yield return null;
        
        _newRoom.SetMaterials(floorMaterial, wallMaterial, ceilingMaterial);
        _newRoom.InstantiateScenery(sceneryObject);
        _newRoom.InstantiateLights(lightObject);

        var roomTypes = roomLayers[_currentRoomLayer].roomTypes;
        if (roomTypes.Count == 3)
        {
            _newRoom.InstantiateDoors(doorObject, roomTypes[0], roomTypes[1], roomTypes[2]);
        } else if (roomTypes.Count() == 1)
        {
            _newRoom.InstantiateDoors(doorObject, RoomType.None, roomTypes[0], RoomType.None);
        }
        else
        {
            Debug.LogError("Layers size must be 3 or 1!");
        }
        ++_currentRoomLayer;
        
        _newRoom.BakeNavMesh();
        
        _newRoom.InstantiateEnemies(enemyObject);
        _newRoom.SetEnemyTarget(enemyTarget);
    }

    public void SwapRooms()
    {
        DeleteCurrentRoom();
        _currentRoom = _newRoom;
        _newRoom = null;
    }
}
