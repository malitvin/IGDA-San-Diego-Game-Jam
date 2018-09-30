using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageUtil
{
    public static int ApplyExplosionDamage(object attacker, Vector3 epicenter, float radius, float damage, int layerMask, ref Collider[] colliders)
    {
        int count = Physics.OverlapSphereNonAlloc(epicenter, radius, colliders, layerMask);
        for(int i = 0; i < count; ++i)
        {
            if(i >= colliders.Length)
            {
                break;
            }

            Collider collider = colliders[i];
            IDamageable damageable = collider.GetComponent<IDamageable>();
            if(damageable != null)
            {
                float distMod = radius / Mathf.Clamp(Vector3.Distance(epicenter, collider.transform.position), 0.01f, radius);
                float useDamage = damage * distMod;
                damageable.TakeDamage(attacker, useDamage);

                Rigidbody physBod = damageable.physbody;
                if(physBod != null)
                {
                    physBod.AddExplosionForce(distMod * 2000.0f, epicenter, 20.0f);
                }
            }
        }
        return count;
    }
}
