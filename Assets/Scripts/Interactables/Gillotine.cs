using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gillotine : AnimationInteractableBase
{
    [SerializeField] private SacrificeReward sacrificeReward;
    [SerializeField] private Collider triggerCollider;

    public override string getToolTip()
    {
        if (!CanUse())
        {
            return "You cannot use gillotine without hands!";
        }
        
        return "Use to lose a hand and gain an item";
    }
    private void Awake()
    {
        OnAnimationEnded += OnEnd;
    }

    private void Start()
    {
        sacrificeReward.PlaceObject();
    }

    private void OnDestroy()
    {
        OnAnimationEnded -= OnEnd;
    }

    private void OnEnd()
    {
        sacrificeReward.MakeObjectPickable();
        if (Player.Instance == null)
        {
            return;
        }

        StartCoroutine(IShowHand());
    }

    IEnumerator IShowHand()
    {
        yield return new WaitForSeconds(1.5f);
        Player.Instance.ShowArm();
    }

    public bool CanUse()
    {
        return Player.Instance.PlayerStatus.LeftArm || Player.Instance.PlayerStatus.RightArm;
    }

    public override void interact(Player user)
    {
        if (Player.Instance == null)
        {
            return;
        }

        if (!CanUse())
        {
            return;
        }

        if (interacted && !interactableMultipleTimes)
        {
            return;
        }
        
        triggerCollider.enabled = false;

        if (Player.Instance.PlayerStatus.LeftArm)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }

        Player.Instance.HideArm();

        base.interact(user);
        
        Player.Instance.RemoveArm();
    }
}
