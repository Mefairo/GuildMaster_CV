using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ItemKey
{
    public SlotData ItemData;
    public int ItemTier;

    public ItemKey(SlotData itemData, int itemTier)
    {
        ItemData = itemData;
        ItemTier = itemTier;
    }
}
