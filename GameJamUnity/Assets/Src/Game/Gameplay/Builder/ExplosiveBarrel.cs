using UnityEngine;
using DG.Tweening;

namespace Gameplay.Building
{
    public class ExplosiveBarrel : Buildable
    {
        public override void Build(Vector3 finalPos, float buildTime, int fallheight, Ease easeType)
        {
            base.Build(finalPos, buildTime, fallheight, easeType);
            Tween t = transform.DOMove(finalPos, buildTime).SetEase(easeType).OnComplete(() =>
            {
               
            });
        }
    }
}
