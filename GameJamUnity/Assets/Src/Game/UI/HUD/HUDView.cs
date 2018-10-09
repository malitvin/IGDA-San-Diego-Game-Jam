using UnityEngine.UI;
using UnityEngine;
using TMPro;
using GhostGen;
using DG.Tweening;

namespace UI.HUD
{
    public class HUDView : UIView
    {
        public CanvasGroup _waveGrid;
        public TextMeshProUGUI _waveText;

        public Slider _healthBar;
        public Image _healthBarImg;

        public Slider _parentHealthBar;
        public Image _parenthealthBarImg;

        private Color _healthyColor;
        private Color _unhealthyColor = Color.red;

        public void OnCreationComplete()
        {
            _waveGrid.alpha = 0;
            _healthyColor = _healthBarImg.color;
        }

        public void OnWaveChanged(int wave)
        {
            Tween intro = _waveGrid.DOFade(0, 1).SetEase(Ease.InOutCubic).OnComplete(() =>
            {
                _waveText.text = wave.ToString();
                Tween outro = _waveGrid.DOFade(1, 1).SetEase(Ease.InOutCubic).OnComplete(() =>
                {
                    _waveText.transform.DOScale(3,.5f).SetLoops(2, LoopType.Yoyo);
                });
            });
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
            fill.color = health >= (slider.maxValue*0.35f) ? _healthyColor : _unhealthyColor;
        }
    }
}
