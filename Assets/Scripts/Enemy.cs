using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class Enemy : MonoBehaviour, IDamagable
{
    public Transform target;

    [SerializeField] private EnemyState state;

    private NavMeshAgent _agent;
    private Animator _animator;
    private float _lastUpdate = 0.0f;
    private float _updateIntervalFar = 5.0f;
    private float _updateIntervalShort = 1.0f;
    private float _shortDistance = 4.0f;

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        _animator.SetFloat("Speed", _agent.velocity.magnitude);
        
        if (target == null) return;
        
        var interval = (transform.position - target.position).magnitude > _shortDistance 
            ? _updateIntervalFar : _updateIntervalShort;
        if( Time.time > _lastUpdate + interval)   
        {
            _lastUpdate = Time.time;
            _agent.SetDestination(target.position);
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
    
    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public void TakeDamage(float value)
    {
        state.Health -= value;

        if (state.Health <= 0f)
        {
            RagDoll();
        }
    }
}
