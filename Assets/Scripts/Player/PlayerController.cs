using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    private Player player;
    
    private void Awake()
    {
        player = GetComponent<Player>();
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            player.Move(Vector2.zero);
        }
        
        var val = context.ReadValue<Vector2>();
        
        player.Move(val);
    }

    public void Look(InputAction.CallbackContext context)
    {
        var val = context.ReadValue<Vector2>();
        
        player.Look(val);
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if(context.started)
            player.Interact();
    }
}
