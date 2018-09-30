//Unity
using DG.Tweening;
using UnityEngine;
using Gameplay.Particles;

namespace Gameplay.Building
{
    public class BasicBlock : Buildable
    {
        private Vector3 glowVector = new Vector3(0, 0.5f, 0);
        public override void Build(Vector3 finalPos, float buildTime, int fallheight, Ease easeType)
        {
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
    }
}
