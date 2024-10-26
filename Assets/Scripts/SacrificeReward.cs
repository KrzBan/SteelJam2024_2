using System.Collections.Generic;
using UnityEngine;

public class SacrificeReward : MonoBehaviour
{
    [SerializeField] private List<GameObject> possibleObjects;

    private Collider[] cols;
    private Rigidbody rb;

    public void PlaceObject()
    {
        var obj = Instantiate(possibleObjects[Random.Range(0, possibleObjects.Count)], transform.position,
            Quaternion.identity);
        cols = obj.GetComponentsInChildren<Collider>(true);
        rb = obj.GetComponent<Rigidbody>();
        foreach (var col in cols)
        {
            col.enabled = false;
        }

        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }

    public void MakeObjectPickable()
    {
        foreach (var col in cols)
        {
            col.enabled = true;
        }
        
        if (rb != null)
        {
            rb.isKinematic = false;
        }
    }
}
