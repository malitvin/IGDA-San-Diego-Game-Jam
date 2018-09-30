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
        SetText(item.startAmount);
    }

    public void SetText(int amount)
    {
        _text.text = amount.ToString();
    }
}
