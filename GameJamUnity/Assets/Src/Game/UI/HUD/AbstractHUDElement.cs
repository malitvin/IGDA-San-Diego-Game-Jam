//Unity
using UnityEngine;

namespace UI.HUD
{
    public abstract class AbstractHUDElement : UIActor, IHUDElement
    {
        public HUDConfig _hudConfig
        {
            get; set;
        }

        public virtual void OnCreationComplete(HUDConfig hudConfig)
        {
            _hudConfig = hudConfig;
        }

        public virtual void OnEnemyCountChanged(int count)
        {

        }

        public virtual void OnWaveChanged(LevelConfig.WaveDef def, int maxWaves)
        {

        }
    }
}
