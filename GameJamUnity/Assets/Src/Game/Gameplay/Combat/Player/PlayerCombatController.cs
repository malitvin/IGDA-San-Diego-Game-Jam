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

    public void Move(Vector3 direction)
    {
        _moveDirection = direction;
    }

	// Update is called once per frame
	public void Tick(float deltaTime)
    {
        _physBody.drag = _config.movement.drag;
        _view.SetAimPosition(_aimPosition);
    }

    public void FixedTick(float fixedDeltaTime)
    {
        Vector3 force = _moveDirection * _config.movement.speed;
        _physBody.AddForce(force);
    }
}
