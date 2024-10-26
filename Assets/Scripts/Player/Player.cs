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
    [CanBeNull] public static Player Instance { get; private set; }

    private Vector2 direction;
    private Rigidbody rb;

    public bool canInteract = true;
    public bool AttackingMoveDebuff = false;


    private void Awake()
    {
        Instance = this;
        inventory = new Inventory();
        rb = GetComponent<Rigidbody>();
        Cursor.Instance.Hide();
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
    }

    void OnLimbLoss(PlayerStatus status)
    {
        if (!status.Head) PlayerStatus.OnDeath.Invoke();
        movementSpeed = 0.2f;
        if (status.LeftLeg) movementSpeed += 0.4f;
        if (status.RightLeg) movementSpeed += 0.4f;

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
        var forward = headTransform.forward;
        forward.y = 0f;
        forward.Normalize();

        var right = headTransform.right;
        right.y = 0f;
        right.Normalize();


        var velo = (forward * direction.y + right * direction.x) * movementSpeed + rb.linearVelocity.y * Vector3.up;
        if (AttackingMoveDebuff) velo *= 0.3f;
        rb.linearVelocity = velo;
    }

    public void Interact()
    {
        RaycastHit hit;
        if (Physics.Raycast(headTransform.position, headTransform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
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
             Destroy(handSlot.GetChild(0));

        Instantiate(item.Prefab, handSlot);
    }

    public void Use()
    {
        rArm.Attack();
    }

    public void TakeDamage(float value)
    {
        PlayerStatus.Health -= value;
    }
}
