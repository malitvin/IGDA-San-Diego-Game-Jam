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

        public void OnCreationComplete()
        {
            _waveGrid.alpha = 0;
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
    }
}
