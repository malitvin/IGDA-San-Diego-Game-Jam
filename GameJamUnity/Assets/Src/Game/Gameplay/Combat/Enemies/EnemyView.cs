using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Common.Pooling;

public class EnemyView : PoolableObject
{
    public NavMeshAgent _agent;

    private Transform _target;
    private Vector3 _DEBUG_pos;
    private float _DEBUG_radius;

    private void Start()
    {
    }

    public NavMeshAgent agent
    {
        get { return _agent; }
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
