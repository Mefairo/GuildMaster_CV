using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class InventorySlot_UI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected Image _itemSprite;
    [SerializeField] protected Image _backgroundSprite;
    [SerializeField] protected TextMeshProUGUI _itemCount;
    [SerializeField] protected InventorySlot _assignedInventorySlot;
    //[Space]
    //[SerializeField] protected GameObject _slotHighlight;
    //[SerializeField] protected ItemsShowInfo _panelInfo;

    protected Button _button;
    private ChangeColorBackgroundItem _backColorItem = new ChangeColorBackgroundItem();

    public InventorySlot AssignedInventorySlot => _assignedInventorySlot;
    public InventoryDisplay ParentDisplay { get; private set; }

    private void Awake()
    {
        ClearSlot();

        _itemSprite.preserveAspect = true;
        //_backgroundSprite.preserveAspect = true;

        _button = GetComponent<Button>();
        _button?.onClick.AddListener(OnUISlotClick);

        ParentDisplay = transform.parent.GetComponent<InventoryDisplay>();
    }

    private void Start()
    {
        //_panelInfo = UIManager.Instance.PanelInfo;

        //if (_panelInfo != null)
        //    _panelInfo.gameObject.SetActive(false);
    }

    public virtual void Init(InventorySlot slot)
    {
        _assignedInventorySlot = slot;
        UpdateUISlot(slot);
    }

    public virtual void UpdateUISlot(InventorySlot slot)
    {
        if (slot.BaseItemData != null)
        {
            _itemSprite.sprite = slot.BaseItemData.Icon;
            _itemSprite.color = Color.white;
            _backgroundSprite.sprite = slot.BaseItemData.IconBackground;

            _backColorItem.SetBackgroundItem(slot, _backgroundSprite);

            if (slot.BaseItemData is EquipItemData equipItemData)
            {
                //_backgroundSprite.sprite = equipItemData.IconBackground;
                //_backgroundSprite.color = _backColorItem.ChangeColor(equipItemData.Tier);
            }


            if (slot.StackSize > 1)
            {
                _itemCount.text = slot.StackSize.ToString();
            }
            else
            {
                _itemCount.text = "";
            }
        }

        else
        {
            ClearSlot();
        }

    }

    public void UpdateUISlot()
    {
        if (_assignedInventorySlot != null)
        {
            UpdateUISlot(_assignedInventorySlot);
        }
    }

    public void ClearSlot()
    {
        _itemSprite.sprite = null;
        _itemSprite.color = Color.clear;

        _backgroundSprite.sprite = null;
        _backgroundSprite.color = Color.clear;

        _itemCount.text = "";
    }

    public virtual void OnUISlotClick()
    {
        ParentDisplay?.SlotClicked(this);
    }

    public virtual void OnUISlotRightClick()
    {
        ParentDisplay?.SlotRightClicked(this);
    }

    //internal void ToggleHighlight()
    //{
    //    _slotHighlight.SetActive(!_slotHighlight.activeInHierarchy);
    //}

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (_assignedInventorySlot is BlankSlot blankSlot)
            InfoPanelUI.Instance.ShowBlankInfo(blankSlot);

        else if (_assignedInventorySlot is RuneSlot runeSlot)
            InfoPanelUI.Instance.ShowRuneInfo(runeSlot);

        else if (_assignedInventorySlot is CatalystSlot catalystSlot)
            InfoPanelUI.Instance.ShowCatalystInfo(catalystSlot);

        else
            InfoPanelUI.Instance.ShowItemInfo(_assignedInventorySlot.ItemData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InfoPanelUI.Instance.HideInfo();
    }
}
