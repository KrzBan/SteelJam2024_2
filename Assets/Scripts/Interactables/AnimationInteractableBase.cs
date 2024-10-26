using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimationInteractableBase : MonoBehaviour, IInteractable
{
    public Action OnAnimationEnded { get; set; }
    
    [SerializeField] protected GameObject objectCamera;
    [SerializeField] protected List<Animator> animatorsToPlay;
    [SerializeField] protected List<GameObject> objectsToEnable;
    [SerializeField] protected List<GameObject> objectsToDisable;
    [SerializeField] protected float timeToAnimationEnd = 4f;
    [SerializeField] protected float timeToAnimationStart = 3f;
    [SerializeField] protected bool interactableMultipleTimes = false;

    protected bool interacted = false;

    public virtual void interact(Player user)
    {
        if (interacted && !interactableMultipleTimes)
        {
            return;
        }

        interacted = true;
        
        StartCoroutine(IStartAnimation());
    }

    IEnumerator IStartAnimation()
    {
        objectCamera.SetActive(true);
        
        yield return new WaitForSeconds(timeToAnimationStart);
        
        foreach (var obj in objectsToEnable)
        {
            obj.SetActive(true);
        }
        
        foreach (var animator in animatorsToPlay)
        {
            animator.enabled = true;
        }

        objectCamera.GetComponent<Animator>().enabled = true;
        
        StartCoroutine(IEndAnimation());
    }

    IEnumerator IEndAnimation()
    {
        yield return new WaitForSeconds(timeToAnimationEnd);
        
        foreach (var animator in animatorsToPlay)
        {
            animator.enabled = false;
        }

        foreach (var obj in objectsToDisable)
        {
            obj.SetActive(false);
        }
        
        objectCamera.GetComponent<Animator>().enabled = false;
        objectCamera.SetActive(false);
        
        OnAnimationEnded?.Invoke();
    }
}
