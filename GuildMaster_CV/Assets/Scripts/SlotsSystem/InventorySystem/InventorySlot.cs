using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using static Unity.VisualScripting.Member;

[System.Serializable]
public class InventorySlot : Slot
{
    protected virtual SlotData _baseItemData => ItemData;
    public virtual SlotData BaseItemData => _baseItemData;
    [SerializeField] protected int _index = -1;

    //public int Index => _index;
    public int Index => _index;



    public InventorySlot(SlotData source, int amount)
    {
        //Debug.Log("1");
        _itemData = source;
        _stackSize = amount;
        _itemID = _itemData.ID;

        SetIconSlot(source.Icon);
    }

    public InventorySlot(SlotData source, int amount, int index)
    {
        //Debug.Log("1");
        _itemData = source;
        _stackSize = amount;
        _itemID = _itemData.ID;
        _index = index;

        SetIconSlot(source.Icon);
    }

    public InventorySlot(int index)
    {
        ClearSlot();
        _index = index;
    }

    public InventorySlot() { }

    public override void AssignItem(InventorySlot invSlot)
    {
        if (_baseItemData == invSlot.BaseItemData)
            AddToStack(invSlot._stackSize);

        else
        {
            _itemData = invSlot.BaseItemData;
            _itemID = _baseItemData.ID;
            _stackSize = 0;

            SetIconSlot(invSlot.IconSlot);

            AddToStack(invSlot._stackSize);
        }
    }

    public void SetIndex(int index)
    {
        _index = index;
    }

    public void UpdateInventorySlot(SlotData data, int amount)
    {
        _itemData = data;
        _stackSize = amount;
        _itemID = _itemData.ID;

        SetIconSlot(_itemData.Icon);
    }

    public bool RoomLeftInStack(int amountToAdd, out int amountRemaining)
    {
        amountRemaining = ItemData.MaxStackSize - _stackSize;
        return EnoughRoomLeftInStack(amountToAdd);

    }

    public bool EnoughRoomLeftInStack(int amountToAdd)
    {
        if (_itemData == null || _itemData != null && _stackSize + amountToAdd <= _itemData.MaxStackSize)
            return true;

        else
            return false;
    }



    public bool SplitStack(out InventorySlot splitStack)
    {
        if (_stackSize <= 1)
        {
            splitStack = null;
            return false;
        }

        int halfStack = Mathf.RoundToInt(_stackSize / 2);
        RemoveFromStack(halfStack);

        splitStack = new InventorySlot(_baseItemData, halfStack);
        return true;
    }

    public virtual InventorySlot CloneSlot()
    {
        return new InventorySlot(ItemData, StackSize);
    }

    public void ChangeStacksize(int stacksize)
    {
        _stackSize = stacksize;
    }
}
