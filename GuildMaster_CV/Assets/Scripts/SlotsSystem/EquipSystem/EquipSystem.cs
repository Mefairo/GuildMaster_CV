using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using System.Drawing;

[System.Serializable]
public class EquipSystem 
{
    [SerializeField] private List<EquipSlot> _slots;

    public List<EquipSlot> Slots => _slots;

    public UnityAction<EquipSlot> OneEquipSlotChanged;
    public int EquipmentSize => _slots.Count;

    public EquipSystem()
    {
        _slots = new List<EquipSlot>();

        for (int i = 0; i < _slots.Count; i++)
            _slots.Add(new EquipSlot(i));
    }

    public EquipSystem(int size)
    {
        CreateEquipment(size);
    }

    public EquipSystem(EquipSystem copyEquipSystem)
    {
        _slots = new List<EquipSlot>(copyEquipSystem.Slots.Count);

        for (int i = 0; i < copyEquipSystem.Slots.Count; i++)
        {
            _slots.Add(copyEquipSystem.Slots[i]);
        }
    }

    private void CreateEquipment(int size)
    {
        _slots = new List<EquipSlot>(size);

        for (int i = 0; i < size; i++)
        {
            _slots.Add(new EquipSlot());
        }
    }

    public void ResetEquipIndex()
    {
        for (int i = 0; i < _slots.Count; i++)
            _slots[i].SetEquipIndex(i);
    }


    //public bool AddToInventory(SlotData itemToAdd, int amountToAdd)
    //{
    //    Debug.Log("Add eq");
    //    if (ContainsItem(itemToAdd, out List<EquipSlot> invSlot))  //Существует ли предмет в инвентаре
    //    {
    //        foreach (var slot in invSlot)
    //        {
    //            if (slot.EnoughRoomLeftInStack(amountToAdd))
    //            {
    //                slot.AddToStack(amountToAdd);
    //                OneEquipSlotChanged?.Invoke(slot);
    //                return true;
    //            }
    //        }

    //    }
    //    if (HasFreeSlot(out EquipSlot freeSlot))  // Получает первый доступный слот
    //    {
    //        if (freeSlot.EnoughRoomLeftInStack(amountToAdd))
    //        {
    //            freeSlot.UpdateInventorySlot(itemToAdd, amountToAdd);
    //            OneEquipSlotChanged?.Invoke(freeSlot);
    //            return true;
    //        }
    //    }

    //    return false;

    //}

    //public bool ContainsItem(SlotData itemToAdd, out List<EquipSlot> invSlot)
    //{
    //    invSlot = Slots.Where(i => i.ItemData == itemToAdd).ToList();

    //    return invSlot == null ? false : true;
    //}

    //public bool HasFreeSlot(out EquipSlot freeSlot)
    //{
    //    freeSlot = Slots.FirstOrDefault(i => i.ItemData == null);
    //    return freeSlot == null ? false : true;
    //}

    public Dictionary<SlotData, int> GetAllItemHeld()
    {
        var distinctItems = new Dictionary<SlotData, int>();

        foreach (var item in _slots)
        {
            if (item.ItemData == null)
                continue;

            if (!distinctItems.ContainsKey(item.ItemData))
                distinctItems.Add(item.ItemData, item.StackSize);

            else
                distinctItems[item.ItemData] += item.StackSize;
        }

        return distinctItems;
    }

    //public void RemoveItemsFromInventory(SlotData data, int amount)
    //{
    //    if (ContainsItem(data, out List<EquipSlot> invSlot))
    //    {
    //        foreach (var slot in invSlot)
    //        {
    //            var stackSize = slot.StackSize;

    //            if (stackSize > amount)
    //            {
    //                slot.RemoveFromStack(amount);
    //                OneEquipSlotChanged?.Invoke(slot);
    //                return;
    //            }

    //            else
    //            {
    //                slot.RemoveFromStack(stackSize);
    //                amount -= stackSize;
    //                OneEquipSlotChanged?.Invoke(slot);
    //            }


    //        }

    //    }
    //}
}
