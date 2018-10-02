//Unity
using UnityEngine;
using System;

namespace UI.HUD
{
    public class HUDController : BaseController
    {
        private PlayerConfig _playerConfig;

        public HUDController(PlayerConfig config)
        {
            _playerConfig = config;
        }

        private HUDView _hudView { get { return view as HUDView; } }

        public void Start(Action onComplete)
        {
            viewFactory.CreateAsync<HUDView>("GUI/PlayerHUD", (v) =>
            {
                view = v;
                OnCreationComplete();
                if(onComplete != null)
                {
                    onComplete();
                }
            });
        }

        public void SetPlayeMaxHealth(float max)
        {
            _hudView.SetPlayerMaxHealth(max);
        }

        public void SetParenMaxHealth(float max)
        {
            _hudView.SetParentMaxhealth(max);
        }

        private void OnCreationComplete()
        {

            SetPlayeMaxHealth(_playerConfig.startHealth);
            SetParenMaxHealth(_playerConfig.parentStartHealth);
            _hudView.OnCreationComplete();
        }

        public void OnWaveChanged(int wave)
        {
            _hudView.OnWaveChanged(wave);
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
