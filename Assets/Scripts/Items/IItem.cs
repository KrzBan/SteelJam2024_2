using UnityEngine;

public class PlayerManager { };
public interface IItem
{
    GameObject GetPrefab();
    void Use(PlayerManager User);
}
