using UnityEngine;

public class GlutTouchHandler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Player")
        {
            Player.Instance.animator.SetBool("OnGlutEnter",true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            Player.Instance.animator.SetBool("OnGlutEnter", false);
            Player.Instance.Bubbles.SetActive(false);   
        }
    }

}
