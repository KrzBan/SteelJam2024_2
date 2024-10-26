using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        Debug.Log("inside");
        SceneManager.LoadScene(sceneName);
        Cursor.Instance.Hide();
    }
}
