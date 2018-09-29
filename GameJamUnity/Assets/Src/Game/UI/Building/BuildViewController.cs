//unity
using UnityEngine;

//Game
using Gameplay.Inventory;
using Gameplay.Building;

//C#
using System.Collections.Generic;

namespace UI.Building
{
    public class BuildViewController : BaseController
    {
        private InventorySystem _inventorySystem;
        private InventoryConfig.UISpawnData _uiSpawnData;
        private BuildConfig _buildConfig;

        private Dictionary<Inventory.Type, BuildCurrencyItem> _inventoryUIItems = new Dictionary<Inventory.Type, BuildCurrencyItem>();

        public BuildViewController(InventorySystem inventorySystem, BuildConfig buildConfig)
        {
            _inventorySystem = inventorySystem;
            _buildConfig = buildConfig;
            _uiSpawnData = inventorySystem.GetSpawnData();

            viewFactory.CreateAsync<BuildView>("GUI/BuildView", (v) =>
            {
                view = v;
                OnCreationComplete();
            });
        }

        private BuildView _buildView { get { return view as BuildView;} }

        private void OnCreationComplete()
        {
            CreateInventory();
            CreateBuildables();
        }

        private void CreateInventory()
        {
            foreach (var element in _inventorySystem.GetInventory())
            {
                BuildCurrencyItem item = GameObject.Instantiate(_uiSpawnData._buildItem, _buildView._inventoryParent.transform, false) as BuildCurrencyItem;
                item.SetContent(element.Value);
                _inventoryUIItems[element.Value.key] = item;
            }
        }

        private void CreateBuildables()
        {
            BuildConfig.BuildableBlueprint[] blueprints = _buildConfig.buildables;
            BuildConfig.BuildUIData _uiData = _buildConfig._uiData;
            for (int i = 0; i < blueprints.Length; i++)
            {
                BuildConfig.BuildableBlueprint blueprint = blueprints[i];
                BuildableUIItem uiItem = GameObject.Instantiate(_uiData._itemPrefabUI, _buildView._buildableParent.transform, false) as BuildableUIItem;
                uiItem.SetContent(blueprint,i);
            }
        }
    }
}
