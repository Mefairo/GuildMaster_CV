using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CatalystSlot : InventorySlot
{
    [SerializeField] private CatalystSlotData _catalystSlotData;

    public CatalystSlotData CatalystSlotData => _catalystSlotData;

    public override SlotData BaseItemData => _catalystSlotData;

    public CatalystSlot(CatalystSlotData catalystSlotData)
    {
        _catalystSlotData = catalystSlotData;
        _iconSlot = catalystSlotData.Icon;
    }

    public CatalystSlot(SlotData data, int size) : base(data, size)
    {
        _catalystSlotData = data as CatalystSlotData;
        _stackSize = size;

        _itemData = data;
        _itemID = data.ID;

        _iconSlot = data.Icon;
    }

    public CatalystSlot(CatalystSlot catalystSlot, int index)
    {
        _catalystSlotData = catalystSlot.CatalystSlotData;
        _stackSize = catalystSlot.StackSize;    
        _index = index;

        _itemData = catalystSlot.BaseItemData;
        _itemID = catalystSlot.BaseItemData.ID;

        _iconSlot = catalystSlot.IconSlot;
    }

    public CatalystSlot() { }

    public override void AssignItem(InventorySlot slot)
    {
        _itemData = slot.ItemData;
        _itemID = slot.BaseItemData.ID;
        _stackSize = 0;

        _iconSlot = slot.IconSlot;

        AddToStack(slot.StackSize);

        if (slot is CatalystSlot catalyst)
        {
            _catalystSlotData = catalyst.CatalystSlotData;
        }

        else
            _catalystSlotData = null;
    }

    public override void ClearSlot()
    {
        base.ClearSlot();

        _catalystSlotData = null;
    }

    public override InventorySlot CloneSlot()
    {
        return new CatalystSlot(ItemData, StackSize);
    }
}
