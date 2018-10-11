//Unity
using UnityEngine;
using System;

//C#
using System.Collections.Generic;

namespace UI.HUD
{
    public class HUDController : BaseController
    {
        private PlayerConfig _playerConfig;
        private HUDConfig _hudConfig;

        private List<IHUDElement> _hudElements;

        public HUDController(PlayerConfig config, HUDConfig hudConfig)
        {
            _playerConfig = config;
            _hudConfig = hudConfig;
        }

        private HUDView _hudView { get { return view as HUDView; } }

        public void Start(Action onComplete)
        {
            viewFactory.CreateAsync<HUDView>("GUI/PlayerHUD", (v) =>
            {
                view = v;
                OnCreationComplete();
                onComplete?.Invoke();
            });
        }

        private void OnCreationComplete()
        {
            SetPlayeMaxHealth(_playerConfig.startHealth);
            SetParenMaxHealth(_playerConfig.parentStartHealth);
            _hudView.OnCreationComplete(_hudConfig);

            //Get All HUD Elements
            _hudElements = new List<IHUDElement>();
            Transform parent = _hudView.transform;
            for (int i = 0; i < parent.childCount; i++)
            {
                IHUDElement child = parent.GetChild(i).GetComponent<IHUDElement>();
                if(child != null)
                {
                    _hudElements.Add(child);
                    child.OnCreationComplete(_hudConfig);
                }
            }
        }

        public void SetPlayeMaxHealth(float max)
        {
            _hudView.SetPlayerMaxHealth(max);
        }

        public void SetParenMaxHealth(float max)
        {
            _hudView.SetParentMaxhealth(max);
        }

        public void OnWaveChanged(LevelConfig.WaveDef def,int maxWaves)
        {
            if (_hudElements != null)
            {
                for (int i = 0; i < _hudElements.Count; i++)
                {
                    _hudElements[i].OnWaveChanged(def,maxWaves);
                }
            }
        }

        public void OnEnemyCountChanged(int count)
        {
            if (_hudElements != null)
            {
                for (int i = 0; i < _hudElements.Count; i++)
                {
                    _hudElements[i].OnEnemyCountChanged(count);
                }
            }
        }

        public void OnPlayerHealthChange(float value)
        {
            _hudView.OnPlayerHealthChange(value);
        }

        public void OnParentHealthChange(float value)
        {
            _hudView.OnParentHealthChange(value);
        }
    }
}
