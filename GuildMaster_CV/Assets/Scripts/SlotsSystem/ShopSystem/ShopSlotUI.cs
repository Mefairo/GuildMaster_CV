using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopSlotUI : MonoBehaviour
{
    [Header("Preview Item")]
    [SerializeField] private Image _itemSprite;
    [SerializeField] private Image _backgroundSprite;
    [SerializeField] private TextMeshProUGUI _itemName;
    [SerializeField] private TextMeshProUGUI _itemCount;
    [SerializeField] private TextMeshProUGUI _itemPrice;
    [SerializeField] private ShopSlot _assignedItemSlot;
    [Space]
    [Header("Other")]
    [SerializeField] private Button _addItemToCartButton;
    [SerializeField] private Button _removeItemFromCartButton;
    [SerializeField] private Button _updatePreviewButton;
    //[Space]
    //[SerializeField] private ItemsShowInfo _panelInfo;

    public ShopSlot AssignedItemSlot => _assignedItemSlot;
    public ShopKeeperDisplay ParentDisplay { get; private set; }

    public float MarkUp { get; private set; }

    private int _tempAmount;
    private ChangeColorBackgroundItem _backColorItem = new ChangeColorBackgroundItem();

    private void Awake()
    {
        ClearSlot();

        _addItemToCartButton?.onClick.AddListener(AddItemToCart);
        _removeItemFromCartButton?.onClick.AddListener(RemoveItemFromCart);
        _updatePreviewButton?.onClick.AddListener(ClickSlot);

        ParentDisplay = GetComponentInParent<ShopKeeperDisplay>();
    }

    public void Init(ShopSlot slot, float markUp)
    {
        _assignedItemSlot = slot;
        MarkUp = markUp;
        _tempAmount = slot.StackSize;

        UpdateUISlot();
    }

    private void UpdateUISlot()
    {
        if (_assignedItemSlot != null)
        {
            _itemSprite.sprite = _assignedItemSlot.IconSlot;
            _itemSprite.color = Color.white;

            if (_assignedItemSlot.ItemData is EquipItemData equipItemData)
            {
                _backgroundSprite.sprite = equipItemData.IconBackground;
                _backgroundSprite.color = _backColorItem.ChangeColor(equipItemData.Tier);
            }

            int modifiedPrice = ShopKeeperDisplay.GetModifiedPrice(_assignedItemSlot.ItemData, 1, MarkUp);

            _itemName.text = $"{_assignedItemSlot.ItemData.DisplayName}";
            _itemCount.text = $"Amount: {_assignedItemSlot.StackSize}";
            _itemPrice.text = modifiedPrice.ToString();
        }
        else
            ClearSlot();
    }

    private void AddItemToCart()
    {
        if (_tempAmount <= 0)
            return;

        _tempAmount--;
        ParentDisplay.AddItemToCart(this);

        _itemCount.text = $"Amount: {_tempAmount}";
        //_itemCount.text = _tempAmount.ToString();
    }

    private void RemoveItemFromCart()
    {
        if (_tempAmount == _assignedItemSlot.StackSize)
            return;

        _tempAmount++;
        ParentDisplay.RemoveItemFromCart(this);

        _itemCount.text = $"Amount: {_tempAmount}";
        //_itemCount.text = _tempAmount.ToString();
    }

    public void ClickSlot()
    {
        //if (_assignedItemSlot.AssignedInvSlot == null)
        ParentDisplay.UpdateItemPreview(this.AssignedItemSlot.ItemData);

        //else
        //    ParentDisplay.UpdateItemPreview(this.AssignedItemSlot.AssignedInvSlot);
    }

    private void ClearSlot()
    {
        _itemSprite.sprite = null;
        _itemSprite.color = Color.clear;

        _backgroundSprite.sprite = null;
        _backgroundSprite.color = Color.clear;

        _itemName.text = "";
        _itemCount.text = "";
        _itemPrice.text = "";
    }
}
