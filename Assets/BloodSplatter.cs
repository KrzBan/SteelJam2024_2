using UnityEngine;
using UnityEngine.VFX;

public class BloodSplatter : MonoBehaviour
{
    public VisualEffect blood;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartBlood()
    {
        blood.gameObject.SetActive(true);
    }
    
    public void StopBlood()
    {
        blood.gameObject.SetActive(false);
    }
}
