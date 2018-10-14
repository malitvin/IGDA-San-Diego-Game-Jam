namespace Gameplay.Loot
{
    public class CollectedState : LootItemState
    {
        private GhostGen.IEventDispatcher _dispatcher;

        public CollectedState(BaseLootItem lootItem) : base(lootItem)
        {
            _dispatcher = Singleton.instance.notificationDispatcher;
        }

        public override void Begin()
        {
            _dispatcher.DispatchEvent(GameplayEventType.LOOT_PICKED_UP, false, _lootItem._itemDef);
            _lootItem.Pickup();
        }
    }
}
