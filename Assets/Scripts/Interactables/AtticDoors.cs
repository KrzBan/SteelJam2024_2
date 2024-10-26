using UnityEngine;
using UnityEngine.SceneManagement;

public class AtticDoors : MonoBehaviour, IInteractable
{
    [SerializeField] private string teleportToSceneName = "SomeScene";
    
    public void interact(Player user)
    {
        Fade.Instance.Out(4f);
        Fade.Instance.OnFadedOut += () => SceneManager.LoadScene(teleportToSceneName);
    }
}
