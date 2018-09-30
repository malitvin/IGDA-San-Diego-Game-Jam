//Unity
using UnityEngine;

namespace UI.HUD
{
    public class HUDController : BaseController
    {
        public HUDController()
        {
            viewFactory.CreateAsync<HUDView>("GUI/PlayerHUD", (v) =>
            {
                Debug.Log("HERE");
                view = v;
                OnCreationComplete();
            });
        }

        private HUDView _hudView { get { return view as HUDView; } }

        private void OnCreationComplete()
        {
            _hudView.OnCreationComplete();
        }
    }
}
