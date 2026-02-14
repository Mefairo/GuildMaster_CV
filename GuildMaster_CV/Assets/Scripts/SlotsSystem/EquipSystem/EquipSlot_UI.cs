using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipSlot_UI : InventorySlot_UI
{
    [Space(2)]
    [SerializeField] private EquipType _itemType;
    [SerializeField] private EquipType _secondaryItemType;
    [SerializeField] private bool _isCantEquip;
    [SerializeField] private Image _equipTypeBackground;
    [SerializeField] private int _equipIndex = -1;

    [SerializeField] private EquipSlot _assignedEquipSlot;

    public EquipType ItemType => _itemType;
    public EquipType SecondaryItemType => _secondaryItemType;
    public bool IsCantEquip => _isCantEquip;
    public EquipSlot AssignedEquipSlot => _assignedEquipSlot;
    public int EquipIndex => _equipIndex;

    public DynamicEquipDisplay ParentEquipmentDisplay { get; private set; }

    private void Awake()
    {
        ClearSlot();

        _itemSprite.preserveAspect = true;

        _button = GetComponent<Button>();
        _button?.onClick.AddListener(OnUISlotClick);

        CheckEquipBackground();

        ParentEquipmentDisplay = transform.parent.GetComponent<DynamicEquipDisplay>();
    }

    public override void OnUISlotRightClick()
    {
        //if (!_isCantEquip)
        //    ParentEquipmentDisplay.RightClickSlot(this);
    }

    public override void OnUISlotClick()
    {
        ParentEquipmentDisplay?.SlotLeftClicked(this);
        CheckEquipBackground();
    }

    public override void Init(InventorySlot slot)
    {
        base.Init(slot);
        AssignEquipSlotData(slot);
    }

    private void AssignEquipSlotData(InventorySlot slot)
    {
        // Check if the slot's item data is of type EquipItemData and assign it to _assignedEquipSlot
        if (slot is BlankSlot blankSlot)
        {
            _assignedEquipSlot = blankSlot;
            _assignedEquipSlot.SetEquipItemData(blankSlot.BlankSlotData);
            //_assignedEquipSlot.AssignItem(slot);
        }

        else if(slot is EquipSlot equipSlot)
        {
            _assignedEquipSlot = equipSlot;
            _assignedEquipSlot.SetEquipItemData(equipSlot.EquipItemData);
        }

        else
        {
            _assignedEquipSlot = null;
        }
    }

    public override void UpdateUISlot(InventorySlot slot)
    {
        base.UpdateUISlot(slot);

        if (slot != null && slot.BaseItemData != null)
            _equipTypeBackground.gameObject.SetActive(false);

        else
            _equipTypeBackground.gameObject.SetActive(true);
    }

    public void UpdateEquipSlotUI()
    {
        if (_assignedEquipSlot != null)
        {
            _itemSprite.sprite = _assignedEquipSlot.IconSlot;
            _itemSprite.color = Color.white;

            if (_assignedEquipSlot.StackSize > 1)
            {
                _itemCount.text = _assignedEquipSlot.StackSize.ToString();
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

    public void CheckEquipBackground()
    {
        if (_assignedInventorySlot != null && _assignedInventorySlot.BaseItemData != null)
            _equipTypeBackground.gameObject.SetActive(false);

        else
            _equipTypeBackground.gameObject.SetActive(true);
    }
}
