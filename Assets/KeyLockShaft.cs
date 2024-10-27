using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class KeyLockShaft : MonoBehaviour
{
    public void ShootAway()
    {
        var rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.AddForce(transform.right * 0.1f + Vector3.up * 0.15f , ForceMode.Impulse);
        rb.AddTorque(transform.up * 0.05f + transform.right * 0.01f, ForceMode.Impulse);
    }
}
