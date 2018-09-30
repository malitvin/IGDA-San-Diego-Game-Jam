//Unity
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Gameplay.Building;

using System.Collections;

public class BuildableUIItem : MonoBehaviour {

    public TextMeshProUGUI _text;
    public TextMeshProUGUI _hotKey;

    public Image _hotKeyBackground;
    public RawImage _renderImage;
    public Image blur;
    private Camera _cam;
    private Camera _renderCam
    {
        get { return _cam ?? (_cam = GetComponentInChildren<Camera>()); }
    }

    private Buildable _prototype;

    public void SetContent(BuildConfig.BuildableBlueprint blueprint,int positionOffset)
    {
        //do cool ass render texture building prototypes (like fortnite) 
        _text.text = blueprint.currencyCost.ToString();
        _hotKey.text = blueprint.buildHotKey.ToString().Replace("Alpha", "");
        RenderTexture texture = new RenderTexture(128, 128, 16, RenderTextureFormat.ARGB32);
        texture.Create();
        _renderCam.targetTexture = texture;
        _renderCam.clearFlags = CameraClearFlags.Color;
        _renderCam.transform.position += new Vector3(positionOffset * 5, 0, 0);
        _renderImage.texture = texture;

        _prototype = GameObject.Instantiate(blueprint.prefab) as Buildable;
        _prototype.gameObject.SetActive(true);
        _prototype.transform.parent = _renderCam.transform;
        _prototype.gameObject.layer = 10;
        _prototype.transform.position = Vector3.zero;

        ToggleItem(false);

        StartCoroutine(Refresh());
    }

    private IEnumerator Refresh()
    {
        yield return new WaitForEndOfFrame();
        _prototype.transform.localPosition = new Vector3(0, 0, 190);
    }

    private void Update()
    {
        _prototype.transform.eulerAngles += new Vector3(12 * Time.deltaTime, 12 * Time.deltaTime, 0);
    }

    public void ToggleItem(bool on)
    {
        _hotKeyBackground.color = on ? Color.green : Color.red;
        blur.enabled = on;
    }

}
