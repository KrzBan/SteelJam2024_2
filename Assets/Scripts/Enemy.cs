using System.Collections;
using UnityEngine;
using UnityEngine.AI;

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

    private NavMeshAgent _agent;
    private Animator _animator;
    private float _lastUpdate = 0.0f;
    private float _updateIntervalFar = 5.0f;
    private float _updateIntervalShort = 1.0f;
    private float _shortDistance = 4.0f;
    private EnemyState _state;
    private float _stateSwitchCooldown=0;
    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _state = EnemyState.Following;
    }

    void Update()
    {
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
                if(Vector3.Distance(transform.position,target.position) < 1f
                    && _stateSwitchCooldown <=0)
                {
                    _state = EnemyState.Attacking;
                    _stateSwitchCooldown = 2f;
                    _animator.SetBool("Attack", true);
                    StartCoroutine(Attack());
                    _agent.Stop();
                }

                break;
            case EnemyState.Attacking:
                if(_stateSwitchCooldown <=0)
                {
                    _state = EnemyState.Following;
                    _agent.Resume();
                }


                break;
        }
        
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


        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, Quaternion.identity, m_LayerMask);
        int i = 0;
        while (i < hitColliders.Length)
        {
            Debug.Log("Hit : " + hitColliders[i].name + i);
            i++;
            IDamagable player;
           if( hitColliders[i].TryGetComponent<IDamagable>(out player))
            {
                player.TakeDamage(1f);
            }

        }

    }    
    
    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public void TakeDamage(float value)
    {
        status.Health -= value;

        if (status.Health <= 0f)
        {
            RagDoll();
        }
    }
}
