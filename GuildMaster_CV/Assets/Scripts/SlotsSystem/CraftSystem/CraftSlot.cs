using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CraftSlot : Slot
{
   [SerializeField] private CraftItemData _craftItemData;

    public CraftItemData CraftItemData => _craftItemData;

    public CraftSlot(CraftItemData itemData)
    {
        _itemData = itemData;
        _craftItemData = itemData;
    }

    public CraftSlot()
    {
        ClearSlot();
    }

    public void AssignItem(SlotData data)
    {
        if (_itemData == data)
            return;
        //AddToStack(amount);

        else
        {
            _itemData = data;
            _craftItemData = (CraftItemData)data;
            _itemID = data.ID;
            //stackSize = 0;
            //AddToStack(amount);
        }
    }

}
