using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PossibleRewardSlot_UI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image _icon;
    [SerializeField] private Image _background;
    [SerializeField] private SlotData _slotData;

    private ChangeColorBackgroundItem _backColorItem = new ChangeColorBackgroundItem();

    public void Init(SlotData itemData)
    {
        _slotData = itemData;

        if (itemData.Icon != null)
            _icon.sprite = itemData.Icon;

        if (_slotData is EquipItemData equipItemData)
        {
            _background.sprite = equipItemData.IconBackground;
            _background.color = _backColorItem.ChangeColor(equipItemData.Tier);
        }

        if (itemData.IconBackground == null)
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
