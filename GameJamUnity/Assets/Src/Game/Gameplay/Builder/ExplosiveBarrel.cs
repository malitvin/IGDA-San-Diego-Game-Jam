using UnityEngine;
using DG.Tweening;
using Gameplay.Particles;
using Audio;

namespace Gameplay.Building
{
    public class ExplosiveBarrel : Buildable, IDamageable
    {
        private const float kDelayDuration = 0.2f;

        private int _explosionMask;
        private string[] maskList = { "Enemy", "Default", "CombatPlayer" };
        private Collider[] _cachedCollider = new Collider[100];
        private Tween _dropTween;
        private Sequence _delayCall;

        public Rigidbody _rigidBody;

        public override void Build(Vector3 finalPos, float buildTime, int fallheight, Ease easeType, BuildConfig.BuildableBlueprint blueprint)
        {
            _explosionMask = LayerMask.GetMask(maskList);
            health = 1;

            base.Build(finalPos, buildTime, fallheight, easeType, blueprint);

            _dropTween = transform.DOMove(finalPos, buildTime)
                .SetEase(easeType)
                .OnComplete(killTweens);
        }

        public float health { get; set; }
        public bool isDead { get { return health <= 0; } }

        public Rigidbody physbody { get { return _rigidBody; } }

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
                _delayCall = DOTween.Sequence();
                _delayCall.InsertCallback(kDelayDuration, applyDamage);
                _delayCall.Play();
            }

            return result;
        }

        public override void RemovePhysics()
        {
            base.RemovePhysics();
            Destroy(_rigidBody);
        }

        private void applyDamage()
        {
            Vector3 position = transform.position;

            if(_blueprint != null)
            {
                DamageUtil.ApplyExplosionDamage(this, position, _blueprint.explodeRadius, _blueprint.damage, _explosionMask, ref _cachedCollider);
            }

            Singleton.instance.particleGod.GenerateParticle(Particle.Type.BaseExplosion, position);
            Singleton.instance.audioSystem.PlaySound(SoundBank.Type.Explosion, null, true);

            killTweens();
            RemoveFromPool();
        }

        private void killTweens()
        {
            if(_dropTween != null)
            {
                _dropTween.Kill(true);
                _dropTween = null;
            }

            if(_delayCall != null)
            {
                _delayCall.Kill(true); ;
                _delayCall = null;
            }
        }
    }
}
