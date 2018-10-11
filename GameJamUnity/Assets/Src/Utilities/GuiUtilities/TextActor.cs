//Unity
using UnityEngine;

//TMPRO
using TMPro;

/// <summary>
/// Wrapper for Text Mesh Pro Class
/// </summary>
[RequireComponent(typeof(TextMeshProUGUI))]
public abstract class TextActor : UIActor
{
    private TextMeshProUGUI _txt;
    public TextMeshProUGUI _text
    {
        get { return _txt ?? (_txt = GetComponent<TextMeshProUGUI>()); }
    }

    public void SetText(string text)
    {
        _text.text = text;
    }

    public void SetTextColor(Color col)
    {
        _text.color = col;
    }
}
