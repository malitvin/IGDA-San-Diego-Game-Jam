//Unity
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public abstract class UIActor : MonoBehaviour
{
    #region Components
    private CanvasGroup _grid;
    public CanvasGroup _canvasGroup
    {
        get
        {
            if (_grid)
            {
                return _grid;
            }
            else
            {
                _grid = GetComponent<CanvasGroup>();
                if (_grid)
                {
                    return _grid;
                }
                else
                {
                    return gameObject.AddComponent<CanvasGroup>();
                }
            }
        }
    }

    private RectTransform _rect;
    public RectTransform _rectTransform
    {
        get { return _rect ?? (_rect = GetComponent<RectTransform>()); }
    }
    #endregion

    public void SetAlpha(float alpha)
    {
        _canvasGroup.alpha = alpha;
    }

    public void SetWidth(float width)
    {
        _rectTransform.sizeDelta = new Vector2(width, _rectTransform.sizeDelta.y);
    }

    #region Tweens

    #endregion

    #region Helpers
    protected T GetUIComponent<T>(T Object) where T : Component
    {
        Object = GetComponent<T>();
        if (Object != null)
        {
            return Object;
        }
        Object = GetComponentInChildren<T>();
        if (Object == null)
        {
            Debug.LogError(typeof(T) + " is not attached to  " + gameObject.name);
        }
        return Object;
    }
    #endregion
}
