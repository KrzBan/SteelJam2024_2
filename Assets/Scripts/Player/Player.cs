using System;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour, IDamagable
{
    [field: SerializeField] public PlayerStatus PlayerStatus { get; set; }
    [SerializeField] public Inventory inventory;
    public Rigidbody Rigidbody => rb;
    [SerializeField, Range(0f, 1f)] private float sensitivity = 0.05f;
    [SerializeField] public Transform headTransform;
    [SerializeField] private float movementSpeed = 1f;
    [SerializeField] private Arm rArm;
    [SerializeField] private Transform handSlot;
    [SerializeField] private GameObject armObject;
    [SerializeField] private GameObject legDecap;
    [SerializeField] private GameObject armDecap;
    [SerializeField] private float dashForce = 500f;
    [SerializeField] private float dashCooldown = 2f;
    [SerializeField] private TMPro.TMP_Text ToolTipTMP;
    [CanBeNull] public static Player Instance { get; private set; }

    private Vector2 direction;
    private Rigidbody rb;
    private float lastDash = 0f;

    private float movementSpeedMultplier = 1;
    public bool canInteract = true;
    public bool canMove = true;
    public bool AttackingMoveDebuff = false;


    private void Awake()
    {
        Instance = this;
        inventory = new Inventory();
        rb = GetComponent<Rigidbody>();
        Cursor.Instance.Hide();
        PlayerStatus.OnLimbStateChanged += OnLimbLoss;
    }

    private void FixedUpdate()
    {
        EvaluateMovement();
    }

    public void HideArm()
    {
        armObject.SetActive(false);
    }

    public void ShowArm()
    {
        armObject.SetActive(true);
    }
    
    public void Move(Vector2 _direction)
    {
        direction = _direction;
    }

    public void Dash()
    {
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
        if (Physics.Raycast(headTransform.position, headTransform.TransformDirection(Vector3.forward), out hit))
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

    }
    void OnLimbLoss(LimbState status)
    {
        if (!status.Head) PlayerStatus.OnDeath.Invoke();
        movementSpeedMultplier = 0.4f;
        if (status.LeftLeg) movementSpeedMultplier += 0.3f;
        if (status.RightLeg) movementSpeedMultplier += 0.3f;


        if (inventory.ItemSlot == null) return;
        if (!(status.LeftArm || status.RightArm)) DropItem();


        if(!(status.LeftArm && status.RightArm))
        {
            if (inventory.ItemSlot.getItemSO().HandRequirement == HandRequirement.DualHanded)
                DropItem();
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


        var velo = (forward * direction.y + right * direction.x) * movementSpeed + rb.linearVelocity.y * Vector3.up;
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
        if (Physics.Raycast(headTransform.position, headTransform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
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
             Destroy(handSlot.GetChild(0).gameObject);

        Instantiate(item.InHandPrefab, handSlot);
    }

    public void Use()
    {
        if (inventory.ItemSlot == null) return;
        switch (inventory.ItemSlot.getItemSO().HandRequirement)
        {
            case HandRequirement.SingeHanded:
                rArm.Attack("Attack");

                break;
            case HandRequirement.DualHanded:
                rArm.Attack("Attack"); //tutaj ata animacja dla 2 rak, ale jje nie ma jeszcze sadge

                break;
        }

    }

    public void TakeDamage(float value)
    {
        PlayerStatus.Health -= value;
    }
}
