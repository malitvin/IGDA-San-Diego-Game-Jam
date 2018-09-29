using UnityEngine.UI;
using TMPro;
using UnityEngine;
using Gameplay.Inventory;

public class BuildCurrencyItem : MonoBehaviour {

    public Image _itemSprite;
    public TextMeshProUGUI _text;

    public void SetContent(InventoryConfig.InventoryItem item)
    {
        _itemSprite.sprite = item.inventoryUISprite;
        _text.text = item.startAmount.ToString();
    }
}
