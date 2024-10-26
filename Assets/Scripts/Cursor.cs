using UnityEngine;

public class Cursor
{
    public static Cursor Instance;
    
    static Cursor()
    {
        Instance = new Cursor();
    }

    public void Hide()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
    }

    public void Show()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
    }
}
