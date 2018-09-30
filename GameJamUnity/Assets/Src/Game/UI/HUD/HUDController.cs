//Unity
using UnityEngine;
using System;

namespace UI.HUD
{
    public class HUDController : BaseController
    {
        public HUDController(Action onComplete)
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

        private HUDView _hudView { get { return view as HUDView; } }

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
