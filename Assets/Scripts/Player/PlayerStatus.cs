using System;
using UnityEngine;

public class LimbState
{
    public bool Head { get; }
    public bool LeftArm { get; }
    public bool RightArm { get; }
    public bool LeftLeg { get; }
    public bool RightLeg { get; }

    public LimbState(bool head, bool lArm, bool rArm, bool lLeg, bool rLeg)
    {
        Head = head;
        LeftArm = lArm;
        RightArm = rArm;
        LeftLeg = lLeg;
        RightLeg = rLeg;
    }
}

[Serializable]
public class PlayerStatus
{
    public Action<LimbState> OnLimbStateChanged { get; set; }
    public Action<float> OnHealthReduced { get; set; }
    public Action<float> OnHealthGained { get; set; }
    public Action OnDeath { get; set; }

    public bool Head
    {
        get => head;
        set
        {
            head = value;
            OnLimbStateChanged?.Invoke(new LimbState(head, lArm, rArm, lLeg, rLeg));
        }
    }
    public bool LeftArm
    {
        get => lArm;
        set
        {
            lArm = value;
            OnLimbStateChanged?.Invoke(new LimbState(head, lArm, rArm, lLeg, rLeg));
        }
    }
    public bool RightArm
    {
        get => rArm;
        set
        {
            rArm = value;
            OnLimbStateChanged?.Invoke(new LimbState(head, lArm, rArm, lLeg, rLeg));
        }
    }
    public bool LeftLeg
    {
        get => lLeg;
        set
        {
            lLeg = value;
            OnLimbStateChanged?.Invoke(new LimbState(head, lArm, rArm, lLeg, rLeg));
        }
    }
    public bool RightLeg
    {
        get => rLeg;
        set
        {
            head = value;
            OnLimbStateChanged?.Invoke(new LimbState(head, lArm, rArm, lLeg, rLeg));
        }
    }

    public float Health
    {
        get => health;
        set
        {
            if (value > health)
            {
                health = value;
                OnHealthGained?.Invoke(health);
            }

            if (value < health && value > 0f)
            {
                health = value;
                OnHealthReduced?.Invoke(health);
            }

            if (value <= 0f)
            {
                health = 0f;
                OnDeath?.Invoke();
            }
            
        }
    }

    private bool head = true;
    private bool lArm = true;
    private bool rArm = true;
    private bool lLeg = true;
    private bool rLeg = true;
    private float health = 100f;
}
