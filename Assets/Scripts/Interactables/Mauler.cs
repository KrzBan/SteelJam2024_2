using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mauler : AnimationInteractableBase
{
    [SerializeField] private SacrificeReward sacrificeReward;
    [SerializeField] private Collider triggerCollider;
    [SerializeField] private List<Transform> blades;
    [SerializeField] private bool spin;
    [SerializeField] private float spinSpeed;

    private void Awake()
    {
        OnAnimationEnded += OnEnd;
    }

    void Update()
    {
        if (spin)
        {
            blades[0].Rotate(Vector3.up, spinSpeed * Time.deltaTime, Space.Self);
            blades[1].Rotate(Vector3.up, -spinSpeed * Time.deltaTime, Space.Self);
        }
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
    
    public override string getToolTip()
    {
        if (!CanUse())
        {
            return "You cannot use mauler without legs!";
        }
        
        return "Sacrifice a leg for a weapon";
    }
    
    public bool CanUse()
    {
        return Player.Instance.PlayerStatus.LeftLeg || Player.Instance.PlayerStatus.RightLeg;
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
        
        Player.Instance.RemoveLeg();
    }
    
    IEnumerator IShowHand()
    {
        yield return new WaitForSeconds(1.5f);
        Player.Instance.ShowArm();
    }

    public void StartMauler()
    {
        spin = true;
    }
    public void StopMauler()
    {
        Debug.Log("WTF");
        spin = false;
    }
}
