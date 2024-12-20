using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.Mathematics.Geometry;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Transform playerSpawnPoint;
    
    public Transform floorsParent;
    public Transform wallsParent;
    public Transform ceilingsParent;

    public Transform sceneryParent;
    public Transform enemiesParent;
    public Transform interactableParent;
    public Transform vfxParent;
    public Transform lightsParent;
    public Transform doorsParent;

    public List<GameObject> enemyList;

    public Transform GetPlayerSpawnPoint()
    {
        return playerSpawnPoint;
    }
    public void SetEnemyTarget(Transform target)
    {
        var enemies = GetComponentsInChildren<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            enemy.SetTarget(target);
        }
    }

    public int GetEnemiesAlive()
    {
        return GetComponentsInChildren<Enemy>().Length;
    }
    public void BakeNavMesh()
    {
        var navMeshSurfaces = GetComponentsInChildren<NavMeshSurface>();
        foreach(NavMeshSurface navMeshSurface in navMeshSurfaces)
        {
            navMeshSurface.BuildNavMesh();
        }
    }
    public void SetMaterial(Material floorMaterial)
    {
        SetFloorMaterial(floorMaterial);
        SetWallMaterial(floorMaterial);
        SetCeilingMaterial(floorMaterial);
    }
    
    public void InstantiateScenery(List<GameObject> props)
    {
        InstantiateInChildren(props, sceneryParent);
    }
    public void InstantiateEnemies()
    {
        if (enemyList.Count <= 0)
        {
            return;
        }
        
        var i = 0;
        foreach (Transform child in enemiesParent)
        {
            var prefab = enemyList[i % enemyList.Count];
            Instantiate(prefab, child.position, child.rotation, child);
            i++;
        }
    }
    public void InstantiateInteractables(GameObject prefab)
    {
        InstantiateInChildren(prefab, interactableParent);
    }
    public void InstantiateVFX(GameObject prefab)
    {
        InstantiateInChildren(prefab, vfxParent);
    }
    public void InstantiateLights(GameObject prefab)
    {
        InstantiateInChildren(prefab, lightsParent);
    }

    public void InstantiateDoors(GameObject door, GameObject lockKey, RoomInfo leftInfo, RoomInfo middleInfo, RoomInfo rightInfo)
    {
        if (doorsParent == null) return;
        
        if (leftInfo != null)
            InstantiateDoor(door, lockKey, leftInfo, doorsParent.GetChild(0));
        if (middleInfo != null)
            InstantiateDoor(door, lockKey, middleInfo, doorsParent.GetChild(1));
        if (rightInfo != null)
            InstantiateDoor(door, lockKey, rightInfo, doorsParent.GetChild(2));
    }

    private void InstantiateDoor(GameObject door, GameObject lockKey, RoomInfo roomInfo, Transform parent)
    {
        var doorObj = Instantiate(door, parent.transform.position, parent.transform.rotation, parent);
        var doorComp = doorObj.GetComponentInChildren<Door>();
        if(doorComp != null)
        {
            doorComp.roomInfo = roomInfo;
        }

        doorComp.RoomText.text = roomInfo.roomType.ToString();

        if (roomInfo.hasKey)
        {
            var lockObj = Instantiate(lockKey, doorComp.lockSpawnPoint.position, doorComp.lockSpawnPoint.rotation, doorObj.transform);
            doorComp.keyLock = lockObj;
        }
    }

    private void InstantiateInChildren(GameObject prefab, Transform parent)
    {
        if (parent == null) return;
        
        foreach(Transform child in parent)
        {
            Instantiate(prefab, child.position, child.rotation, child);        
        }
    }
    
    private void InstantiateInChildren(List<GameObject> props, Transform parent)
    {
        if (parent == null) return;
        
        foreach(Transform child in parent)
        {
            Instantiate(props[Random.Range(0, props.Count)], child.position, child.rotation, child);        
        }
    }
     
    void SetFloorMaterial(Material material)
    {
        SetMaterialInChildren(floorsParent, material);
    }
    void SetWallMaterial(Material material)
    {
        SetMaterialInChildren(wallsParent, material);
    }
    void SetCeilingMaterial(Material material)
    {
        SetMaterialInChildren(ceilingsParent, material);
    }
    void SetMaterialInChildren(Transform parent, Material material)
    {
        if (parent == null) return;
        
        foreach (Transform child in parent.transform)
        {
            var ren = child.GetComponent<Renderer>();
            if(ren != null)
            {
                ren.material = material;
            }
        }
    }
}
