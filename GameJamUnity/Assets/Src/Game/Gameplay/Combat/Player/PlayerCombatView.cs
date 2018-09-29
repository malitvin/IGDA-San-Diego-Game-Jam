using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatView : MonoBehaviour
{
    public Rigidbody _rigidBody;
    public Transform _weaponEndPoint;
    public Transform _rotateTransform;

	// Use this for initialization
	void Start ()
    {
		
	}
	
    public Vector3 rigidPosition
    {
        get { return _rigidBody.position; }
    }

    public Vector3 viewPosition
    {
        get { return _rotateTransform.position; }
    }

    public Quaternion rotation
    {
        get { return _rotateTransform.rotation; }
        set { _rotateTransform.rotation = value; }
    }

    public void SetAimPosition(Vector3 aimPosition)
    {
        _rotateTransform.LookAt(aimPosition);
    }

    public void SetCurrentWeapon(string weapon)
    {

    }

    public void VisualFireWeapon(Vector3 target)
    {

    }
}
