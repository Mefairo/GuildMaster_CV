using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RuneSlot : InventorySlot
{
    [SerializeField] private RuneSlotData _runeSlotData;
    [SerializeField] private int _tier;

    public override SlotData BaseItemData => _runeSlotData;

    public RuneSlotData RuneSlotData => _runeSlotData;
    public int Tier => _tier;

    public RuneSlot(RuneSlotData runeSlotData, int level)
    {
        _runeSlotData = runeSlotData;
        _tier = runeSlotData.Tier;

        _itemData = runeSlotData;
        _itemID = runeSlotData.ID;

        _iconSlot = runeSlotData.Icon;
    }

    public RuneSlot(SlotData data, int size, int tier) : base(data, size)
    {
        _runeSlotData = data as RuneSlotData;

        _stackSize = size;
        _tier = tier;

        _itemData = data;
        _itemID = data.ID;

        _iconSlot = data.Icon;
    }

    public RuneSlot(RuneSlot runeSlot, int index)
    {
        _runeSlotData = runeSlot.RuneSlotData;
        _stackSize = runeSlot.StackSize;
        _tier = runeSlot.Tier;
        _index = index;

        _itemData = runeSlot.BaseItemData;
        _itemID = runeSlot.BaseItemData.ID;

        _iconSlot = runeSlot.IconSlot;
    }

    public RuneSlot() { }

    public override void AssignItem(InventorySlot slot)
    {
        _itemData = slot.ItemData;
        _itemID = slot.ItemData.ID;
        _stackSize = 0;

        _iconSlot = slot.IconSlot;

        AddToStack(slot.StackSize);

        if (slot.ItemData is RuneSlotData runeData)
        {
            _runeSlotData = runeData;
            _tier = runeData.Tier;
        }

        else
            _runeSlotData = null;
    }

    public override void ClearSlot()
    {
        base.ClearSlot();

        _runeSlotData = null;
        _tier = 0;
    }

    public override InventorySlot CloneSlot()
    {
        return new RuneSlot(ItemData, StackSize, _tier);
    }
}
