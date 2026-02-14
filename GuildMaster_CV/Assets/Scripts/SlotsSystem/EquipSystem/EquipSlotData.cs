using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EquipSlotData
{
    [SerializeField] private SlotData _itemData;
    [SerializeField] private int _itemTier;

    public SlotData ItemData => _itemData;
    public int ItemTier => _itemTier;

    public EquipSlotData(SlotData itemData, int itemTier)
    {
        _itemData = itemData;
        _itemTier = itemTier;
    }

    public EquipSlotData()
    {
        Clear();
    }

    public void Clear()
    {
        _itemData = null;
        _itemTier = 0;
    }
}
