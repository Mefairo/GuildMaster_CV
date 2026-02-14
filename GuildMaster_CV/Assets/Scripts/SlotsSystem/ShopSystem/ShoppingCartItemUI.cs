using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShoppingCartItemUI : MonoBehaviour
{
    [SerializeField] private ShopSlot _slot;

    [SerializeField] private SlotData _itemData;
    [SerializeField] private TextMeshProUGUI _itemText;
    [SerializeField] private TextMeshProUGUI _amountText;
    [SerializeField] private Image _itemSprite;
    [SerializeField] private Image _backgroundSprite;
    [SerializeField] private Button _buttonSelf;
    [SerializeField] private InventorySlot _assignedInvSlot;

    public InventorySlot AssignedInvSlot => _assignedInvSlot;

    private ChangeColorBackgroundItem _backColorItem = new ChangeColorBackgroundItem();

    public SlotData ItemData => _itemData;
    public ShopSlot Slot => _slot;

    public ShopKeeperDisplay ShopKeeperDisplay { get; private set; }

    private void Awake()
    {
        ShopKeeperDisplay = GetComponentInParent<ShopKeeperDisplay>();

        _buttonSelf?.onClick.AddListener(ClickSlot);
    }

    public void SetItemText(SlotData itemData, string newAmount)
    {
        _itemData = itemData;
        _itemText.text = $"- {itemData.DisplayName}";
        _amountText.text = newAmount;
        _itemSprite.sprite = itemData.Icon;

        if (itemData is EquipItemData equipItemData)
        {
            _backgroundSprite.sprite = equipItemData.IconBackground;
            _backgroundSprite.color = _backColorItem.ChangeColor(equipItemData.Tier);
        }
        else
        {
            _backgroundSprite.sprite = null;
            _backgroundSprite.color = Color.clear;
        }
    }

    private void ClickSlot()
    {
        if (ShopKeeperDisplay != null)
        {
            //if (_assignedInvSlot == null)
            ShopKeeperDisplay.UpdateItemPreview(_itemData);

            //else
            //    ShopKeeperDisplay.UpdateItemPreview(_assignedInvSlot);
        }
    }
}

