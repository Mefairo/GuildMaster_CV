using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using System;

[System.Serializable]
public class InventorySystem
{
    [SerializeField] private List<InventorySlot> _inventorySlots;
    [SerializeField] private int _gold;

    public event UnityAction<int> OnChangeGold;
    public UnityAction<InventorySlot> OnInventorySlotChanged;


    public int Gold
    {
        get => _gold;
        private set
        {
            _gold = value;
            OnChangeGold?.Invoke(value);
        }
    }

    public List<InventorySlot> InventorySlots => _inventorySlots;
    public int InventorySize => InventorySlots.Count;

    public InventorySystem(int size)
    {
        _gold = 0;

        CreateInventory(size);
    }

    public InventorySystem(int size, int gold)
    {
        _gold = gold;

        CreateInventory(size);
    }

    private void CreateInventory(int size)
    {
        _inventorySlots = new List<InventorySlot>(size);

        for (int i = 0; i < size; i++)
        {
            _inventorySlots.Add(new InventorySlot(i));
        }

    }

    public bool AddToInventory(SlotData itemToAdd, int amountToAdd)
    {
        if (ContainsItem(itemToAdd, out List<InventorySlot> invSlot))  //Существует ли предмет в инвентаре
        {
            foreach (var slot in invSlot)
            {
                if (slot.EnoughRoomLeftInStack(amountToAdd))
                {
                    slot.AddToStack(amountToAdd);
                    OnInventorySlotChanged?.Invoke(slot);
                    return true;
                }
            }
        }

        if (HasFreeSlot(out InventorySlot freeSlot, out int freeSlotIndex))  // Нужно знать индекс!
        {
            if (freeSlot.EnoughRoomLeftInStack(amountToAdd))
            {
                _inventorySlots[freeSlotIndex] = new InventorySlot(itemToAdd, amountToAdd, freeSlotIndex);
                OnInventorySlotChanged?.Invoke(_inventorySlots[freeSlotIndex]);
                return true;
            }
        }

        //if (HasFreeSlot(out InventorySlot freeSlot, out int freeSlotIndex))  // Получает первый доступный слот
        //{
        //    if (freeSlot.EnoughRoomLeftInStack(amountToAdd))
        //    {
        //        freeSlot.UpdateInventorySlot(itemToAdd, amountToAdd);
        //        OnInventorySlotChanged?.Invoke(freeSlot);
        //        return true;
        //    }
        //}

        return false;
    }

    public bool AddToInventory(InventorySlot itemToAdd, int amountToAdd)
    {
        if (ContainsItem(itemToAdd.BaseItemData, out List<InventorySlot> invSlot))  //Существует ли предмет в инвентаре
        {
            foreach (var slot in invSlot)
            {
                if (slot.EnoughRoomLeftInStack(amountToAdd))
                {
                    slot.AddToStack(amountToAdd);
                    OnInventorySlotChanged?.Invoke(slot);
                    return true;
                }
            }
        }

        if (HasFreeSlot(out InventorySlot freeSlot, out int freeSlotIndex))  // Нужно знать индекс!
        {
            if (freeSlot.EnoughRoomLeftInStack(amountToAdd))
            {
                CheckedTypeSlot(itemToAdd, freeSlotIndex);
                InventorySlot newSlot = _inventorySlots[freeSlotIndex];
                newSlot.UpdateInventorySlot(itemToAdd.BaseItemData, amountToAdd);
                OnInventorySlotChanged?.Invoke(newSlot);
                return true;
            }
        }

        return false;
    }

    public InventorySlot ChangeTypeSlot1(InventorySlot itemToAdd, int indexItem)
    {
        InventorySlot newSlot;

        if (itemToAdd is BlankSlot blank)
            newSlot = new BlankSlot(blank, indexItem);

        else if (itemToAdd is RuneSlot rune)
            newSlot = new RuneSlot(rune, indexItem);

        else if(itemToAdd is CatalystSlot catalyst)
            newSlot = new CatalystSlot(catalyst, indexItem);

        else if (itemToAdd is EquipSlot equip)
            newSlot = new EquipSlot(equip, indexItem);

        else
            newSlot = new InventorySlot(itemToAdd.BaseItemData, itemToAdd.StackSize, indexItem);

        newSlot.UpdateInventorySlot(itemToAdd.BaseItemData, itemToAdd.StackSize);
        OnInventorySlotChanged?.Invoke(newSlot);
        return newSlot;
    }

    private void CheckedTypeSlot(InventorySlot invSlot, int index)
    {
        if (invSlot is BlankSlot blank)
            _inventorySlots[index] = new BlankSlot(blank, index);

        else if (invSlot is RuneSlot rune)
            _inventorySlots[index] = new RuneSlot(rune, index);

        else if (invSlot is CatalystSlot catalyst)
            _inventorySlots[index] = new CatalystSlot(catalyst, index);

        else if (invSlot is EquipSlot equip)
            _inventorySlots[index] = new EquipSlot(equip, index);

        else if (invSlot is InventorySlot inventorySlot)
            _inventorySlots[index] = new InventorySlot(inventorySlot.BaseItemData, inventorySlot.StackSize, index);
    }

    public bool ContainsItem(SlotData itemToAdd, out List<InventorySlot> invList)
    {
        invList = InventorySlots.Where(i => i.ItemData == itemToAdd).ToList();

        return invList == null ? false : true;
    }

    public bool HasFreeSlot(out InventorySlot freeSlot, out int index)
    {
        index = _inventorySlots.FindIndex(i => i.ItemData == null);
        if (index >= 0)
        {
            freeSlot = _inventorySlots[index];
            return true;
        }

        freeSlot = null;
        return false;
    }

    public bool HasFreeSlot(out InventorySlot freeSlot)
    {
        freeSlot = InventorySlots.FirstOrDefault(i => i.ItemData == null);
        return freeSlot == null ? false : true;
    }

    internal bool CheckInventoryRemaining(Dictionary<SlotData, int> shoppingCart)
    {
        var clonedSystem = new InventorySystem(this.InventorySize);

        for (int i = 0; i < InventorySize; i++)
        {
            clonedSystem.InventorySlots[i].AssignItem(this.InventorySlots[i].ItemData, this.InventorySlots[i].StackSize);
        }

        foreach (var kvp in shoppingCart)
        {
            for (int i = 0; i < kvp.Value; i++)
            {
                if (!clonedSystem.AddToInventory(kvp.Key, 1))
                    return false;
            }
        }

        return true;
    }

    public void SpendGold(int price)
    {
        Gold -= price;
        //OnChangeGold?.Invoke(Gold);
    }

    public Dictionary<SlotData, int> GetAllItemHeld()
    {
        var distinctItems = new Dictionary<SlotData, int>();

        foreach (var item in _inventorySlots)
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

    public Dictionary<InventorySlot, int> GetAllItemHeld1()
    {
        Dictionary<InventorySlot, int> distinctItems = new Dictionary<InventorySlot, int>();

        foreach (var item in _inventorySlots)
        {
            if (item.ItemData == null)
                continue;

            if (!distinctItems.ContainsKey(item))
                distinctItems.Add(item, item.StackSize);

            else
                distinctItems[item] += item.StackSize;
        }

        return distinctItems;
    }

    public void GainGold(int price)
    {
        Gold += price;
        //OnChangeGold?.Invoke(Gold);
    }

    public void RemoveItemsFromInventory(SlotData data, int amount)
    {
        if (ContainsItem(data, out List<InventorySlot> invSlot))
        {
            foreach (var slot in invSlot)
            {
                int stackSize = slot.StackSize;

                if (stackSize > amount)
                {
                    slot.RemoveFromStack(amount);
                    OnInventorySlotChanged?.Invoke(slot);
                    return;
                }

                else
                {
                    slot.RemoveFromStack(stackSize);
                    amount -= stackSize;
                    OnInventorySlotChanged?.Invoke(slot);
                }
            }
        }
    }

    public void RemoveItemsFromInventory(InventorySlot slot, int amount)
    {
        slot.RemoveFromStack(amount);
        OnInventorySlotChanged?.Invoke(slot);
    }

    //public void RemoveItemsFromInventory1(InventorySlot invSlot, int amount)
    //{
    //    if (ContainsItem(invSlot.BaseItemData, out List<InventorySlot> invSlots))
    //    {
    //        foreach (var slot in invSlots)
    //        {
    //            int stackSize = slot.StackSize;

    //            if (stackSize > amount)
    //            {
    //                slot.RemoveFromStack(amount);
    //                OnInventorySlotChanged?.Invoke(slot);
    //                return;
    //            }

    //            else
    //            {
    //                slot.RemoveFromStack(stackSize);
    //                amount -= stackSize;
    //                OnInventorySlotChanged?.Invoke(slot);
    //            }
    //        }
    //    }
    //}
}
