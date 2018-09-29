﻿//Unity
using DG.Tweening;
using UnityEngine;

namespace Gameplay.Building
{
    public class BasicBlock : Buildable
    {
        public override void Build(Vector3 finalPos, float buildTime, int fallheight, Ease easeType)
        {
            base.Build(finalPos, buildTime, fallheight, easeType);
            Tween t = transform.DOMove(finalPos, buildTime).SetEase(easeType).OnUpdate(() =>
            {
                Vector3 center = _boxCollider.center;
                center.y = -transform.position.y + 0.5f;
                _boxCollider.center = center;
            });
        }
    }
}
