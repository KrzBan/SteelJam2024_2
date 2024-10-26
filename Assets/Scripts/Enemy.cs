using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class Enemy : MonoBehaviour, IDamagable
{
    public Transform target;
    public LayerMask m_LayerMask;

    [SerializeField] private CinemachineImpulseSource impulseSource;
    [SerializeField] private EnemyStatus status;
    [SerializeField] private int moneyDropMin;
    [SerializeField] private int moneyDropMax;
    [SerializeField] private GameObject moneyPrefab;
    [SerializeField] private GameObject bloodHitPrefab;
    [SerializeField] private float damage = 1f;
    [SerializeField] private float limbLossChanceOnHit = 0.1f;
    [SerializeField] private float hitRange = 1f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float rotateDuringAttackSpeed = 30f;
    [SerializeField] private float hitAngle = 30f;

    private bool movementPaused = false;
    private NavMeshAgent _agent;
    private Animator _animator;
    private float cooldown = 0f;
    private bool dead = false;

    private Vector3 OverlapBoxOffset;
    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    private void OnValidate()
    {
        if (moneyDropMax < moneyDropMin)
        {
            moneyDropMax = moneyDropMin;
        }
    }

    void Update()
    {
        if (dead)
        {
            return;
        }

        if (!movementPaused)
        {
            _animator.SetFloat("Speed", _agent.velocity.magnitude);
        }

        if (target == null)
        {
            return;
        }
        
        if (cooldown < attackCooldown)
        {
            var angle = Vector3.SignedAngle(transform.forward, (target.position - transform.position).normalized, Vector3.up);

            if (angle > 0.01f)
            {
                angle = rotateDuringAttackSpeed;
            }
            else if (angle < -0.01f)
            {
                angle = -rotateDuringAttackSpeed;
            }
            
            transform.Rotate(Vector3.up, angle * Time.deltaTime);
            
            cooldown += Time.deltaTime;
            //return;
        }
        else
        {
            cooldown = 0f;
            _animator.SetTrigger("Attack");
        }

        if (Vector3.Distance(target.position, transform.position) > hitRange)
        {
            if (!movementPaused)
            {
                _agent.SetDestination(target.position);
            }
            else
            {
                _agent.SetDestination(transform.position);
            }
            
            return;
        }
    }

    private void DropMoney()
    {
        var moneyObj = Instantiate(moneyPrefab, transform.position + Vector3.up, Quaternion.identity);
        moneyObj.GetComponent<Money>().Amount = Random.Range(moneyDropMin, moneyDropMax + 1);
    }

    public void PauseMovement()
    {
        movementPaused = true;
    }

    public void ResumeMovement()
    {
        movementPaused = false;
    }
    
    private void RagDoll()
    {
        foreach (var rb in GetComponentsInChildren<Rigidbody>())
        {
            rb.isKinematic = false;
        }

        _animator.enabled = false;
    }
    
    
    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public void DrawBloodHit(Vector3 position)
    {
        StartCoroutine(IDrawBlood(position));
    }

    IEnumerator IDrawBlood(Vector3 position)
    {
        var bloodObj = Instantiate(bloodHitPrefab, position, Quaternion.identity);
        bloodObj.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(1f);
        Destroy(bloodObj);
    }

    public void Attack()
    {
        if (target == null)
            return;
        
        var player = target.GetComponent<Player>();
        if (player == null)
        {
            return;
        }

        var playerDir = target.position - transform.position;
        playerDir.y = 0f;
        playerDir.Normalize();

        var forward = transform.forward;
        forward.y = 0f;
        forward.Normalize();
        
        var angle = Vector3.Angle(transform.forward, playerDir);
        if (angle <= hitAngle && Vector3.Distance(transform.position, target.position) <= hitRange + 0.3f)
        {
            player.Hit(new HitParams(damage, limbLossChanceOnHit));
            impulseSource.GenerateImpulse(0.2f);
        }
    }

    public void TakeDamage(float value)
    {
        if (dead)
        {
            return;
        }
        
        status.Health -= value;
        Debug.Log("Zombie HP :" + status.Health);
        if (status.Health <= 0f)
        {
            _agent.Stop();
            dead = true;
            RagDoll();
            DropMoney();
            
            RoomManager.Instance.OnEnemyDeath();
        }
    }
}
