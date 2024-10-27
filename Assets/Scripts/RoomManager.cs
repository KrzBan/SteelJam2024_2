using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public enum RoomType
{
    Start,
    Normal,
    Heal,
    Sacrifice,
    Boss,
    Challange,
    Special,
    Treasure,
    Slime
}

[Serializable]
public class RoomInfo
{
    public RoomType roomType;
    public int roomIndexFromList;
    public bool hasKey;
}

[Serializable]
public class RoomLayer
{
    public List<RoomInfo> roomTypes;
}
public class RoomManager : MonoBehaviour
{
    public Action OnDoorsUnlocked { get; set; }
    
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
    
    public List<Material> roomMaterials;
    public Material ceilingMaterial;

    public List<GameObject> propObjects;
    public GameObject lightObject;
    public GameObject enemyObject;
    public GameObject doorObject;
    public GameObject keyObject;
    
    [Header("Rooms")]
    public List<Room> startRoomTemplates;
    public List<Room> bossRoomTemplates;
    public List<Room> normalRoomTemplates;
    public List<Room> healRoomTemplates;
    public List<Room> sacrificeRoomTemplates;
    public List<Room> challangeRoomTemplates;
    public List<Room> specialRoomTemplates;
    public List<Room> treasureRoomTemplates;
    public List<Room> slimeRoomTemplates;

    private Room _currentRoom;
    private Room _newRoom;

    private float _roomOffset = 100.0f;
    private int _enemiesAlive = 0;

    public void SpawnRoom(RoomInfo info)
    {
        switch (info.roomType)
        {
            case RoomType.Start:
                SpawnRoom(startRoomTemplates, info);
                break;
            case RoomType.Normal:
                SpawnRoom(normalRoomTemplates, info);
                break;
            case RoomType.Heal:
                SpawnRoom(healRoomTemplates, info);
                break;
            case RoomType.Sacrifice:
                SpawnRoom(sacrificeRoomTemplates, info);
                break;
            case RoomType.Challange:
                SpawnRoom(challangeRoomTemplates, info);
                break;
            case RoomType.Boss:
                SpawnRoom(bossRoomTemplates, info);
                break;
            case RoomType.Special:
                SpawnRoom(specialRoomTemplates, info);
                break;
            case RoomType.Treasure:
                SpawnRoom(treasureRoomTemplates, info);
                break;
            case RoomType.Slime:
                SpawnRoom(slimeRoomTemplates, info);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    public Transform GetPlayerSpawnPoint()
    {
        return _newRoom.GetPlayerSpawnPoint();
    }
    
    public void SpawnRoom(List<Room> roomTemplates, RoomInfo info)
    {
        _newRoom = Instantiate( roomTemplates[info.roomIndexFromList], 
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
        
        _newRoom.SetMaterial(roomMaterials[Random.Range(0, roomMaterials.Count)]);
        _newRoom.InstantiateScenery(propObjects);
        _newRoom.InstantiateLights(lightObject);

        var roomTypes = roomLayers[_currentRoomLayer].roomTypes;
        if (roomTypes.Count == 3)
        {
            _newRoom.InstantiateDoors(doorObject, keyObject, roomTypes[0], roomTypes[1], roomTypes[2]);
        } 
        else if (roomTypes.Count == 2)
        {
            _newRoom.InstantiateDoors(doorObject, keyObject, roomTypes[0], roomTypes[1], null);
        }
        else if (roomTypes.Count() == 1)
        {
            _newRoom.InstantiateDoors(doorObject, keyObject, null, roomTypes[0], null);
        }
        else
        {
            Debug.LogError("Layers size must be 3 or 1!");
        }
        ++_currentRoomLayer;
        
        _newRoom.BakeNavMesh();
        
        _newRoom.InstantiateEnemies();
    }

    public void SwapRooms()
    {
        DeleteCurrentRoom();
        _currentRoom = _newRoom;
        _newRoom = null;
        
        _currentRoom.SetEnemyTarget(Player.Instance.transform);
        _enemiesAlive = _currentRoom.GetEnemiesAlive();
        Debug.Log(_currentRoomLayer);
        if(_enemiesAlive == 0 && _currentRoomLayer != 1) UnlockDoors(); // Dont unlock starting doors
    }

    public void OnEnemyDeath()
    {
        if (_enemiesAlive > 0) --_enemiesAlive;

        if (_enemiesAlive == 0)
        {
            UnlockDoors();
        }
    }

    private void UnlockDoors()
    {
        var objs = FindObjectsByType<Door>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        foreach (var door in objs)
        {
            door.Open();
        }
        
        OnDoorsUnlocked?.Invoke();
        OnDoorsUnlocked = null;
    }
}
