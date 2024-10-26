using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class UIController : MonoBehaviour
{
    private bool eqShown = false;
    
    public void ShowEquipment(InputAction.CallbackContext context)
    {
        if (!context.started)
        {
            return;
        }

        if (UI.Instance != null)
        {
            if (!eqShown)
            {
                UI.Instance.ShowUI("equipment_ui");
                eqShown = true;
            }
            else
            {
                UI.Instance.HideUI();
                eqShown = false;
            }
        }
    }

    public void Hide(InputAction.CallbackContext context)
    {
        if (!context.started)
        {
            return;
        }
        
        if (UI.Instance == null)
        {
            return;
        }
        
        UI.Instance.HideUI();

        eqShown = false;
    }
}
