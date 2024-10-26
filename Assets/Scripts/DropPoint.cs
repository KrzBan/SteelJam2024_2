using UnityEngine;

public class DropPoint : MonoBehaviour
{
    [SerializeField] private GameObject objectToDrop;

    public void Drop()
    {
        Instantiate(objectToDrop, transform.position, Quaternion.identity);
    }
}
