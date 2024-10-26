using System;
using UnityEngine;

[Serializable]
public class PlayerStatus
{
    public Action<bool> OnHeadStateChanged { get; set; }
    public Action<bool> OnLeftArmStateChanged { get; set; }
    public Action<bool> OnRightArmStateChanged { get; set; }
    public Action<bool> OnLeftLegStateChanged { get; set; }
    public Action<bool> OnRightLegStateChanged { get; set; }
    
    public bool Head
    {
        get => head;
        set
        {
            head = value;
            OnHeadStateChanged?.Invoke(value);
        }
    }
    public bool LeftArm
    {
        get => lArm;
        set
        {
            lArm = value;
            OnLeftArmStateChanged?.Invoke(value);
        }
    }
    public bool RightArm
    {
        get => rArm;
        set
        {
            rArm = value;
            OnRightArmStateChanged?.Invoke(value);
        }
    }
    public bool LeftLeg
    {
        get => lLeg;
        set
        {
            lLeg = value;
            OnLeftLegStateChanged?.Invoke(value);
        }
    }
    public bool RightLeg
    {
        get => rLeg;
        set
        {
            head = value;
            OnRightLegStateChanged?.Invoke(value);
        }
    }

    private bool head = true;
    private bool lArm = true;
    private bool rArm = true;
    private bool lLeg = true;
    private bool rLeg = true;
}
