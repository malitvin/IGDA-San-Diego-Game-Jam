﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropDamageable : MonoBehaviour, IDamageable
{
    public Rigidbody _rigidBody;

    public float health
    {
        get { return 100; }
    }

    public Rigidbody physbody { get { return _rigidBody; } }

    public DamageResult TakeDamage(object attacker, float damage)
    {
        DamageResult result = new DamageResult();
        result.attacker = attacker;
        result.victim =this;
        return result;
    }
}
