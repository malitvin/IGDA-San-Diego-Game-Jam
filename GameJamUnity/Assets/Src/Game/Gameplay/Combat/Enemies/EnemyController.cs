using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using GhostGen;

public class EnemyController
{
    private EnemyView _view;
    private Transform _target;
    private NavMeshAgent _agent;
    private EnemyDef _def;
    private float _attackTimer;
    private IEventDispatcher _dispatcher;
    private int _enemyLayer;

    public EnemyController(EnemyDef def, EnemyView view, Transform target)
    {
        _def = def;
        _view = view;
        _view.controller = this;
        _agent = view.agent;
        _target = target;
        _dispatcher = Singleton.instance.notificationDispatcher;
        string[] maskList = { "Enemy" };
        _enemyLayer = LayerMask.GetMask(maskList);
    }

    public void Init()
    {
        health = _def.startHealth;
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

    public float health { get; set; }
    
    public bool isDead
    {
        get { return health <= 0; }
    }

    public DamageResult TakeDamage(object attacker, float damage)
    {
        DamageResult result = new DamageResult();
        result.prevHealth = health;
        health = Mathf.Max(health - damage, 0.0f);
        result.newHealth = health;
        result.victim = this;
        result.attacker = attacker;

        if(isDead && result.prevHealth > 0.0f)
        {
            _view.Die();
        }
        return result;
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
        if(_agent == null || _attackTimer > 0)
        {
            return;
        }
        
        if(_view != null)
        {
            _view.DebugDraw(position, 0.75f);
        }

        Collider[] targets = Physics.OverlapSphere(position, 0.75f, _enemyLayer);//, _def.targetMask);
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
                
                DamageResult damageResult = target.TakeDamage(this, _def.attack.damage);
                _dispatcher.DispatchEvent(GameplayEventType.DAMAGE_TAKEN, false, damageResult);
            }
        }

        _attackTimer = _def.attack.refreshDuration;
    }
}
