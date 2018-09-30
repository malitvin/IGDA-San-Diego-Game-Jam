using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GhostGen;
using Audio;
using UI.HUD;

public class PlayerCombatController : EventDispatcher
{
    private PlayerCombatView _view;
    private PlayerConfig _config;

    private Rigidbody _physBody;
    private Vector3 _moveDirection;
    private Vector3 _aimPosition;
    private float _fireTimer;
    private RaycastHit[] rayHits = new RaycastHit[10];
    private bool _isEnabled;
    private IEventDispatcher _dispatcher;

    public PlayerCombatController(PlayerCombatView view, PlayerConfig config)
    {
        health = config.startHealth;
        _view = view;
        _view.controller = this;
        _physBody = view._rigidBody;
        _config = config;

        _physBody.drag = _config.movement.drag;
        _moveDirection = Vector3.zero;

        _dispatcher = Singleton.instance.notificationDispatcher;
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

    public bool isDead
    {
        get { return health <= 0; }
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

        DispatchEvent(GameplayEventType.DAMAGE_TAKEN,false, result);

        if(isDead && result.prevHealth > 0.0f)
        {
            //Do sumthin'
        }
        return result;
    }

    public void FireWeapon(Vector3 targetPos)
    {
        if(_fireTimer > 0 || !isEnabled || isDead)
            return;
        
        Vector3 weaponPos = _view.weaponBarrelPosition;
        Vector3 viewDir = (targetPos - weaponPos).normalized;
        viewDir.y = 0;
        Vector3 adjustedPos = weaponPos + (viewDir * 50.0f);

        Vector3 rayPoint = _view.rigidPosition;
        rayPoint.y = weaponPos.y;
        Ray ray = new Ray(weaponPos, viewDir);

        Singleton.instance.audioSystem.PlaySound(SoundBank.Type.WeaponFire, null, true);

        IDamageable target;
        RaycastHit hit;
        if(getTarget(ray, out hit, out target))
        {
            Vector3 force = viewDir * 110.0f;

            DamageResult dResult = target.TakeDamage(this, _config.weapon.damage);
            _dispatcher.DispatchEvent(GameplayEventType.DAMAGE_TAKEN, false, dResult);

            if(target.physbody)
            {
                target.physbody.AddForceAtPosition(force, hit.point, ForceMode.Impulse);
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
        if(!isEnabled || isDead)
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
        if(!isEnabled || isDead)
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

        string[] layerList = { "CombatPlayer" };
        if(Physics.RaycastNonAlloc(fireDirection, rayHits,100.0f, ~LayerMask.GetMask(layerList)) > 0)
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
