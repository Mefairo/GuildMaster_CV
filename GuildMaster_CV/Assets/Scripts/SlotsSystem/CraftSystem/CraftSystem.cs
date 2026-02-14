using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[System.Serializable]
public class CraftSystem
{
    [SerializeField] private List<CraftSlot> _craftList;

    public List<CraftSlot> CraftList => _craftList;

    public CraftSystem(int size)
    {
        SetCraftSize(size);
    }

    private void SetCraftSize(int size)
    {
        _craftList = new List<CraftSlot>();

        for (int i = 0; i < size; i++)
        {
            _craftList.Add(new CraftSlot());
        }
    }

    public void AddToCraft(CraftItemData data)
    {
        if (ContainsItem(data, out CraftSlot craftSlot))
        {
            return;
        }

        var freeSlot = GetFreeSlot();
        freeSlot.AssignItem(data);
    }

    private CraftSlot GetFreeSlot()
    {
        var freeslot = _craftList.FirstOrDefault(i => i.ItemData == null);
        var freeslot1 = _craftList.FirstOrDefault(i => i.CraftItemData == null);

        if (freeslot == null)
        {
            freeslot = new CraftSlot();
            freeslot1 = new CraftSlot();
            _craftList.Add(freeslot);
        }

        return freeslot;
    }

    public bool ContainsItem(CraftItemData itemToAdd, out CraftSlot craftSlot)
    {
        craftSlot = _craftList.Find(i => i.ItemData == itemToAdd);
        craftSlot = _craftList.Find(i => i.CraftItemData == itemToAdd);

        return craftSlot != null;
    }
}
