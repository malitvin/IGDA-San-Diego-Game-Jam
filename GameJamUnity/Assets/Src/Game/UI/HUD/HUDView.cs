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

        private Tween _playerSliderTween;
        private Tween _parentSliderTween;

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
                    _waveText.DOColor(Color.black, 1).SetLoops(2, LoopType.Yoyo);
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
            ChangeHealthBar(_healthBar, _healthBarImg, health,_playerSliderTween);
        }

        public void OnParentHealthChange(float health)
        {
            ChangeHealthBar(_parentHealthBar, _parenthealthBarImg, health, _parentSliderTween);
        }

        private void ChangeHealthBar(Slider slider,Image fill,float health, Tween t)
        {
            if(t != null)
            {
                t.Kill(true);
            }
            t = slider.DOValue(health, .1f).SetEase(Ease.InElastic);
            fill.color = health >= (slider.maxValue*0.35f) ? _healthyColor : _unhealthyColor;
        }
    }
}
