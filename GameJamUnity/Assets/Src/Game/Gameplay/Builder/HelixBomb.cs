//Unity
using DG.Tweening;
using UnityEngine;

namespace Gameplay.Building
{
    public class HelixBomb : Buildable
    {
        public override void Build(Vector3 finalPos, float buildTime, int fallheight, Ease easeType)
        {
            base.Build(finalPos, buildTime, fallheight, easeType);
            Tween t = transform.DOMove(finalPos, buildTime).SetEase(easeType).OnComplete(() =>
            {
                RemoveFromPool();
            });
        }
    }
}
