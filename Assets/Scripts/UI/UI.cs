using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private List<UIElementBase> uis;
        
    [CanBeNull] public static UI Instance;

    [CanBeNull] private UIElementBase currentUI;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowUI(string _name)
    {
        foreach (var ui in uis)
        {
            if (ui.Name == _name)
            {
                ui.Show();
                currentUI = ui;
            }
        }
    }

    public void HideUI()
    {
        if (currentUI != null)
        {
            currentUI.Hide();
        }
    }
}
