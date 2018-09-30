using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController
{
    private EnemyView _view;
    private Transform _target;
    private NavMeshAgent _agent;
    private EnemyDef _def;
    private float _attackTimer; 

    public EnemyController(EnemyDef def, EnemyView view, Transform target)
    {
        _def = def;
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
    
    public void Tick(float deltaTime)
    {
        _attackTimer -= deltaTime;
        attackCheck();
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
    
	private void attackCheck()
    {
        if(_agent == null)
        {
            return;
        }

        if(_attackTimer > 0)
        {
            return;
        }
        
        if(_view != null)
        {
            _view.DebugDraw(position, 0.55f);
        }

        Collider[] targets = Physics.OverlapSphere(position, 0.75f);
        if(targets != null)
        {
            for(int i = 0; i < targets.Length; ++i)
            {
                Transform targetXform = targets[i].transform;
                IDamageable target = targetXform.GetComponent<IDamageable>();

                if(target == null)
                {
                    continue;
                }

                Vector3 direction = (targetXform.position - _agent.transform.position).normalized;
                Vector3 force = direction * 10.0f;

                target.TakeDamage(targetXform.position + direction, direction, _def.attack.damage);
            }
        }

        _attackTimer = _def.attack.refreshDuration;
    }
}
