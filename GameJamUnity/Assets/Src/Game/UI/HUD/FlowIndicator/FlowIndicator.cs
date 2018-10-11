using DG.Tweening;


//Unity
using UnityEngine;

namespace UI.HUD
{
    public class FlowIndicator : AbstractHUDElement
    {
        #region Components
        private EnemyCountText _enemyCount;
        private EnemyCountText _enemyCountText
        {
            get { return _enemyCount ?? (_enemyCount = GetUIComponent(_enemyCount)); }
        }

        private EnemyWaveText _enemyWave;
        private EnemyWaveText _enemyWaveText
        {
            get { return _enemyWave ?? (_enemyWave = GetUIComponent(_enemyWave)); }
        }

        private EnemyWaveBar _waveBar;
        private EnemyWaveBar _enemyWaveBar
        {
            get { return _waveBar ?? (_waveBar = GetUIComponent(_waveBar)); }
        }
        #endregion

        private HUDConfig.FlowIndicatorData _flowIndicatorData;

        private Tween _countFlashTween;
        private Tween _countIntroTween;
        private Tween _countScaleTween;

        private Tween _waveBarMorph;

        public override void OnCreationComplete(HUDConfig hudConfig)
        {
            _flowIndicatorData = hudConfig.flowIndicatorData;
            _enemyWaveBar.SetWidth(_flowIndicatorData.waveBarHiddenWidth);
            _enemyCountText.SetAlpha(0);
            _enemyCountText.DOKill();
        }

        public override void OnWaveChanged(LevelConfig.WaveDef def, int maxWaves)
        {
            CanvasGroup grid = _enemyCountText._canvasGroup;

            if (_countIntroTween != null)
            {
                _countIntroTween.Kill(true);
            }
            if (_countScaleTween != null)
            {
                _countScaleTween.Kill(true);
            }
            if (_waveBarMorph != null)
            {
                _waveBarMorph.Kill(true);
            }

            _countIntroTween = grid.DOFade(0, _flowIndicatorData.enemyCountFadeTime).SetEase(_flowIndicatorData.enemyCountFadeEaseType).OnComplete(() =>
            {
                _enemyCountText.SetText(def.enemyCount.ToString());

                _countIntroTween = grid.DOFade(1, _flowIndicatorData.enemyCountFadeTime).
                SetEase(_flowIndicatorData.enemyCountFadeEaseType).
                OnComplete(() =>
                {
                    _countScaleTween = _enemyCountText.transform.DOScale(_flowIndicatorData.enemyCountScale, _flowIndicatorData.enemyCountScaleTime).
                    SetEase(_flowIndicatorData.enemyCountScaleEaseType).
                    SetLoops(2, LoopType.Yoyo);
                });
            });

            RectTransform bar = _enemyWaveBar._rectTransform;
            _waveBarMorph = bar.DOSizeDelta(new Vector2(_flowIndicatorData.waveBarHiddenWidth, bar.sizeDelta.y), _flowIndicatorData.waveBarMorphSpeed).
                SetEase(_flowIndicatorData.waveBarMorphEaseType).OnComplete(() =>
                {
                    //Set Wave Progress Text
                    _enemyWaveText.SetText((def.waveIndex + 1).ToString() + "/" + maxWaves);
                    _waveBarMorph = bar.DOSizeDelta(new Vector2(_flowIndicatorData.waveBarVisibleWidth, bar.sizeDelta.y), _flowIndicatorData.waveBarMorphSpeed).
                    SetEase(_flowIndicatorData.waveBarMorphEaseType);
                });
        }

        public override void OnEnemyCountChanged(int count)
        {
            if (_countFlashTween != null)
            {
                _countFlashTween.Kill(true);
            }
            //Set Enemy Counter Text
            _enemyCountText.SetText(count.ToString());

            _enemyCountText.SetTextColor(Color.white);
            _countFlashTween = _enemyCountText.
                _text.DOColor(_flowIndicatorData.enemyCountFlashColor, _flowIndicatorData.enemyCountFlashTime).
                SetEase(Ease.Flash).
                SetLoops(2, LoopType.Yoyo);
        }
    }
}
