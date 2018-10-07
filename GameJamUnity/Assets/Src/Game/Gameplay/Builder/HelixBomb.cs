//Unity
using DG.Tweening;
using UnityEngine;
using Gameplay.Particles;
using Audio;

namespace Gameplay.Building
{
    public class HelixBomb : Buildable
    {
        private int _explosionMask;
        private Collider[] _cachedColliders = new Collider[100];
        private string[] maskList = { "Enemy", "Default" };

        public override void Build(Vector3 finalPos, float buildTime, int fallheight, Ease easeType, BuildConfig.BuildableBlueprint blueprint)
        {
            _explosionMask = LayerMask.GetMask(maskList);
            base.Build(finalPos, buildTime, fallheight, easeType, blueprint);

            Tween t = transform.DOMove(finalPos, buildTime).SetEase(easeType).OnComplete(() =>
            {
                DamageUtil.ApplyExplosionDamage(this, finalPos, _blueprint.explodeRadius, _blueprint.damage, _explosionMask, ref _cachedColliders);
                
                Singleton.instance.particleGod.GenerateParticle(Particle.Type.HelixBomb, finalPos);
                Singleton.instance.audioSystem.PlaySound(SoundBank.Type.HelixBomb);
                RemoveFromPool();
            });
        }

        private void applyDamage(Vector3 epicenter, float radius)
        {
            int count = Physics.OverlapSphereNonAlloc(epicenter, radius, _cachedColliders, _explosionMask);
            for(int i = 0; i < count; ++i)
            {
                if(i >= _cachedColliders.Length)
                {
                    break;
                }

                Collider collider = _cachedColliders[i];
                IDamageable damageable = collider.GetComponent<IDamageable>();
                if(damageable != null)
                {
                    float distMod = radius / Mathf.Clamp(Vector3.Distance(epicenter, collider.transform.position), 0.01f, radius);
                    float useDamage = _blueprint.damage * distMod;
                    damageable.TakeDamage(this, _blueprint.damage);
                    Rigidbody physBod = damageable.physbody;

                    if(physBod != null)
                    {
                        physBod.AddExplosionForce(100.0f, epicenter, 20.0f);
                    }
                }
            }
        }
    }
}
