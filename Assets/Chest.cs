using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    public void interact(Player user)
    {
        Debug.Log("Im being interacted with");
    }
    public string getToolTip()
    {
        return "xd";
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
