//Unity
using UnityEngine;

//Tween
using DG.Tweening;

namespace UI.HUD
{
    [CreateAssetMenu(menuName = "Beasts/UI/HUD Config")]
    public class HUDConfig : ScriptableObject
    {
        [System.Serializable]
        public class FlowIndicatorData
        {
            [Header("ENEMY COUNT TEXT")]
            public Color enemyCountFlashColor = Color.red;
            [Range(0,3)]
            public float enemyCountFlashTime = 0.25f;
            [Range(0, 3)]
            public float enemyCountFadeTime = 1.0f;
            public Ease enemyCountFadeEaseType = Ease.InOutCubic;
            [Range(0, 3)]
            public float enemyCountScaleTime = 0.5f;
            [Range(0, 3)]
            public float enemyCountScale = 1.33f;
            public Ease enemyCountScaleEaseType = Ease.Flash;

            [Header("WAVE BAR")]
            public float waveBarVisibleWidth = 175;
            public float waveBarHiddenWidth = 55;
            [Range(0, 3)]
            public float waveBarMorphSpeed = 0.5f;
            public Ease waveBarMorphEaseType = Ease.InBounce;
        }

        [System.Serializable]
        public class HealthBarData
        {
            public Color healthyColor;
            public Color unhealthyColor;
            [Tooltip("What percent does the health need to go down to be unhealthy")]
            [Range(0,1)]
            public float unhealthyThreshold;
        }

        public FlowIndicatorData flowIndicatorData;
        public HealthBarData healthBarData;
    }
}
