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
    
    public PlayerCombatController(PlayerCombatView view, PlayerConfig config)
    {
        _view = view;
        _physBody = view._rigidBody;
        _config = config;

        _physBody.drag = _config.movement.drag;
        _moveDirection = Vector3.zero;
    }
    
    public Transform transform
    {
        get { return _physBody.transform; }
    }

	public void SetAimPosition(Vector3 aimPos)
    {
        _aimPosition = aimPos;
    }

    public void FireWeapon(Vector3 targetPos)
    {
        if(_fireTimer > 0)
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
            target.TakeDamage(hit.point, viewDir * 110.0f, 5.0f);
            adjustedPos = hit.point;
            Debug.Log("Target Hit");
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
        _physBody.drag = _config.movement.drag;
        _view.SetAimPosition(_aimPosition);

        if(_fireTimer > 0)
        {
            _fireTimer -= deltaTime;
        }
    }

    public void FixedTick(float fixedDeltaTime)
    {
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
