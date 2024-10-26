using UnityEngine;


public enum HandRequirement
{
    SingeHanded,
    DualHanded
}
[CreateAssetMenu(fileName = "ItemSO", menuName = "Scriptable Objects/ItemSO")]
public class ItemSO : ScriptableObject
{
    public GameObject InHandPrefab;
    public GameObject WorldPrefab;

    public HandRequirement HandRequirement;
    public float Range;
    public float Damage = 1f;
}
