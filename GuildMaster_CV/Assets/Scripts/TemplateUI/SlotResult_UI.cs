using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotResult_UI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image _icon;
    [SerializeField] private Image _background;
    [SerializeField] private TextMeshProUGUI _stacksize;
    [SerializeField] private SlotData _slotData;

    private ChangeColorBackgroundItem _backColorItem = new ChangeColorBackgroundItem();

    //public void Init(Sprite icon, int tier, int stacksize)
    //{
    //    if (icon != null)
    //        _icon.sprite = icon;

    //    if (stacksize != 0)
    //        _stacksize.text = $"{stacksize}";

    //    if (_assignedItemSlot.CraftItemData is EquipItemData equipItemData)
    //    {
    //        _backgroundSprite.sprite = equipItemData.IconBackground;
    //        _backgroundSprite.color = _backColorItem.ChangeColor(equipItemData.Tier);
    //    }

    //    ChangeBackgroundColor(tier);
    //}

    public void Init(SlotData itemData, int stacksize)
    {
        _slotData = itemData;

        if (itemData.Icon != null)
            _icon.sprite = itemData.Icon;

        if (stacksize != 0)
            _stacksize.text = $"{stacksize}";

        if (_slotData is EquipItemData equipItemData)
        {
            _background.sprite = equipItemData.IconBackground;
            _background.color = _backColorItem.ChangeColor(equipItemData.Tier);
        }

        if(itemData.IconBackground == null)
            _background.color = new Color(1, 1, 1, 0); // (R, G, B, A)
        //_background.color = _background.color.WithAlpha(0);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        InfoPanelUI.Instance.ShowItemInfo(_slotData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InfoPanelUI.Instance.HideInfo();
    }
}
