using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController
{
    private PlayerCombatView _view;
    private PlayerConfig _config;

    private Rigidbody _physBody;
    private Vector3 _moveDirection;
    private Vector3 _aimPosition;
    private float _fireTimer;
    private RaycastHit[] rayHits = new RaycastHit[10];
    private bool _isEnabled;

    public PlayerCombatController(PlayerCombatView view, PlayerConfig config)
    {
        health = config.startHealth;
        _view = view;
        _physBody = view._rigidBody;
        _config = config;

        _physBody.drag = _config.movement.drag;
        _moveDirection = Vector3.zero;
    }
    
    public float health { get; set; }

    public bool isEnabled
    {
        get { return _isEnabled; }
        set
        {
            _isEnabled = value;
            if(_view != null)
            {
                _view.gameObject.SetActive(value);
            }
        }
    }

    public Transform transform
    {
        get { return _physBody.transform; }
    }

	public void SetAimPosition(Vector3 aimPos)
    {
        _aimPosition = aimPos;
    }

    public DamageResult TakeDamage(object attacker, float damage)
    {
        DamageResult result = new DamageResult();
        result.prevHealth = health;
        health = Mathf.Max(health - damage, 0.0f);
        result.newHealth = health;
        result.victim = this;
        result.attacker = attacker;
        return result;
    }

    public void FireWeapon(Vector3 targetPos)
    {
        if(_fireTimer > 0 || !isEnabled)
            return;
        
        Vector3 weaponPos = _view.weaponBarrelPosition;
        Vector3 viewDir = (targetPos - weaponPos).normalized;
        viewDir.y = 0;
        Vector3 adjustedPos = weaponPos + (viewDir * 50.0f);

        Vector3 rayPoint = _view.rigidPosition;
        rayPoint.y = weaponPos.y;
        Ray ray = new Ray(weaponPos, viewDir);

        IDamageable target;
        RaycastHit hit;
        if(getTarget(ray, out hit, out target))
        {
            Vector3 force = viewDir * 110.0f;
            target.TakeDamage(this, 5.0f);
            if(target.rigidBody)
            {
                target.rigidBody.AddForceAtPosition(force, hit.point, ForceMode.Impulse);
            }

            adjustedPos = hit.point;
        }
        
        Debug.DrawRay(adjustedPos, Vector3.up, Color.red);
        _view.VisualFireWeapon(_config.weapon.bulletSpeed, adjustedPos);
        _fireTimer = _config.weapon.fireCooldown;
    }

    public void SetMoveDirection(Vector3 direction)
    {
        _moveDirection = direction;
    }

	// Update is called once per frame
	public void Tick(float deltaTime)
    {
        if(!isEnabled)
        {
            return;
        }

        _physBody.drag = _config.movement.drag;
        _view.SetAimPosition(_aimPosition);

        if(_fireTimer > 0)
        {
            _fireTimer -= deltaTime;
        }
    }

    public void FixedTick(float fixedDeltaTime)
    {
        if(!isEnabled)
        {
            return;
        }

        Vector3 force = _moveDirection * _config.movement.speed;
        _physBody.AddForce(force, ForceMode.Acceleration); // Already applies fixedDeltaTime, Fuck ya.
    }
    
    private bool getTarget(Ray fireDirection, out RaycastHit raycastHit, out IDamageable target)
    {
        bool result = false;

        target = null;
        raycastHit = default(RaycastHit);

        if(Physics.RaycastNonAlloc(fireDirection, rayHits) > 0)
        {
            for(int i = 0; i < rayHits.Length; ++i)
            {
                RaycastHit hit = rayHits[i];
                if(hit.collider == null)
                {
                    continue;
                }

                target = hit.collider.GetComponent<IDamageable>();
                if(target != null)
                {
                    raycastHit = hit;
                    result = true;
                    break;
                }
            }
        }

        return result;
    }
}
