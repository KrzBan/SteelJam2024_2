using System;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour, IDamagable
{
    [field: SerializeField] public PlayerStatus PlayerStatus { get; set; }
    public float MaxHealth => maxHealth;
    
    [SerializeField] public Inventory inventory;
    public Rigidbody Rigidbody => rb;
    [SerializeField, Range(0f, 1f)] private float sensitivity = 0.05f;
    [SerializeField] public Transform headTransform;
    [SerializeField] private float movementSpeed = 1f;
    [SerializeField] private Arm rArm;
    [SerializeField] private Arm doubleArm;
    [SerializeField] private Transform handSlot;
    [SerializeField] private Transform twoHandSlot;
    [SerializeField] private GameObject armObject;
    [SerializeField] private GameObject doubleArmObject;
    [SerializeField] private GameObject legDecap;
    [SerializeField] private GameObject armDecap;
    [SerializeField] private float dashForce = 500f;
    [SerializeField] private float dashCooldown = 2f;
    [SerializeField] private float interactRange = 1.5f;
    [SerializeField] private float maxHealth = 10;
    [SerializeField] public GameObject Bubbles;
    [SerializeField] private AudioClip woshSFX;
    [SerializeField] private GameObject ragdollParent;
    [SerializeField] private Transform head;
    [CanBeNull] public static Player Instance { get; private set; }
    public Animator animator;

    private Vector2 direction;
    private Rigidbody rb;
    private float lastDash = 0f;
    private TMPro.TMP_Text ToolTipTMP;

    private float movementSpeedMultplier = 1;
    public bool canInteract = true;
    public bool canMove = true;
    public bool AttackingMoveDebuff = false;
    public bool PropelUp = false;


    private void Awake()
    {
        Instance = this;
        inventory = new Inventory();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        Cursor.Instance.Hide();
        PlayerStatus.OnLimbStateChanged += OnLimbLoss;
        
        PlayerStatus.OnDeath += Ragdoll;

        PlayerStatus.Health = maxHealth;
    }

    public void Ragdoll()
    {
        rb.isKinematic = true;
        canInteract = false;
        canMove = false;
        headTransform.parent = head;
        headTransform.localPosition = Vector3.zero;
        ragdollParent.SetActive(true);
        GetComponent<CapsuleCollider>().enabled = false;
        foreach (var r in ragdollParent.GetComponentsInChildren<Rigidbody>())
        {
            r.isKinematic = false;
        }
    }

    public void Wosh()
    {
        AudioSource.PlayClipAtPoint(woshSFX, transform.position);
    }
    
    public void SetGlutPartices()
    {
        Bubbles.SetActive(true);
    }

    public void HealLimbs()
    {
        PlayerStatus.Head = true;
        PlayerStatus.LeftArm = true;
        PlayerStatus.LeftLeg  = true;
        PlayerStatus.RightArm = true;
        PlayerStatus.RightLeg = true;
        PlayerStatus.Health = 10;


    }
    private void Start()
    {
        ToolTipTMP = GameObject.FindGameObjectWithTag("Tooltip").GetComponent<TMP_Text>();
        
        ShowArm();
    }

    private void FixedUpdate()
    {
        EvaluateMovement();
    }

    public void HideArm()
    {
        armObject.SetActive(false);
        doubleArmObject.SetActive(false);
    }

    public void ShowArm()
    {
        if (inventory.ItemSlot != null && inventory.ItemSlot.getItemSO().HandRequirement == HandRequirement.DualHanded &&
            PlayerStatus.RightArm && PlayerStatus.LeftArm)
        {
            doubleArmObject.SetActive(true);
            return;
        }
        
        if (PlayerStatus.RightArm)
        {
            armObject.SetActive(true);
        }
    }
    
    public void Move(Vector2 _direction)
    {
        direction = _direction;
    }

    public void Dash()
    {
        if (!PlayerStatus.RightLeg || !PlayerStatus.LeftLeg)
        {
            return;
        }
        
        if (Time.time - lastDash < dashCooldown)
        {
            return;
        }

        lastDash = Time.time;
        
        if (direction == Vector2.zero)
        {
            return;
        }
        
        var forward = headTransform.forward;
        forward.y = 0f;
        forward.Normalize();

        var right = headTransform.right;
        right.y = 0f;
        right.Normalize();
        
        rb.AddForce((forward * direction.y + right * direction.x) * dashForce);
    }

    public void Look(Vector2 delta)
    {
        if (headTransform.eulerAngles.x - delta.y < 270f && headTransform.eulerAngles.x >= 270f)
        {
            var angles = headTransform.eulerAngles;
            angles.x = 270f;
            headTransform.eulerAngles = angles;
        }

        if (headTransform.eulerAngles.x - delta.y > 90f && headTransform.eulerAngles.x <= 90f)
        {
            var angles = headTransform.eulerAngles;
            angles.x = 90f;
            headTransform.eulerAngles = angles;
        }
        
        headTransform.RotateAround(headTransform.position, Vector3.up, delta.x * sensitivity);
        headTransform.RotateAround(headTransform.position, headTransform.right, -delta.y * sensitivity);

        doToolTip();
    }
    void doToolTip()
    {
        RaycastHit hit;
        if (Physics.Raycast(headTransform.position, headTransform.TransformDirection(Vector3.forward), out hit, interactRange))
        {
             var isInteractable = hit.collider.TryGetComponent<IInteractable>(out var Interactable);
             if (isInteractable)
             {
                if(ToolTipTMP != null)
                    ToolTipTMP.text = Interactable.getToolTip();
             }
             else 
             {
                if (ToolTipTMP != null)
                    ToolTipTMP.text = "";
             }

        }
        else 
        {
            if (ToolTipTMP != null)
                ToolTipTMP.text = "";
        }

    }
    void OnLimbLoss(LimbState status)
    {
        if (!status.Head) PlayerStatus.OnDeath?.Invoke();
        movementSpeedMultplier = 0.4f;
        if (status.LeftLeg) movementSpeedMultplier += 0.3f;
        if (status.RightLeg) movementSpeedMultplier += 0.3f;


        if (inventory.ItemSlot == null) return;
        if (!(status.LeftArm || status.RightArm))
        {
            DropItem();
            HideArm();
            ShowArm();
        }


        if(!(status.LeftArm && status.RightArm))
        {
            if (inventory.ItemSlot != null && inventory.ItemSlot.getItemSO().HandRequirement == HandRequirement.DualHanded)
            {
                DropItem();
                HideArm();
                ShowArm();
            }
        }
    }
    public void DropItem()
    {

        if (handSlot.childCount != 0)
            Destroy(handSlot.GetChild(0).gameObject);

        if (inventory.ItemSlot == null) { Debug.Log("Nothin to drop"); return; }

       var DroppedItem = Instantiate(inventory.ItemSlot.GetWorldPrefab(), handSlot);
        DroppedItem.transform.localPosition = new Vector3();
        DroppedItem.transform.SetParent(null);
        inventory.ItemSlot = null;
    }
    public void Hit(HitParams hitParams)
    {
        TakeDamage(hitParams.Damage);
        if (Random.Range(0f, 1f) <= hitParams.LibLossChance)
        {
            RemoveLimbOrdered();
        }
    }

    public void RemoveArm()
    {
        if (PlayerStatus.LeftArm)
        {
            PlayerStatus.LeftArm = false;
        }
        else if (PlayerStatus.RightArm)
        {
            PlayerStatus.RightArm = false;
        }
    }
    
    public void RemoveLeg()
    {
        if (PlayerStatus.LeftLeg)
        {
            PlayerStatus.LeftLeg = false;
        }
        else if (PlayerStatus.RightLeg)
        {
            PlayerStatus.RightLeg = false;
        }
    }

    /// <summary>
    /// Order - left arm -> left leg -> right leg -> right arm
    /// </summary>
    public void RemoveLimbOrdered()
    {
        if (PlayerStatus.LeftArm)
        {
            PlayerStatus.LeftArm = false;
            Instantiate(armDecap, transform.position + Vector3.up, Quaternion.identity);
        }
        else if (PlayerStatus.LeftLeg)
        {
            PlayerStatus.LeftLeg = false;
            Instantiate(legDecap, transform.position + Vector3.up, Quaternion.identity);
        }
        else if (PlayerStatus.RightLeg)
        {
            PlayerStatus.RightLeg = false;
            Instantiate(legDecap, transform.position + Vector3.up, Quaternion.identity);
        }
        else if (PlayerStatus.RightArm)
        {
            PlayerStatus.RightArm = false;
            Instantiate(armDecap, transform.position + Vector3.up, Quaternion.identity);
        }
    }

    private void EvaluateMovement()
    {
        if (Time.time - lastDash < 0.6f)
        {
            rb.linearVelocity -= Time.fixedDeltaTime * rb.linearVelocity * 5f;
            return;
        }
        
        var forward = headTransform.forward;
        forward.y = 0f;
        forward.Normalize();

        var right = headTransform.right;
        right.y = 0f;
        right.Normalize();

        float upBoost = 0;
        if(PropelUp)
        {
            upBoost =2.5f;
        }

        var velo = (forward * direction.y + right * direction.x) * movementSpeed + (upBoost +  rb.linearVelocity.y) * Vector3.up;
        velo *= movementSpeedMultplier;
        if (AttackingMoveDebuff) velo *= 0.3f;
        rb.linearVelocity = velo;
    }


    public void Interact()
    {
        int keyLockLayer = LayerMask.NameToLayer("KeyLock");
        int layerMask = 1 << keyLockLayer;
        layerMask = ~layerMask;
        
        RaycastHit hit;
        if (Physics.Raycast(headTransform.position, headTransform.TransformDirection(Vector3.forward), out hit, interactRange, layerMask))
        {
            IInteractable interactable;
            bool isInteractable = hit.collider.TryGetComponent<IInteractable>(out interactable );

            if(isInteractable)
            {
                interactable.interact(this);
            }
        }
        
    }

    public void PlaceInHand(ItemSO item)
    {
        if (handSlot.childCount > 0)
        {
            Destroy(handSlot.GetChild(0).gameObject);
        }

        if (twoHandSlot.childCount > 0)
        {
            Destroy(twoHandSlot.GetChild(0).gameObject);
        }

        if (item.HandRequirement == HandRequirement.SingeHanded)
        {
            Instantiate(item.InHandPrefab, handSlot);
        }
        else if (item.HandRequirement == HandRequirement.DualHanded)
        {
            Instantiate(item.InHandPrefab, twoHandSlot);
        }
        
        HideArm();
        ShowArm();
    }

    public void Use()
    {
        if (!PlayerStatus.RightArm)
            return;
        
        if (inventory.ItemSlot == null) return;
        switch (inventory.ItemSlot.getItemSO().HandRequirement)
        {
            case HandRequirement.SingeHanded:
                rArm.Attack("Attack");

                break;
            case HandRequirement.DualHanded:
                doubleArm.Attack("Attack");

                break;
        }

    }

    public void TakeDamage(float value)
    {
        PlayerStatus.Health -= value;
    }
}
