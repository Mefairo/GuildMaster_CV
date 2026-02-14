using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class DynamicEquipDisplay : EquipDisplay
{
    [SerializeField] private List<EquipSlot_UI> _slotsList;
    [SerializeField] private List<EquipSlot_UI> _trinketList;

    public List<EquipSlot_UI> SlotsList => _slotsList;
    public List<EquipSlot_UI> TrinketList => _trinketList;

    public void RefreshDynamicInventory(EquipSystem equipToDisplay, int offset)
    {
        ClearSlots();

        _equipSystem = equipToDisplay;

        if (_equipSystem != null)
        {
            _equipSystem.OneEquipSlotChanged += UpdateSlot;
        }
        AssignSlot(equipToDisplay, offset);

        for (int i = 0; i < _slotsList.Count; i++)
        {
            _slotsList[i].CheckEquipBackground();
            _slotsList[i].AssignedEquipSlot.SetEquipIndex(i);
        }
    }

    public override void AssignSlot(EquipSystem equipToDisplay, int offset)
    {
        ClearSlots();

        _equipDictionary = new Dictionary<EquipSlot_UI, EquipSlot>();

        if (equipToDisplay == null)
            return;

        for (int i = 0; i < equipToDisplay.Slots.Count; i++)
        {
            _slotsList[i].Init(equipToDisplay.Slots[i]);
            _slotsList[i].UpdateUISlot(equipToDisplay.Slots[i]);
        }

    }

    private void ClearSlots()
    {
        foreach (var slot in _slotsList)
        {
            slot.ClearSlot();
        }

        if (_equipDictionary != null)
        {
            _equipDictionary.Clear();
        }
    }

    public void ClickBlankSlot(BlankSlot blankSlot)
    {
        if (KeeperDisplay != null && KeeperDisplay.SelectedHero != null && KeeperDisplay.SelectedHero.Hero != null)
            _hero = KeeperDisplay.SelectedHero.Hero;

        if (PrepareQuestUIController != null && PrepareQuestUIController.SelectedHero != null && PrepareQuestUIController.SelectedHero.Hero != null)
            _hero = PrepareQuestUIController.SelectedHero.Hero;

        if (blankSlot.UsedRuneSlots.Count == 0)
            return;

        if (blankSlot.BlankSlotData.EquipType == EquipType.Trinket)
        {

        }

        else
        {
            int equipSlotIndex = SetIndexEquipSlot(blankSlot.BlankSlotData.EquipType);

            if (blankSlot.BlankSlotData.EquipType == EquipType.OffHandWeapon && _slotsList[equipSlotIndex - 1].AssignedEquipSlot.BaseItemData != null &&
               _slotsList[equipSlotIndex - 1].AssignedEquipSlot.EquipItemData.EquipType == EquipType.TwoHandWeapon)
                return;

            if (blankSlot.BlankSlotData.EquipType == EquipType.TwoHandWeapon && _slotsList[equipSlotIndex + 1].AssignedEquipSlot.BaseItemData != null)
                return;

            if (_slotsList[equipSlotIndex].AssignedEquipSlot.BaseItemData == null)
            {
                var newSlot = SetTypeEquipSlot(_slotsList[equipSlotIndex].AssignedEquipSlot, blankSlot);
                _slotsList[equipSlotIndex].Init(newSlot);
                _equipSystem.Slots[equipSlotIndex] = newSlot;
                _slotsList[equipSlotIndex].UpdateUISlot();

                if (KeeperDisplay == null && PrepareQuestUIController == null)
                {
                    return;
                }

                else if (KeeperDisplay != null)
                {
                    //OnHeroEquip?.Invoke(KeeperDisplay.SelectedHero.Hero, equipSlot_UI);
                    OnHeroEquip?.Invoke(_hero, _slotsList[equipSlotIndex]);

                    if (blankSlot.StackSize == 1)
                    {
                        _playerInventoryHolder.PrimaryInventorySystem.RemoveItemsFromInventory(_playerInventoryHolder.PrimaryInventorySystem.
InventorySlots[blankSlot.Index], blankSlot.StackSize);
                        KeeperDisplay.UpdateUIInfo(_hero);
                        return;
                    }

                    else
                    {
                        blankSlot.RemoveFromStack(1);
                        KeeperDisplay.UpdateUIInfo(_hero);
                        return;
                    }
                }

                else if (PrepareQuestUIController != null)
                {
                    OnHeroEquip?.Invoke(_hero, _slotsList[equipSlotIndex]);
                    if (blankSlot.StackSize == 1)
                    {
                        _playerInventoryHolder.PrimaryInventorySystem.RemoveItemsFromInventory(_playerInventoryHolder.PrimaryInventorySystem.
InventorySlots[blankSlot.Index], blankSlot.StackSize);
                        PrepareQuestUIController.UpdateUIInfo(_hero);
                        return;
                    }

                    else
                    {
                        blankSlot.RemoveFromStack(1);
                        PrepareQuestUIController.UpdateUIInfo(_hero);
                        return;
                    }
                }
            }

            else
            {
                EquipSlot itemInHeroEquipInventory = _slotsList[equipSlotIndex].AssignedEquipSlot;

                // Если предметы одинаковые (тот же тип)
                if (itemInHeroEquipInventory.BaseItemData == blankSlot.BaseItemData)
                {
                    var localBaseData = (EquipItemData)itemInHeroEquipInventory.BaseItemData;
                    int spaceLeftInClickedSlot = localBaseData.EquipMaxStackSize - itemInHeroEquipInventory.StackSize;
                    int remainingOnMouse = blankSlot.StackSize - spaceLeftInClickedSlot;

                    // Если можно объединить стаки полностью
                    if (itemInHeroEquipInventory.StackSize != localBaseData.EquipMaxStackSize)
                    {
                        if (blankSlot.StackSize - 1 == 0)
                        {
                            itemInHeroEquipInventory.AddToStack(1);
                            blankSlot.RemoveFromStack(1);

                            if (localBaseData.ItemSecondaryType == ItemSecondaryType.Potion)
                                OnHeroEquip?.Invoke(_hero, _slotsList[equipSlotIndex]);

                            _slotsList[equipSlotIndex].UpdateEquipSlotUI();
                        }

                        else
                        {
                            itemInHeroEquipInventory.AddToStack(1);
                            blankSlot.RemoveFromStack(1);

                            if (localBaseData.ItemSecondaryType == ItemSecondaryType.Potion)
                                OnHeroEquip?.Invoke(_hero, _slotsList[equipSlotIndex]);

                            _slotsList[equipSlotIndex].UpdateEquipSlotUI();
                        }
                    }

                    else
                    {
                        Debug.Log("swap blank");
                        if (KeeperDisplay != null)
                        {
                            OnHeroTakeOfEquip1?.Invoke(_hero, _slotsList[equipSlotIndex].AssignedEquipSlot);
                            KeeperDisplay.UpdateUIInfo(_hero);
                        }

                        else if (PrepareQuestUIController != null)
                            OnHeroTakeOfEquip1?.Invoke(_hero, _slotsList[equipSlotIndex].AssignedEquipSlot);

                        BlankSlot clonedSlot = new BlankSlot((BlankSlot)_slotsList[equipSlotIndex].AssignedEquipSlot,
                            _slotsList[equipSlotIndex].AssignedEquipSlot.Index);
                        _slotsList[equipSlotIndex].AssignedEquipSlot.ClearSlot();
                        _slotsList[equipSlotIndex].AssignedEquipSlot.AssignItem(blankSlot);
                        _slotsList[equipSlotIndex].UpdateUISlot(blankSlot);
                        _playerInventoryHolder.PrimaryInventorySystem.RemoveItemsFromInventory(_playerInventoryHolder.PrimaryInventorySystem.
                            InventorySlots[blankSlot.Index], blankSlot.StackSize);

                        blankSlot.ClearSlot();
                        blankSlot.AssignItem(clonedSlot);


                        //_slotsList[equipSlotIndex].UpdateUISlot();

                        if (KeeperDisplay != null)
                        {
                            if (localBaseData.ItemSecondaryType == ItemSecondaryType.Potion)
                                OnHeroPotionsEquip?.Invoke(_hero, _slotsList[equipSlotIndex]);

                            else
                                OnHeroEquip?.Invoke(_hero, _slotsList[equipSlotIndex]);

                            KeeperDisplay.UpdateUIInfo(_hero);
                        }

                        else if (PrepareQuestUIController != null)
                        {
                            if (localBaseData.ItemSecondaryType == ItemSecondaryType.Potion)
                                OnHeroPotionsEquip?.Invoke(_hero, _slotsList[equipSlotIndex]);

                            else
                                OnHeroEquip?.Invoke(_hero, _slotsList[equipSlotIndex]);

                            PrepareQuestUIController.UpdateUIInfo(_hero);
                        }

                        //_mouseInventoryItem.UpdateMouseSlot(cloneEquipSlot);

                        return;
                    }
                }

                // Если предметы разные, меняем их местами
                else if (itemInHeroEquipInventory.BaseItemData != blankSlot.BaseItemData)
                {
                    Debug.Log("swap dif 1");
                    var localBaseData = (EquipItemData)itemInHeroEquipInventory.BaseItemData;

                    if (KeeperDisplay != null)
                    {
                        OnHeroTakeOfEquip1?.Invoke(_hero, _slotsList[equipSlotIndex].AssignedEquipSlot);
                        KeeperDisplay.UpdateUIInfo(_hero);
                    }

                    else if (PrepareQuestUIController != null)
                        OnHeroTakeOfEquip1?.Invoke(_hero, _slotsList[equipSlotIndex].AssignedEquipSlot);

                    var cloneHeroEquipSlot = SetTypeEquipSlot(_slotsList[equipSlotIndex].AssignedEquipSlot, _slotsList[equipSlotIndex].AssignedEquipSlot);
                    var cloneInventoryEquipSlot = SetTypeEquipSlot(blankSlot, blankSlot);

                    _slotsList[equipSlotIndex].AssignedEquipSlot.AssignItem(cloneInventoryEquipSlot);
                    _slotsList[equipSlotIndex].Init(cloneInventoryEquipSlot);
                    _equipSystem.Slots[_slotsList[equipSlotIndex].EquipIndex] = cloneInventoryEquipSlot;

                    _playerInventoryHolder.PrimaryInventorySystem.InventorySlots[blankSlot.Index].AssignItem(cloneHeroEquipSlot);
                    _playerInventoryHolder.PrimaryInventorySystem.InventorySlots[blankSlot.Index] = cloneHeroEquipSlot;

                    _playerInventoryHolder.PrimaryInventorySystem.InventorySlots[blankSlot.Index].SetIndex(cloneInventoryEquipSlot.Index);

                    _slotsList[equipSlotIndex].UpdateUISlot();

                    if (KeeperDisplay != null)
                    {
                        if (localBaseData.ItemSecondaryType == ItemSecondaryType.Potion)
                            OnHeroPotionsEquip?.Invoke(_hero, _slotsList[equipSlotIndex]);

                        else
                            OnHeroEquip?.Invoke(_hero, _slotsList[equipSlotIndex]);

                        KeeperDisplay.UpdateUIInfo(_hero);
                    }

                    else if (PrepareQuestUIController != null)
                    {
                        if (localBaseData.ItemSecondaryType == ItemSecondaryType.Potion)
                            OnHeroPotionsEquip?.Invoke(_hero, _slotsList[equipSlotIndex]);

                        else
                            OnHeroEquip?.Invoke(_hero, _slotsList[equipSlotIndex]);

                        PrepareQuestUIController.UpdateUIInfo(_hero);
                    }
                }
            }
        }

        _backPackInventory.RefreshDynamicInventory(_playerInventoryHolder.PrimaryInventorySystem, 0);
    }

    private void AddTrinketAmountToStack(EquipSlot_UI trinketSlotUI, EquipSlot equipSlot)
    {
        int spaceLeftInClickedSlot = trinketSlotUI.AssignedEquipSlot.EquipItemData.EquipMaxStackSize - trinketSlotUI.AssignedEquipSlot.StackSize;
        OnHeroTakeOfEquip?.Invoke(_hero, trinketSlotUI);

        if (spaceLeftInClickedSlot >= equipSlot.StackSize)
        {
            trinketSlotUI.AssignedEquipSlot.AddToStack(equipSlot.StackSize);
            trinketSlotUI.UpdateUISlot(trinketSlotUI.AssignedEquipSlot);

            _playerInventoryHolder.PrimaryInventorySystem.RemoveItemsFromInventory(_playerInventoryHolder.PrimaryInventorySystem.
InventorySlots[equipSlot.Index], equipSlot.StackSize);
        }

        else
        {
            trinketSlotUI.AssignedEquipSlot.AddToStack(spaceLeftInClickedSlot);
            trinketSlotUI.UpdateUISlot(trinketSlotUI.AssignedEquipSlot);

            equipSlot.RemoveFromStack(spaceLeftInClickedSlot);
        }

        if (trinketSlotUI.AssignedEquipSlot.EquipItemData.ItemSecondaryType == ItemSecondaryType.Potion)
            OnHeroPotionsEquip?.Invoke(_hero, trinketSlotUI);
    }

    public void ClickEquipSlot(EquipSlot equipSlot, InventorySlot_UI clickedSlot_UI)
    {
        if (KeeperDisplay != null && KeeperDisplay.SelectedHero != null && KeeperDisplay.SelectedHero.Hero != null)
        {
            _hero = KeeperDisplay.SelectedHero.Hero;
        }

        if (PrepareQuestUIController != null && PrepareQuestUIController.SelectedHero != null && PrepareQuestUIController.SelectedHero.Hero != null)
        {
            _hero = PrepareQuestUIController.SelectedHero.Hero;
        }

        if (equipSlot.EquipItemData.EquipType == EquipType.Light)
            EquipLightItem(equipSlot);

        else if (equipSlot.EquipItemData.EquipType == EquipType.Trinket)
        {
            bool canAddToAmountItem = false;
            for (int i = 0; i < _trinketList.Count; i++)
            {
                if (_trinketList[i].AssignedEquipSlot == null && _trinketList[i].AssignedEquipSlot.BaseItemData == null)
                    continue;

                else
                {
                    if (_trinketList[i].AssignedEquipSlot.BaseItemData != equipSlot.BaseItemData)
                        continue;

                    else
                    {
                        canAddToAmountItem = true;
                        AddTrinketAmountToStack(_trinketList[i], equipSlot);
                        break;
                    }
                }
            }

            if (!canAddToAmountItem)
            {
                EquipSlot_UI freeSlot = _trinketList.FirstOrDefault(i => i.AssignedEquipSlot.BaseItemData == null);

                if (freeSlot == null)
                    return;
                
                var newSlot = SetTypeEquipSlot(freeSlot.AssignedEquipSlot, equipSlot);
                freeSlot.Init(newSlot);
                _equipSystem.Slots[freeSlot.EquipIndex] = newSlot;

                if (equipSlot.StackSize > equipSlot.EquipItemData.EquipMaxStackSize)
                {
                    int difAmountStacksize = equipSlot.StackSize - equipSlot.EquipItemData.EquipMaxStackSize;
                    freeSlot.AssignedEquipSlot.RemoveFromStack(difAmountStacksize);

                    _playerInventoryHolder.PrimaryInventorySystem.RemoveItemsFromInventory(_playerInventoryHolder.PrimaryInventorySystem.
InventorySlots[equipSlot.Index], equipSlot.EquipItemData.EquipMaxStackSize);
                }

                else
                    _playerInventoryHolder.PrimaryInventorySystem.RemoveItemsFromInventory(_playerInventoryHolder.PrimaryInventorySystem.
InventorySlots[equipSlot.Index], equipSlot.StackSize);

                freeSlot.UpdateUISlot();

                if (KeeperDisplay == null && PrepareQuestUIController == null)
                {
                    return;
                }

                else
                {
                    OnHeroPotionsEquip?.Invoke(_hero, freeSlot);

                    //                    _playerInventoryHolder.PrimaryInventorySystem.RemoveItemsFromInventory(_playerInventoryHolder.PrimaryInventorySystem.
                    //InventorySlots[equipSlot.Index], equipSlot.StackSize);

                    if (KeeperDisplay != null)
                        KeeperDisplay.UpdateUIInfo(_hero);

                    return;
                }


            }
        }

        else
        {
            int equipSlotIndex = SetIndexEquipSlot(equipSlot.EquipItemData.EquipType);

            if (equipSlot.EquipItemData.EquipType == EquipType.OffHandWeapon && _slotsList[equipSlotIndex - 1].AssignedEquipSlot.EquipItemData != null &&
               _slotsList[equipSlotIndex - 1].AssignedEquipSlot.EquipItemData.EquipType == EquipType.TwoHandWeapon)
                return;

            if (equipSlot.EquipItemData.EquipType == EquipType.TwoHandWeapon && _slotsList[equipSlotIndex + 1].AssignedEquipSlot.EquipItemData != null)
                return;

            if (_slotsList[equipSlotIndex].AssignedEquipSlot.BaseItemData == null)
            {
                var newSlot = SetTypeEquipSlot(_slotsList[equipSlotIndex].AssignedEquipSlot, equipSlot);
                _slotsList[equipSlotIndex].Init(newSlot);
                _equipSystem.Slots[equipSlotIndex] = newSlot;
                _slotsList[equipSlotIndex].UpdateUISlot();

                if (KeeperDisplay == null && PrepareQuestUIController == null)
                    return;

                else if (KeeperDisplay != null)
                {
                    //OnHeroEquip?.Invoke(KeeperDisplay.SelectedHero.Hero, equipSlot_UI);
                    OnHeroEquip?.Invoke(_hero, _slotsList[equipSlotIndex]);

                    if (equipSlot.StackSize == 1)
                    {
                        _playerInventoryHolder.PrimaryInventorySystem.RemoveItemsFromInventory(_playerInventoryHolder.PrimaryInventorySystem.
InventorySlots[equipSlot.Index], equipSlot.StackSize);
                        KeeperDisplay.UpdateUIInfo(_hero);
                        return;
                    }

                    else
                    {
                        equipSlot.RemoveFromStack(equipSlot.StackSize);
                        KeeperDisplay.UpdateUIInfo(_hero);
                        return;
                    }
                }

                else if (PrepareQuestUIController != null)
                {
                    OnHeroEquip?.Invoke(_hero, _slotsList[equipSlotIndex]);
                    if (equipSlot.StackSize == 1)
                    {
                        _playerInventoryHolder.PrimaryInventorySystem.RemoveItemsFromInventory(_playerInventoryHolder.PrimaryInventorySystem.
InventorySlots[equipSlot.Index], equipSlot.StackSize);
                        PrepareQuestUIController.UpdateUIInfo(_hero);
                        return;
                    }

                    else
                    {
                        equipSlot.RemoveFromStack(equipSlot.StackSize);
                        PrepareQuestUIController.UpdateUIInfo(_hero);
                        return;
                    }
                }
            }

            else
            {
                EquipSlot itemInHeroEquipInventory = _slotsList[equipSlotIndex].AssignedEquipSlot;

                // Если предметы одинаковые (тот же тип)
                if (itemInHeroEquipInventory.BaseItemData == equipSlot.BaseItemData)
                {
                    var localBaseData = (EquipItemData)itemInHeroEquipInventory.BaseItemData;
                    int spaceLeftInClickedSlot = localBaseData.EquipMaxStackSize - itemInHeroEquipInventory.StackSize;
                    int remainingOnMouse = equipSlot.StackSize - spaceLeftInClickedSlot;

                    // Если можно объединить стаки полностью
                    if (itemInHeroEquipInventory.StackSize != localBaseData.EquipMaxStackSize)
                    {
                        if (equipSlot.StackSize - 1 == 0)
                        {
                            itemInHeroEquipInventory.AddToStack(1);
                            equipSlot.RemoveFromStack(1);

                            if (localBaseData.ItemSecondaryType == ItemSecondaryType.Potion)
                                OnHeroEquip?.Invoke(_hero, _slotsList[equipSlotIndex]);

                            _slotsList[equipSlotIndex].UpdateEquipSlotUI();
                        }

                        else
                        {
                            itemInHeroEquipInventory.AddToStack(1);
                            equipSlot.RemoveFromStack(1);

                            if (localBaseData.ItemSecondaryType == ItemSecondaryType.Potion)
                                OnHeroEquip?.Invoke(_hero, _slotsList[equipSlotIndex]);

                            _slotsList[equipSlotIndex].UpdateEquipSlotUI();
                        }
                    }

                    else
                    {
                        if (KeeperDisplay != null)
                        {
                            OnHeroTakeOfEquip1?.Invoke(_hero, _slotsList[equipSlotIndex].AssignedEquipSlot);
                            KeeperDisplay.UpdateUIInfo(_hero);
                        }

                        else if (PrepareQuestUIController != null)
                            OnHeroTakeOfEquip1?.Invoke(_hero, _slotsList[equipSlotIndex].AssignedEquipSlot);

                        var cloneEquipSlot = SetTypeEquipSlot(_slotsList[equipSlotIndex].AssignedEquipSlot, _slotsList[equipSlotIndex].AssignedEquipSlot);
                        var cloneRightClickSlot = SetTypeEquipSlot(equipSlot, equipSlot);

                        _slotsList[equipSlotIndex].Init(cloneRightClickSlot);
                        _equipSystem.Slots[_slotsList[equipSlotIndex].EquipIndex] = cloneRightClickSlot;

                        _playerInventoryHolder.PrimaryInventorySystem.InventorySlots[equipSlot.Index].AssignItem(cloneEquipSlot);
                        _playerInventoryHolder.PrimaryInventorySystem.InventorySlots[equipSlot.Index] = cloneEquipSlot;

                        _slotsList[equipSlotIndex].UpdateUISlot();

                        if (KeeperDisplay != null)
                        {
                            if (localBaseData.ItemSecondaryType == ItemSecondaryType.Potion)
                                OnHeroPotionsEquip?.Invoke(_hero, _slotsList[equipSlotIndex]);

                            else
                                OnHeroEquip?.Invoke(_hero, _slotsList[equipSlotIndex]);

                            KeeperDisplay.UpdateUIInfo(_hero);
                        }

                        else if (PrepareQuestUIController != null)
                        {
                            if (localBaseData.ItemSecondaryType == ItemSecondaryType.Potion)
                                OnHeroPotionsEquip?.Invoke(_hero, _slotsList[equipSlotIndex]);

                            else
                                OnHeroEquip?.Invoke(_hero, _slotsList[equipSlotIndex]);

                            PrepareQuestUIController.UpdateUIInfo(_hero);
                        }

                        //_mouseInventoryItem.UpdateMouseSlot(cloneEquipSlot);

                        return;
                    }
                }

                // Если предметы разные, меняем их местами
                else if (itemInHeroEquipInventory.BaseItemData != equipSlot.BaseItemData)
                {
                    var localBaseData = (EquipItemData)itemInHeroEquipInventory.BaseItemData;

                    if (KeeperDisplay != null)
                    {
                        OnHeroTakeOfEquip1?.Invoke(_hero, _slotsList[equipSlotIndex].AssignedEquipSlot);
                        KeeperDisplay.UpdateUIInfo(_hero);
                    }

                    else if (PrepareQuestUIController != null)
                        OnHeroTakeOfEquip1?.Invoke(_hero, _slotsList[equipSlotIndex].AssignedEquipSlot);

                    var cloneHeroEquipSlot = SetTypeEquipSlot(_slotsList[equipSlotIndex].AssignedEquipSlot, _slotsList[equipSlotIndex].AssignedEquipSlot);
                    var cloneInventoryEquipSlot = SetTypeEquipSlot(equipSlot, equipSlot);

                    _slotsList[equipSlotIndex].AssignedEquipSlot.AssignItem(cloneInventoryEquipSlot);
                    _slotsList[equipSlotIndex].Init(cloneInventoryEquipSlot);
                    _equipSystem.Slots[_slotsList[equipSlotIndex].EquipIndex] = cloneInventoryEquipSlot;

                    _playerInventoryHolder.PrimaryInventorySystem.InventorySlots[equipSlot.Index].AssignItem(cloneHeroEquipSlot);
                    _playerInventoryHolder.PrimaryInventorySystem.InventorySlots[equipSlot.Index] = cloneHeroEquipSlot;

                    _playerInventoryHolder.PrimaryInventorySystem.InventorySlots[equipSlot.Index].SetIndex(cloneInventoryEquipSlot.Index);

                    _slotsList[equipSlotIndex].UpdateUISlot();

                    if (KeeperDisplay != null)
                    {
                        if (localBaseData.ItemSecondaryType == ItemSecondaryType.Potion)
                            OnHeroPotionsEquip?.Invoke(_hero, _slotsList[equipSlotIndex]);

                        else
                            OnHeroEquip?.Invoke(_hero, _slotsList[equipSlotIndex]);

                        KeeperDisplay.UpdateUIInfo(_hero);
                    }

                    else if (PrepareQuestUIController != null)
                    {
                        if (localBaseData.ItemSecondaryType == ItemSecondaryType.Potion)
                            OnHeroPotionsEquip?.Invoke(_hero, _slotsList[equipSlotIndex]);

                        else
                            OnHeroEquip?.Invoke(_hero, _slotsList[equipSlotIndex]);

                        PrepareQuestUIController.UpdateUIInfo(_hero);
                    }

                    //_mouseInventoryItem.UpdateMouseSlot();
                    //_mouseInventoryItem.UpdateMouseSlot(cloneEquipSlot);

                }
            }
        }

        _backPackInventory.RefreshDynamicInventory(_playerInventoryHolder.PrimaryInventorySystem, 0);
    }

    public void RightClickSlot(InventorySlot_UI clickSlot)
    {
        if (KeeperDisplay != null && KeeperDisplay.SelectedHero != null && KeeperDisplay.SelectedHero.Hero != null)
            _hero = KeeperDisplay.SelectedHero.Hero;

        if (PrepareQuestUIController != null && PrepareQuestUIController.SelectedHero != null && PrepareQuestUIController.SelectedHero.Hero != null)
            _hero = PrepareQuestUIController.SelectedHero.Hero;

        if (clickSlot is EquipSlot_UI equipSlot_UI)
        {
            var cloneHeroEquipSlot = SetTypeEquipSlot(equipSlot_UI.AssignedEquipSlot, equipSlot_UI.AssignedEquipSlot);

            _playerInventoryHolder.PrimaryInventorySystem.AddToInventory(cloneHeroEquipSlot, cloneHeroEquipSlot.StackSize);

            _slotsList[equipSlot_UI.EquipIndex].ClearSlot();
            _slotsList[equipSlot_UI.EquipIndex].AssignedEquipSlot.ClearSlot();
            OnHeroTakeOfEquip1?.Invoke(_hero, cloneHeroEquipSlot);

            _slotsList[equipSlot_UI.EquipIndex].UpdateUISlot();

            _backPackInventory.RefreshDynamicInventory(_playerInventoryHolder.PrimaryInventorySystem, 0);
        }

        if (KeeperDisplay != null)
            KeeperDisplay.UpdateUIInfo(_hero);

        else if (PrepareQuestUIController != null)
            PrepareQuestUIController.UpdateUIInfo(_hero);
    }

    protected override EquipSlot SetTypeEquipSlot(EquipSlot equipSlot, InventorySlot slotToCompare)
    {
        EquipSlot newSlot;

        if (slotToCompare is BlankSlot blank)
            newSlot = new BlankSlot(blank, blank.Index);

        else if (slotToCompare is EquipSlot equip)
            newSlot = new EquipSlot(equip, equip.Index);

        else
            newSlot = new EquipSlot();

        return newSlot;
    }

    private void EquipLightItem(EquipSlot lightItem)
    {
        if (_slotsList[18].AssignedEquipSlot == null || _slotsList[18].AssignedEquipSlot.EquipItemData == null) // 18 - индекс слота со светом в панели эквипа
        {
            Debug.Log("light slot null");

            var newSlot = SetTypeEquipSlot(lightItem, lightItem);

            _slotsList[18].Init(newSlot);
            _equipSystem.Slots[18] = newSlot;

            if (lightItem.StackSize > lightItem.EquipItemData.EquipMaxStackSize)
            {
                int difAmountStacksize = lightItem.StackSize - lightItem.EquipItemData.EquipMaxStackSize;
                _slotsList[18].AssignedEquipSlot.RemoveFromStack(difAmountStacksize);

                _playerInventoryHolder.PrimaryInventorySystem.RemoveItemsFromInventory(_playerInventoryHolder.PrimaryInventorySystem.
InventorySlots[lightItem.Index], lightItem.EquipItemData.EquipMaxStackSize);
            }

            else
                _playerInventoryHolder.PrimaryInventorySystem.RemoveItemsFromInventory(_playerInventoryHolder.PrimaryInventorySystem.
InventorySlots[lightItem.Index], lightItem.StackSize);

            _slotsList[18].UpdateUISlot();
            OnHeroPotionsEquip?.Invoke(_hero, _slotsList[18]);

            return;
        }

        else
        {
            Debug.Log("light slot ne null");

            if (_slotsList[18].AssignedEquipSlot.EquipItemData == lightItem.EquipItemData)
            {
                Debug.Log("light slot same");

                int spaceLeftInEquipSlot = _slotsList[18].AssignedEquipSlot.EquipItemData.EquipMaxStackSize - _slotsList[18].AssignedEquipSlot.StackSize;

                OnHeroTakeOfEquip?.Invoke(_hero, _slotsList[18]);

                if (spaceLeftInEquipSlot >= lightItem.StackSize)
                {
                    Debug.Log("add 1");
                    _slotsList[18].AssignedEquipSlot.AddToStack(lightItem.StackSize);
                    _slotsList[18].UpdateUISlot(_slotsList[18].AssignedEquipSlot);

                    _playerInventoryHolder.PrimaryInventorySystem.RemoveItemsFromInventory(_playerInventoryHolder.PrimaryInventorySystem.
        InventorySlots[lightItem.Index], lightItem.StackSize);
                }

                else
                {
                    Debug.Log("add 2");
                    _slotsList[18].AssignedEquipSlot.AddToStack(spaceLeftInEquipSlot);
                    _slotsList[18].UpdateUISlot(_slotsList[18].AssignedEquipSlot);

                    lightItem.RemoveFromStack(spaceLeftInEquipSlot);
                }

                OnHeroPotionsEquip?.Invoke(_hero, _slotsList[18]);
            }

            else
            {
                Debug.Log("light slot ne same");

                if (KeeperDisplay != null)
                {
                    OnHeroTakeOfEquip1?.Invoke(_hero, _slotsList[18].AssignedEquipSlot);
                    KeeperDisplay.UpdateUIInfo(_hero);
                }

                else if (PrepareQuestUIController != null)
                    OnHeroTakeOfEquip1?.Invoke(_hero, _slotsList[18].AssignedEquipSlot);

                var cloneHeroEquipSlot = SetTypeEquipSlot(_slotsList[18].AssignedEquipSlot, _slotsList[18].AssignedEquipSlot);
                var cloneInventoryEquipSlot = SetTypeEquipSlot(lightItem, lightItem);

                _slotsList[18].AssignedEquipSlot.AssignItem(cloneInventoryEquipSlot);
                _slotsList[18].Init(cloneInventoryEquipSlot);
                _equipSystem.Slots[_slotsList[18].EquipIndex] = cloneInventoryEquipSlot;

                _playerInventoryHolder.PrimaryInventorySystem.InventorySlots[lightItem.Index].AssignItem(cloneHeroEquipSlot);
                _playerInventoryHolder.PrimaryInventorySystem.InventorySlots[lightItem.Index] = cloneHeroEquipSlot;

                _playerInventoryHolder.PrimaryInventorySystem.InventorySlots[lightItem.Index].SetIndex(cloneInventoryEquipSlot.Index);

                _slotsList[18].UpdateUISlot();
            }
        }
    }

    private int SetIndexEquipSlot(EquipType equipType)
    {
        int equipSlotIndex = -1;

        switch (equipType)
        {
            case EquipType.None:
                break;

            case EquipType.Helmet:
                equipSlotIndex = 0;
                break;

            case EquipType.Neck:
                equipSlotIndex = 1;
                break;

            case EquipType.Chest:
                equipSlotIndex = 2;
                break;

            case EquipType.Gloves:
                equipSlotIndex = 3;
                break;

            case EquipType.Ring:
                equipSlotIndex = 4;
                break;

            case EquipType.Pants:
                equipSlotIndex = 5;
                break;

            case EquipType.Boots:
                equipSlotIndex = 6;
                break;

            case EquipType.OneHandWeapon:
                equipSlotIndex = 7;
                break;

            case EquipType.TwoHandWeapon:
                equipSlotIndex = 7;
                break;

            case EquipType.OffHandWeapon:
                equipSlotIndex = 8;
                break;
        }

        return equipSlotIndex;
    }
}
