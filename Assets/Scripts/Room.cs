using Unity.AI.Navigation;
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
    public void BakeNavMesh()
    {
        var navMeshSurfaces = GetComponentsInChildren<NavMeshSurface>();
        foreach(NavMeshSurface navMeshSurface in navMeshSurfaces)
        {
            navMeshSurface.BuildNavMesh();
        }
    }
    public void SetMaterials(Material floorMaterial, Material wallMaterial, Material ceilingMaterial)
    {
        SetFloorMaterial(floorMaterial);
        SetWallMaterial(wallMaterial);
        SetCeilingMaterial(ceilingMaterial);
    }
    
    public void InstantiateScenery(GameObject prefab)
    {
        InstantiateInChildren(prefab, sceneryParent);
    }
    public void InstantiateEnemies(GameObject prefab)
    {
        InstantiateInChildren(prefab, enemiesParent);
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

    public void InstantiateDoors(GameObject door, RoomType leftType, RoomType middleType, RoomType rightType)
    {
        if (leftType != RoomType.None)
            InstantiateDoor(door, leftType, doorsParent.GetChild(0));
        if (middleType != RoomType.None)
            InstantiateDoor(door, middleType, doorsParent.GetChild(1));
        if (rightType != RoomType.None)
            InstantiateDoor(door, rightType, doorsParent.GetChild(2));
    }

    private void InstantiateDoor(GameObject door, RoomType roomType, Transform parent)
    {
        var doorObj = Instantiate(door, parent.transform.position, parent.transform.rotation, parent);
        var doorComp = doorObj.GetComponentInChildren<Door>();
        if(doorComp != null)
        {
            doorComp.roomType = roomType;
        }
    }

    private void InstantiateInChildren(GameObject prefab, Transform parent)
    {
        foreach(Transform child in parent)
        {
            Instantiate(prefab, child.position, child.rotation, child);        
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
