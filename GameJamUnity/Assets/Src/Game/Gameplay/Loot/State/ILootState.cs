namespace Gameplay.Loot
{
    public interface ILootState
    {
        void Begin();
        void Tick();
        void LateTick();
        void FixedTick();
        void Exit();
    }
}
