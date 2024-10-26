using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class KeyLockShaft : MonoBehaviour
{
    public void ShootAway()
    {
        var rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.AddForce(transform.right * 0.1f + transform.up * 0.15f , ForceMode.Impulse);
        rb.AddTorque(transform.forward * 0.05f, ForceMode.Impulse);
    }
}
