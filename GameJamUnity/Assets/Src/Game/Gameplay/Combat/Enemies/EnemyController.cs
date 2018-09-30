using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController
{
    private EnemyView _view;
    private Transform _target;
    private NavMeshAgent _agent;

    public EnemyController(EnemyView view, Transform target)
    {
        _view = view;
        _agent = view.agent;
        _target = target;
    }

    public Vector3 position
    {
        get { return _view.transform.position; }
        set { _agent.Warp(value); }
    }

    public float speed
    {
        set { _agent.speed = value; }
    }

    public void RefreshTarget(Transform target = null)
    {
        if(target != null)
        {
            _target = target;
        }

        if(_agent != null)
        {
            _agent.SetDestination(_target.position);
            NavMeshPath path = _agent.path;
            if(path.status != NavMeshPathStatus.PathInvalid)
            {
                //Debug.Log("Path Found!");
            }
        }
    }
    
	
}
