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
        private Collider[] _cachedColliders = new Collider[40];
        private string[] maskList = { "Enemy", "Default" };

        public override void Build(Vector3 finalPos, float buildTime, int fallheight, Ease easeType, BuildConfig.BuildableBlueprint blueprint)
        {
            _explosionMask = LayerMask.GetMask(maskList);
            base.Build(finalPos, buildTime, fallheight, easeType, blueprint);

            Tween t = transform.DOMove(finalPos, buildTime).SetEase(easeType).OnComplete(() =>
            {
                ////applyDamage(finalPos);

                Singleton.instance.particleGod.GenerateParticle(Particle.Type.HelixBomb, finalPos);
                Singleton.instance.audioSystem.PlaySound(SoundBank.Type.HelixBomb);
                RemoveFromPool();
            });
        }

        private void applyDamage(Vector3 epicenter)
        {
            int count = Physics.OverlapSphereNonAlloc(epicenter, _blueprint.explodeRadius, _cachedColliders, _explosionMask);
            for(int i = 0; i < count; ++i)
            {
                if(i >= _cachedColliders.Length)
                {
                    break;
                }

                IDamageable damageable = _cachedColliders[i].GetComponent<IDamageable>();
                if(damageable != null)
                {
                    damageable.TakeDamage(this, _blueprint.damage);
                }

            }
        }
    }
}
