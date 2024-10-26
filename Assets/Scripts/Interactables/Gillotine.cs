using System;
using System.Collections;
using UnityEngine;

public class Gillotine : AnimationInteractableBase
{
    [SerializeField] private SacrificeReward sacrificeReward;
    [SerializeField] private Collider triggerCollider;

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
        triggerCollider.enabled = false;
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

    public override void interact(Player user)
    {
        if (Player.Instance == null)
        {
            return;
        }

        if (interacted && !interactableMultipleTimes)
        {
            return;
        }

        Player.Instance.HideArm();

        base.interact(user);
        
        Player.Instance.RemoveLimbOrdered();
    }
}
