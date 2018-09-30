using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    float health { get; }

    Rigidbody physbody { get; }
    DamageResult TakeDamage(object attacker, float damage);
}
