using UnityEngine.UI;
using GhostGen;
using DG.Tweening;

namespace UI.HUD
{
    public class HUDView : UIView
    {
        public Slider _healthBar;
        public Image _healthBarImg;

        public Slider _parentHealthBar;
        public Image _parenthealthBarImg;

        private HUDConfig.HealthBarData _healthBarData;

        public void OnCreationComplete(HUDConfig hudConfig)
        {
            _healthBarData = hudConfig.healthBarData;
        }

        public void SetPlayerMaxHealth(float max)
        {
            _healthBar.maxValue = max;
            _healthBar.value = max;
        }

        public void SetParentMaxhealth(float max)
        {
            _parentHealthBar.maxValue = max;
            _parentHealthBar.value = max;
        }

        public void OnPlayerHealthChange(float health)
        {
            ChangeHealthBar(_healthBar, _healthBarImg, health);
        }

        public void OnParentHealthChange(float health)
        {
            ChangeHealthBar(_parentHealthBar, _parenthealthBarImg, health);
        }

        private void ChangeHealthBar(Slider slider,Image fill,float health)
        {
            slider.DOKill();
            slider.DOValue(health, .1f).SetEase(Ease.OutElastic);
            fill.color = health >= (slider.maxValue*_healthBarData.unhealthyThreshold) ? _healthBarData.healthyColor : _healthBarData.unhealthyColor;
        }
    }
}
