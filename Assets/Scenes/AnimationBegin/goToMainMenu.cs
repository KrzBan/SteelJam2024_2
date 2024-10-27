using UnityEngine;
using UnityEngine.SceneManagement;

public class goToMainMenu : MonoBehaviour
{
    public void EndOfAnimation()
    {
        SceneManager.LoadScene("Start");
    }
}