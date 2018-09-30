using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    float health { get; set; }

    Rigidbody rigidBody { get; }
    DamageResult TakeDamage(object attacker, float damage);
}
