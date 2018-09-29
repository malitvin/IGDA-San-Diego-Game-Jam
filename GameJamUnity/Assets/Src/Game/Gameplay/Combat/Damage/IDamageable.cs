using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    float health { get; set; }

    DamageResult TakeDamage(Vector3 hitPosition, Vector3 force, float damage);
}
