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

        private void OnCreationComplete()
        {
            _hudView.OnCreationComplete();
        }

        public void OnWaveChanged(int wave)
        {
            _hudView.OnWaveChanged(wave);
        }
    }
}
