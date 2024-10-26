using UnityEngine;


public enum HandRequirement
{
    SingeHanded,
    DualHanded
}
[CreateAssetMenu(fileName = "ItemSO", menuName = "Scriptable Objects/ItemSO")]
public class ItemSO : ScriptableObject
{
    GameObject Prefab;
    HandRequirement HandRequirement;
    float Range;
}
