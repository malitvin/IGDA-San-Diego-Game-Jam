//Unity
using UnityEngine;

namespace Gameplay.Loot
{
    public interface ILootState
    {
        void Begin();
        void Tick();
        void LateTick();
        void FixedTick();
        void Exit();

        Vector3 GetPosition();
        void SetPosition(Vector3 pos);
    }
}
