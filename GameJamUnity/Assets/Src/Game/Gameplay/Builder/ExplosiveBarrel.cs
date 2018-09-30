using UnityEngine;
using DG.Tweening;
using Gameplay.Particles;
using Audio;

namespace Gameplay.Building
{
    public class ExplosiveBarrel : Buildable, IDamageable
    {
        private int _explosionMask;
        private string[] maskList = { "Enemy", "Default", "CombatPlayer" };
        private Collider[] _cachedCollider = new Collider[30];
        private Tween _dropTween;

        public override void Build(Vector3 finalPos, float buildTime, int fallheight, Ease easeType, BuildConfig.BuildableBlueprint blueprint)
        {
            _explosionMask = LayerMask.GetMask(maskList);
            health = 1;

            base.Build(finalPos, buildTime, fallheight, easeType, blueprint);
            _dropTween = transform.DOMove(finalPos, buildTime).SetEase(easeType).OnComplete(() =>
            {
                _dropTween = null;
            });
        }

        public float health { get; set; }
        public bool isDead { get { return health <= 0; } }

        public Rigidbody physbody { get; }

        public DamageResult TakeDamage(object attacker, float damage)
        {
            DamageResult result = new DamageResult();
            result.prevHealth = health;
            health = Mathf.Max(health - damage, 0.0f);
            result.newHealth = health;
            result.victim = this;
            result.attacker = attacker;

            if(isDead && result.prevHealth > 0.0f)
            {
                DamageUtil.ApplyExplosionDamage(this, transform.position, _blueprint.explodeRadius, _blueprint.damage, _explosionMask, ref _cachedCollider);
                Singleton.instance.particleGod.GenerateParticle(Particle.Type.BaseExplosion, transform.position);
                Singleton.instance.audioSystem.PlaySound(SoundBank.Type.Explosion, null, true);

                if(_dropTween != null)
                {
                    _dropTween.Kill(true);
                    _dropTween = null;
                }

                RemoveFromPool();
            }
            return result;
        }

        public override void RemovePhysics()
        {
            base.RemovePhysics();
            Destroy(_rigidBody);
        }
    }
}
