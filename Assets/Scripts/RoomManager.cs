using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public enum RoomType
{
    Normal,
    Heal,
    Sacrifice,
    Boss,
    Challange,
    Special,
    Treasure
}

[Serializable]
public class RoomInfo
{
    public RoomType roomType;
    public bool hasKey;
}

[Serializable]
public class RoomLayer
{
    public List<RoomInfo> roomTypes;
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
    
    public List<Material> roomMaterials;
    public Material ceilingMaterial;

    public List<GameObject> propObjects;
    public GameObject lightObject;
    public GameObject enemyObject;
    public GameObject doorObject;
    public GameObject keyObject;
    
    [Header("Rooms")]
    public List<Room> bossRoomTemplates;
    public List<Room> normalRoomTemplates;
    public List<Room> healRoomTemplates;
    public List<Room> sacrificeRoomTemplates;
    public List<Room> challangeRoomTemplates;
    public List<Room> specialRoomTemplates;
    public List<Room> treasureRoomTemplates;

    private Room _currentRoom;
    private Room _newRoom;

    private float _roomOffset = 100.0f;
    private int _enemiesAlive = 0;

    public void SpawnRoomByType(RoomType type)
    {
        switch (type)
        {
            case RoomType.Normal:
                SpawnRandomRoom(normalRoomTemplates);
                break;
            case RoomType.Heal:
                SpawnRandomRoom(healRoomTemplates);
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
            case RoomType.Special:
                SpawnRandomRoom(specialRoomTemplates);
                break;
            case RoomType.Treasure:
                SpawnRandomRoom(treasureRoomTemplates);
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
        
        _newRoom.InstantiateEnemies(enemyObject);
    }

    public void SwapRooms()
    {
        DeleteCurrentRoom();
        _currentRoom = _newRoom;
        _newRoom = null;
        
        _currentRoom.SetEnemyTarget(Player.Instance.transform);
        _enemiesAlive = _currentRoom.GetEnemiesAlive();
        if(_enemiesAlive == 0) UnlockDoors();
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
    }
}
