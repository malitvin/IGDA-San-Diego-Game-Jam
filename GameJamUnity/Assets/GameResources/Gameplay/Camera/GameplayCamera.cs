using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayCamera : MonoBehaviour
{
    private Camera _camera;
    private Transform _target;

	// Use this for initialization
	void Awake ()
    {
        _camera = GetComponent<Camera>();
	}

    public Camera camera
    {
        get { return _camera; }
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }


	
}
