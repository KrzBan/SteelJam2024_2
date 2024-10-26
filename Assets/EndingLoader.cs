using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingLoader : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
