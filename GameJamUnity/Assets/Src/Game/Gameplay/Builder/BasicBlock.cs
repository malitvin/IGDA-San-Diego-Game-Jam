//Unity
using DG.Tweening;
using UnityEngine;
using Gameplay.Particles;

namespace Gameplay.Building
{
    public class BasicBlock : Buildable, IDamageable
    {
        private Tween _damageShake;
        private Vector3 glowVector = new Vector3(0, 0.5f, 0);
        public override void Build(Vector3 finalPos, float buildTime, int fallheight, Ease easeType)
        {
            health = 8;

            Sequence s = DOTween.Sequence();

            base.Build(finalPos, buildTime, fallheight, easeType);
            Tween t = transform.DOMove(finalPos, buildTime).SetEase(easeType).OnUpdate(() =>
            {
                Vector3 center = _boxCollider.center;
                center.y = -transform.position.y + 0.5f;
                _boxCollider.center = center;
            });

            s.Insert(0.0f, t);
            s.InsertCallback(t.Duration() * 0.375f, () =>
            {
                Singleton.instance.particleGod.GenerateParticle(Particle.Type.BuildingGlow, finalPos += glowVector);
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
                Singleton.instance.particleGod.GenerateParticle(Particle.Type.Break, transform.position);
                RemoveFromPool();
            }
            else
            {
                damageShake();
            }
            return result;
        }

        private void damageShake()
        {
            killTween();

            _damageShake = transform.DOShakeRotation(0.4f, 20);
            _damageShake.OnComplete(() => _damageShake = null);
        }

        private void killTween()
        {
            if(_damageShake != null)
            {
                _damageShake.Kill(true);
                _damageShake = null;
            }
        }
    }
}
