using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Common.Pooling;

public class EnemyView : PoolableObject, IDamageable
{
    public NavMeshAgent _agent;

    private Transform _target;
    private Vector3 _DEBUG_pos;
    private float _DEBUG_radius;
    
    public EnemyController controller
    {
        get; set;
    }

    public NavMeshAgent agent
    {
        get { return _agent; }
    }

    public float health
    {
        get { return controller != null ? controller.health : 0; }
    }

    public Rigidbody physbody { get; }

    public DamageResult TakeDamage(object attacker, float damage)
    {
        return controller != null ? controller.TakeDamage(attacker, damage) : null;
    }
    
    public void Die()
    {
        RemoveFromPool();
    }

    public void DebugDraw(Vector3 pos, float radius)
    {
        _DEBUG_pos = pos;
        _DEBUG_radius = radius;
    }



    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_DEBUG_pos, _DEBUG_radius);
    }
}
