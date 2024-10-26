using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class Enemy : MonoBehaviour, IDamagable
{

    enum EnemyState
    {
        Following,
        Attacking
    }
    public Transform target;
    public LayerMask m_LayerMask;

    [SerializeField] private EnemyStatus status;
    [SerializeField] private int moneyDropMin;
    [SerializeField] private int moneyDropMax;
    [SerializeField] private GameObject moneyPrefab;
    [SerializeField] private GameObject bloodHitPrefab;

    private NavMeshAgent _agent;
    private Animator _animator;
    private float _lastUpdate = 0.0f;
    private float _updateIntervalFar = 5.0f;
    private float _updateIntervalShort = 1.0f;
    private float _shortDistance = 4.0f;
    private EnemyState _state;
    private float _stateSwitchCooldown = 0;

    private Vector3 OverlapBoxOffset;
    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _state = EnemyState.Following;
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
        OverlapBoxOffset = new Vector3(0, 1, 0) + (transform.forward * 0.5f);
        _animator.SetFloat("Speed", _agent.velocity.magnitude);
        
        if (target == null) return;

        _stateSwitchCooldown -= Time.deltaTime;
        switch(_state)
        {
            case EnemyState.Following :

                var interval = (transform.position - target.position).magnitude > _shortDistance
            ? _updateIntervalFar : _updateIntervalShort;
                if (Time.time > _lastUpdate + interval)
                {
                    _lastUpdate = Time.time;
                    _agent.SetDestination(target.position);
                }
                if(Vector3.Distance(transform.position,target.position) < 1.2f
                    && _stateSwitchCooldown <=0)
                {
                    _state = EnemyState.Attacking;
                    _stateSwitchCooldown = 2f;
                    _animator.SetBool("Attack", true);
                    StartCoroutine(Attack());
                    _agent.isStopped = true;
                }

                break;
            case EnemyState.Attacking:
                if(_stateSwitchCooldown <=0)
                {
                    _state = EnemyState.Following;
                    _agent.isStopped = false;
                }


                break;
        }
        
    }

    private void DropMoney()
    {
        var moneyObj = Instantiate(moneyPrefab, transform.position + Vector3.up, Quaternion.identity);
        moneyObj.GetComponent<Money>().Amount = Random.Range(moneyDropMin, moneyDropMax + 1);
    }

    private void RagDoll()
    {
        foreach (var rb in GetComponentsInChildren<Rigidbody>())
        {
            rb.isKinematic = false;
        }

        _animator.enabled = false;
    }
    IEnumerator Attack()
    {


        yield return new WaitForSeconds(1f);


        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position + OverlapBoxOffset, transform.localScale / 2, Quaternion.identity, m_LayerMask);
        int i = 0;
        while (i < hitColliders.Length)
        {
            Debug.Log("Hit : " + hitColliders[i].name + i);
          
            IDamagable player;
           if( hitColliders[i].TryGetComponent<IDamagable>(out player))
            {
                player.TakeDamage(1f);
            }
            i++;
        }

    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
            //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
            Gizmos.DrawWireCube(transform.position + OverlapBoxOffset, transform.localScale);
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

    public void TakeDamage(float value)
    {
        status.Health -= value;

        if (status.Health <= 0f)
        {
            RagDoll();
            DropMoney();
        }
    }
}
