using UnityEngine;

public abstract class UIElementBase : MonoBehaviour
{
    public string Name;

    public abstract void Show();
    public abstract void Hide();
}
