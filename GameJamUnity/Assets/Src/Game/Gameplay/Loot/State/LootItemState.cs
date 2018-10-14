namespace Gameplay.Loot
{
    public abstract class LootItemState : ILootState
    {
        protected BaseLootItem _lootItem;

        public LootItemState(BaseLootItem lootItem)
        {
            _lootItem = lootItem;
        }

        #region Interface Methods
        public virtual void Begin()
        {
            
        }

        public virtual void Tick()
        {
            
        }

        public virtual void LateTick()
        {

        }

        public virtual void FixedTick()
        {

        }

        public virtual void Exit()
        {
            
        }
        #endregion
    }
}
