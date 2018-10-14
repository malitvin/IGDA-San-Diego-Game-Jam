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
        private BuildingSystem _buildingSystem;
        private InventorySystem _inventorySystem;
        private InventoryConfig.UISpawnData _uiSpawnData;
        private BuildConfig _buildConfig;

        private Dictionary<Storeable.Type, BuildCurrencyItem> _inventoryUIItems = new Dictionary<Storeable.Type, BuildCurrencyItem>();
        private Dictionary<Buildable.TYPE, BuildableUIItem> _buildItems = new Dictionary<Buildable.TYPE, BuildableUIItem>();
        private BuildableUIItem _currentItem;
        private bool _isVisible;

        public BuildViewController(BuildingSystem buildingSystem,InventorySystem inventorySystem, BuildConfig buildConfig)
        {
            _buildingSystem = buildingSystem;
            _inventorySystem = inventorySystem;
            _buildConfig = buildConfig;
            _uiSpawnData = inventorySystem.GetSpawnData();

            viewFactory.CreateAsync<BuildView>("GUI/BuildView", (v) =>
            {
                view = v;
                SetVisible(_isVisible);
                OnCreationComplete();
            });
        }

        public void SetVisible(bool isVisible)
        {
            _isVisible = isVisible;
            if(_buildView)
            {
                _buildView.SetVisible(isVisible);
            }
        }
        private BuildView _buildView { get { return view as BuildView;} }

        private void OnCreationComplete()
        {
            CreateInventory();
            CreateBuildables();
        }

        private void CreateInventory()
        {
            foreach (var element in _inventorySystem.GetInventoryMap())
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
                _buildItems[blueprint.key] = uiItem;
            }
            OnBuildTypeChange(_buildingSystem._currentBuildType);
        }

        public void OnBuildTypeChange(Buildable.TYPE type)
        {
            if(_currentItem)
            {
                _currentItem.ToggleItem(false);
            }
            if(_buildItems.ContainsKey(type))
            {
                _currentItem = _buildItems[type];
                _currentItem.ToggleItem(true);
            }
        }

        public void UpdateInventoryUI(Storeable.Type type, int amount)
        {
            if(_inventoryUIItems.ContainsKey(type))
            {
                _inventoryUIItems[type].SetText(amount);
            }
        }
    }
}
