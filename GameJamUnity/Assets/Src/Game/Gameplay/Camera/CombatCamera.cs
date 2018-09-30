using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatCamera
{
    public float smoothTime = 0.2F;
    private Vector3 velocity = Vector3.zero;

    private Transform _target;
    private GameplayCamera _gameplayCam;
    private Vector3 _offsetDirection;
    private Vector3 _targetPosition;

    public CombatCamera(GameplayCamera gameplayCam, Transform target)
    {
        _target = target;
        _gameplayCam = gameplayCam;
    }

    public bool isEnabled
    {
        get; set;
    }

    public void Tick(float deltaTime)
    {
        if(!isEnabled || _target == null || _gameplayCam == null)
        {
            return;
        }
        // Define a target position above and behind the target transform
        Vector3 targetPosition = _target.TransformPoint(new Vector3(0, 15, -5));

        // Smoothly move the camera towards that target position
        _gameplayCam.transform.position = Vector3.SmoothDamp(_gameplayCam.transform.position, targetPosition, ref velocity, smoothTime);
    }
}
