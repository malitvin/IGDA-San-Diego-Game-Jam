using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropDamageable : MonoBehaviour, IDamageable
{
    public Rigidbody _rigidBody;

    public float health
    {
        get { return 100; }
        set
        {
        // don't set it
        }
    }

    public DamageResult TakeDamage(Vector3 hitPosition, Vector3 force, float damage)
    {
        DamageResult result = new DamageResult();
        result.prevHealth = health;
        result.newHealth = health;

        if(_rigidBody)
        {
            _rigidBody.AddForceAtPosition(force, hitPosition, ForceMode.Impulse);
        }

        return result;
    }
}
