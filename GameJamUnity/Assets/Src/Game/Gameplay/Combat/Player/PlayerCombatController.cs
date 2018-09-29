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
    
    public PlayerCombatController(PlayerCombatView view, PlayerConfig config)
    {
        _view = view;
        _physBody = view._rigidBody;
        _config = config;

        _physBody.drag = _config.movement.drag;
        _moveDirection = Vector3.zero;
    }
    
	public void SetAimPosition(Vector3 aimPos)
    {
        _aimPosition = aimPos;
    }

    public void FireWeapon(Vector3 targetPos)
    {
        if(_fireTimer > 0)
            return;

        Vector3 adjustedPos = (targetPos - _view.viewPosition).normalized * 50.0f;
        adjustedPos.y = targetPos.y;

        _view.VisualFireWeapon(_config.weapon.bulletSpeed, adjustedPos);
        _fireTimer = _config.weapon.fireCooldown;
    }

    public void Move(Vector3 direction)
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
}
