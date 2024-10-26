using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour, IDamagable
{
    public Transform target;
    
    private NavMeshAgent _agent;
    private float _lastUpdate = 0.0f;
    private float _updateIntervalFar = 5.0f;
    private float _updateIntervalShort = 1.0f;
    private float _shortDistance = 4.0f;

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (target == null) return;
        
        var interval = (transform.position - target.position).magnitude > _shortDistance 
            ? _updateIntervalFar : _updateIntervalShort;
        if( Time.time > _lastUpdate + interval)   
        {
            _lastUpdate = Time.time;
            _agent.SetDestination(target.position);
        }
    }
    
    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public void TakeDamage(float value)
    {
        Destroy(this);
    }
}
