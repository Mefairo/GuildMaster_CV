using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public abstract class InventoryDisplay : MonoBehaviour
{
    [SerializeField] protected MouseItemData _mouseInventoryItem;
    [SerializeField] protected Hero _hero;
    [Header("Other Displays")]
    [SerializeField] protected ReforgeController _reforgeController;
    [SerializeField] protected DynamicEquipDisplay _guildEquipDisplay;
    [SerializeField] protected DynamicEquipDisplay _questEquipDisplay;

    protected InventorySystem _inventorySystem;
    protected Dictionary<InventorySlot_UI, InventorySlot> _slotDictionary;

    public InventorySystem InventorySystem => _inventorySystem;

    public Dictionary<InventorySlot_UI, InventorySlot> SlotDictionary => _slotDictionary;


    public abstract void AssignSlot(InventorySystem invToDisplay, int offset);

    protected virtual void UpdateSlot(InventorySlot updatedSlot)
    {
        foreach (var slot in SlotDictionary)
        {
            if (slot.Value == updatedSlot) // Значение слота - "под худом" слота инвентаря
                slot.Key.UpdateUISlot(updatedSlot); // Ключ слота - UI представление значения
        }
    }

    public void SlotRightClicked(InventorySlot_UI clickedUISlot)
    {
        if (_reforgeController.ReforgePanel.gameObject.activeInHierarchy)
        {
            if (clickedUISlot.AssignedInventorySlot is BlankSlot blankSlot)
            {
                Debug.Log("BlankRightClicked");
                _reforgeController.ClickBlankSlot(blankSlot);
            }

            else if (clickedUISlot.AssignedInventorySlot is RuneSlot runeSlot)
            {
                Debug.Log("RuneRightClicked");
                _reforgeController.ClickRuneSlot(runeSlot);
            }

            else if (clickedUISlot.AssignedInventorySlot is CatalystSlot catalystSlot)
            {
                Debug.Log("CatalystRightClicked");
                _reforgeController.ClickCatalystSlot(catalystSlot);
            }
        }

        if (_guildEquipDisplay.gameObject.activeInHierarchy)
        {
            if (clickedUISlot.AssignedInventorySlot is BlankSlot blankSlot)
            {
                Debug.Log("BlankRightClicked 1");
                _guildEquipDisplay.ClickBlankSlot(blankSlot);
            }

            else if (clickedUISlot.AssignedInventorySlot is EquipSlot equipSlot)
            {
                Debug.Log("EquipRightClicked 1");
                _guildEquipDisplay.ClickEquipSlot(equipSlot, clickedUISlot);
            }
        }

        if (_questEquipDisplay.gameObject.activeInHierarchy)
        {
            if (clickedUISlot.AssignedInventorySlot is BlankSlot blankSlot)
            {
                Debug.Log("BlankRightClicked 2");
                _questEquipDisplay.ClickBlankSlot(blankSlot);
            }

            else if (clickedUISlot.AssignedInventorySlot is EquipSlot equipSlot)
            {
                Debug.Log("EquipRightClicked 2");
                _questEquipDisplay.ClickEquipSlot(equipSlot, clickedUISlot);
            }
        }

        clickedUISlot.UpdateUISlot();

        if(_hero != null)
            _hero.EquipHolder.EquipSystem.ResetEquipIndex();
    }

    public void SlotClicked(InventorySlot_UI clickedUISlot)
    {
        // Если кликнуть на слот,в котором есть предмет,а у мыши нет элемента, тогда нужно поднять предмет
        if (clickedUISlot.AssignedInventorySlot.BaseItemData != null && _mouseInventoryItem.AssignedInventorySlot.BaseItemData == null)
        {

            // Если игрок держит SHIFT, то нужно разделить количество предметов в слоте
            if (Input.GetKey(KeyCode.LeftShift) && clickedUISlot.AssignedInventorySlot.SplitStack(out InventorySlot halfStackSlot))
            {
                int mouseHalfStack = halfStackSlot.StackSize;
                halfStackSlot = SetTypeInventorySlot(clickedUISlot.AssignedInventorySlot);
                _mouseInventoryItem.AssignedInventorySlot = _inventorySystem.ChangeTypeSlot1(halfStackSlot, clickedUISlot.AssignedInventorySlot.Index);

                if (_mouseInventoryItem.AssignedInventorySlot.StackSize > mouseHalfStack) // ЕСЛИ НЕЧЕТНОЕ
                    _mouseInventoryItem.AssignedInventorySlot.RemoveFromStack(1);

                _mouseInventoryItem.UpdateMouseSlot();
                clickedUISlot.UpdateUISlot();
                return;
            }
            else
            {
                _mouseInventoryItem.AssignedInventorySlot = _inventorySystem.ChangeTypeSlot1(clickedUISlot.AssignedInventorySlot,
                    clickedUISlot.AssignedInventorySlot.Index);
                //_mouseInventoryItem.UpdateMouseSlot(clickedUISlot.AssignedInventorySlot);
                _mouseInventoryItem.UpdateMouseSlot();
                clickedUISlot.AssignedInventorySlot.ClearSlot();
                clickedUISlot.UpdateUISlot();
                return;
            }

        }

        // Если слот не содержит предмет, а мышь содержит предмет, тогда нужно положить предмет на курсоре мышки в этот пустой слот
        if (clickedUISlot.AssignedInventorySlot.BaseItemData == null && _mouseInventoryItem.AssignedInventorySlot.BaseItemData != null)
        {
            var newSlot = _inventorySystem.ChangeTypeSlot1(_mouseInventoryItem.AssignedInventorySlot, clickedUISlot.AssignedInventorySlot.Index);

            clickedUISlot.Init(newSlot);
            _inventorySystem.InventorySlots[clickedUISlot.AssignedInventorySlot.Index] = newSlot;
            //clickedUISlot.AssignedInventorySlot.AssignItem(_mouseInventoryItem.AssignedInventorySlot);
            clickedUISlot.UpdateUISlot();

            _mouseInventoryItem.ClearSlot();
            return;
        }

        // Если оба слота содержат предметы, то нужно решить...
        if (clickedUISlot.AssignedInventorySlot.ItemData != null && _mouseInventoryItem.AssignedInventorySlot.ItemData != null)
        {
            bool isSameItem = clickedUISlot.AssignedInventorySlot.ItemData == _mouseInventoryItem.AssignedInventorySlot.ItemData;

            // Если предмет в инвентаре и на курсоре мыши одинаковый, то объединяем их
            if (isSameItem && clickedUISlot.AssignedInventorySlot.EnoughRoomLeftInStack(_mouseInventoryItem.AssignedInventorySlot.StackSize))
            {
                //clickedUISlot.AssignedInventorySlot.AssignItem(_mouseInventoryItem.AssignedInventorySlot);
                //for (int i = 0; i < _mouseInventoryItem.AssignedInventorySlot.StackSize; i++)
                //    clickedUISlot.AssignedInventorySlot.AddToStack(_mouseInventoryItem.AssignedInventorySlot.StackSize);
                clickedUISlot.AssignedInventorySlot.AddToStack(_mouseInventoryItem.AssignedInventorySlot.StackSize);

                clickedUISlot.UpdateUISlot();

                _mouseInventoryItem.ClearSlot();
                return;
            }

            else if (isSameItem && !clickedUISlot.AssignedInventorySlot.RoomLeftInStack(_mouseInventoryItem.AssignedInventorySlot.StackSize, out int leftInStack))
            {
                if (leftInStack < 1)
                {
                    SwapSlots(clickedUISlot);
                }

                else
                {
                    int remainingOnMouse = _mouseInventoryItem.AssignedInventorySlot.StackSize - leftInStack;
                    clickedUISlot.AssignedInventorySlot.AddToStack(leftInStack);
                    clickedUISlot.UpdateUISlot();

                    //var newItem = new InventorySlot(_mouseInventoryItem.AssignedInventorySlot.ItemData, remainingOnMouse);
                    //var newItem = _mouseInventoryItem.AssignedInventorySlot.CloneSlot();

                    _mouseInventoryItem.AssignedInventorySlot.ChangeStacksize(remainingOnMouse);
                    _mouseInventoryItem.UpdateMouseSlot();
                    //_mouseInventoryItem.ClearSlot();

                    return;
                }
            }

            // Если предметы разные, то меняем их местами
            else if (!isSameItem)
            {
                SwapSlots(clickedUISlot);
                return;
            }
        }
    }

    private void SwapSlots(InventorySlot_UI clickedUISlot)
    {
        // Получаем предметы в слотах
        InventorySlot itemInClickedSlot = clickedUISlot.AssignedInventorySlot;
        InventorySlot itemOnMouse = _mouseInventoryItem.AssignedInventorySlot;

        // Если предметы одинаковые (тот же тип)
        if (itemInClickedSlot.ItemData == itemOnMouse.ItemData)
        {
            int spaceLeftInClickedSlot = itemInClickedSlot.ItemData.MaxStackSize - itemInClickedSlot.StackSize;
            int remainingOnMouse = itemOnMouse.StackSize - spaceLeftInClickedSlot;

            // Если можно объединить стаки полностью
            if (spaceLeftInClickedSlot >= itemOnMouse.StackSize)
            {
                itemInClickedSlot.AddToStack(itemOnMouse.StackSize);
                _mouseInventoryItem.ClearSlot();
            }


            else if (itemInClickedSlot.StackSize == itemInClickedSlot.ItemData.MaxStackSize)
            {
                //int slotSize = itemInClickedSlot.StackSize + 1;
                ////var clonedClickedSlot = new InventorySlot(itemInClickedSlot.ItemData, itemInClickedSlot.StackSize);
                //var clonedClickedSlot = itemInClickedSlot.CloneSlot();

                //itemInClickedSlot.SwapStack(itemOnMouse.StackSize);
                //clickedUISlot.UpdateUISlot();

                //_mouseInventoryItem.ClearSlot();
                //itemOnMouse.AddToStack(slotSize);
                //_mouseInventoryItem.UpdateMouseSlot(clonedClickedSlot);




                var cloneClickSlot = SetTypeInventorySlot(clickedUISlot.AssignedInventorySlot);
                var cloneMouseSlot = SetTypeInventorySlot(_mouseInventoryItem.AssignedInventorySlot);

                clickedUISlot.AssignedInventorySlot.ClearSlot();
                _mouseInventoryItem.AssignedInventorySlot.ClearSlot();

                var newClickSlot = _inventorySystem.ChangeTypeSlot1(cloneMouseSlot, cloneClickSlot.Index);
                _mouseInventoryItem.AssignedInventorySlot = _inventorySystem.ChangeTypeSlot1(cloneClickSlot, cloneClickSlot.Index);

                clickedUISlot.Init(newClickSlot);
                _inventorySystem.InventorySlots[clickedUISlot.AssignedInventorySlot.Index] = newClickSlot;
                clickedUISlot.UpdateUISlot();

                _mouseInventoryItem.UpdateMouseSlot();
            }

            return;
        }

        // Если предметы разные, меняем их местами
        else if (itemInClickedSlot.ItemData != itemOnMouse.ItemData)
        {
            //InventorySlot clonedClickedSlot = new InventorySlot(itemInClickedSlot.ItemData, itemInClickedSlot.StackSize);
            //var clonedClickedSlot = itemInClickedSlot.CloneSlot();
            //clickedUISlot.ClearSlot();
            //clickedUISlot.AssignedInventorySlot.AssignItem(itemOnMouse);
            //clickedUISlot.UpdateUISlot();

            //_mouseInventoryItem.UpdateMouseSlot(clonedClickedSlot);


            var cloneClickSlot = SetTypeInventorySlot(clickedUISlot.AssignedInventorySlot);
            var cloneMouseSlot = SetTypeInventorySlot(_mouseInventoryItem.AssignedInventorySlot);

            clickedUISlot.AssignedInventorySlot.ClearSlot();
            _mouseInventoryItem.AssignedInventorySlot.ClearSlot();

            var newClickSlot = _inventorySystem.ChangeTypeSlot1(cloneMouseSlot, cloneClickSlot.Index);
            _mouseInventoryItem.AssignedInventorySlot = _inventorySystem.ChangeTypeSlot1(cloneClickSlot, cloneClickSlot.Index);

            clickedUISlot.Init(newClickSlot);
            _inventorySystem.InventorySlots[clickedUISlot.AssignedInventorySlot.Index] = newClickSlot;
            clickedUISlot.UpdateUISlot();

            _mouseInventoryItem.UpdateMouseSlot();

            return;
        }

        else if (itemInClickedSlot != itemOnMouse)
        {
            var newSlot = _inventorySystem.ChangeTypeSlot1(_mouseInventoryItem.AssignedInventorySlot, clickedUISlot.AssignedInventorySlot.Index);

            clickedUISlot.Init(newSlot);
            _inventorySystem.InventorySlots[clickedUISlot.AssignedInventorySlot.Index] = newSlot;
            clickedUISlot.UpdateUISlot();

            _mouseInventoryItem.ClearSlot();

            return;
        }
    }

    private InventorySlot SetTypeInventorySlot(InventorySlot invSlot)
    {
        InventorySlot newSlot;

        if (invSlot is BlankSlot blank)
            newSlot = new BlankSlot(blank, blank.Index);

        else if (invSlot is RuneSlot rune)
            newSlot = new RuneSlot(rune, rune.Index);

        else if (invSlot is CatalystSlot catalyst)
            newSlot = new CatalystSlot(catalyst, catalyst.Index);

        else if (invSlot is EquipSlot equip)
            newSlot = new EquipSlot(equip, equip.Index);

        else
            newSlot = new InventorySlot(invSlot.BaseItemData, invSlot.StackSize, invSlot.Index);

        //newSlot.UpdateInventorySlot(invSlot.BaseItemData, invSlot.StackSize);
        return newSlot;
    }
}

