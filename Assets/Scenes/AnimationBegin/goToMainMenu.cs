using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToMainMenu : MonoBehaviour
{
    public void EndOfAnimation()
    {
        SceneManager.LoadScene("Start");
    }
}