using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToMainMenu : MonoBehaviour
{
    public void EndOfAnimation()
    {
        Debug.Log("dupa");
        SceneManager.LoadScene("MainMenu");
    }
}