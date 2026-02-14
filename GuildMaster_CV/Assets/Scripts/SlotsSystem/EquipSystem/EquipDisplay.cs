using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.LowLevel;

public class EquipDisplay : MonoBehaviour
{
    [SerializeField] protected StaticInventoryDisplay _invDisplay;
    [SerializeField] protected MouseItemData _mouseInventoryItem;
    [SerializeField] protected EquipSlot_UI _weaponCheckSlot_1;
    [SerializeField] protected EquipSlot_UI _weaponCheckSlot_2;
    [SerializeField] protected PlayerInventoryHolder _playerInventoryHolder;
    [SerializeField] protected DynamicInventoryDisplay _backPackInventory;
    [SerializeField] protected Hero _hero;

    protected EquipSystem _equipSystem;

    protected EquipItem _equipSlot = new EquipItem();
    protected Dictionary<EquipSlot_UI, EquipSlot> _equipDictionary;

    public KeeperDisplay KeeperDisplay { get; private set; }
    public PrepareQuestUIController PrepareQuestUIController { get; private set; }

    public Dictionary<EquipSlot_UI, EquipSlot> EquipDictionary => _equipDictionary;

    public static UnityAction<Hero, EquipSlot_UI> OnHeroEquip;
    public static UnityAction<Hero, EquipSlot_UI> OnHeroPotionsEquip;
    public static UnityAction<Hero, EquipSlot_UI> OnHeroTakeOfEquip;
    public static UnityAction<Hero, EquipSlot> OnHeroTakeOfEquip1;


    protected void Awake()
    {
        KeeperDisplay = GetComponentInParent<KeeperDisplay>();
        PrepareQuestUIController = GetComponentInParent<PrepareQuestUIController>();
    }

    private void OnEnable()
    {
        _equipSlot.Subscribe();
    }

    private void OnDisable()
    {
        _equipSlot.Unsubscribe();
    }

    public virtual void AssignSlot(EquipSystem equipToDisplay, int offset)
    {
        Debug.Log("Assign Slot override");
    }

    public void DisplayEquipWindow(EquipSystem equipSystem, int offset)
    {
        _equipSystem = equipSystem;
    }

    protected virtual void UpdateSlot(InventorySlot updatedSlot)
    {
        foreach (var slot in EquipDictionary)
        {
            if (slot.Value == updatedSlot) // Значение слота - "под худом" слота инвентаря
                slot.Key.UpdateUISlot(updatedSlot); // Ключ слота - UI представление значения
        }
    }

    public void SlotLeftClicked(EquipSlot_UI equipSlot_UI)
    {
        SlotClicked(equipSlot_UI);

        if (_hero != null)
            _hero.EquipHolder.EquipSystem.ResetEquipIndex();
    }

    public void SlotClicked(EquipSlot_UI equipSlot_UI)
    {
        if (_mouseInventoryItem != null && _mouseInventoryItem.AssignedInventorySlot != null && _mouseInventoryItem.AssignedInventorySlot.BaseItemData != null)
            if (_mouseInventoryItem.AssignedInventorySlot is EquipSlot equipSlot && equipSlot.ItemStats.Count == 0 && equipSlot.ItemDefRes.Count == 0
            && equipSlot.ItemDefDebuffs.Count == 0 && equipSlot.ItemDefAffix.Count == 0)
                return;

        if (equipSlot_UI.AssignedEquipSlot != null && equipSlot_UI.AssignedEquipSlot.BaseItemData != null)
            if (equipSlot_UI.AssignedEquipSlot.ItemStats.Count == 0 && equipSlot_UI.AssignedEquipSlot.ItemDefRes.Count == 0
               && equipSlot_UI.AssignedEquipSlot.ItemDefDebuffs.Count == 0 && equipSlot_UI.AssignedEquipSlot.ItemDefAffix.Count == 0)
                return;

        if (KeeperDisplay != null && KeeperDisplay.SelectedHero != null && KeeperDisplay.SelectedHero.Hero != null)
        {
            _hero = KeeperDisplay.SelectedHero.Hero;
        }

        if (PrepareQuestUIController != null && PrepareQuestUIController.SelectedHero != null && PrepareQuestUIController.SelectedHero.Hero != null)
        {
            _hero = PrepareQuestUIController.SelectedHero.Hero;
        }

        if (equipSlot_UI.IsCantEquip)
            return;

        if (!_hero.IsSentOnQuest)
        {
            SlotData equipSlot = equipSlot_UI.AssignedInventorySlot.BaseItemData;
            SlotData mouseSlot = _mouseInventoryItem.AssignedInventorySlot.BaseItemData;

            if (equipSlot != null && mouseSlot == null && Input.GetKey(KeyCode.LeftControl))
            {
                OnHeroTakeOfEquip?.Invoke(KeeperDisplay.SelectedHero.Hero, equipSlot_UI);

                _invDisplay.InventorySystem.AddToInventory(equipSlot_UI.AssignedInventorySlot.ItemData, 1);

                equipSlot_UI.AssignedEquipSlot.ClearSlot();
                equipSlot_UI.UpdateUISlot();
            }


            // Если кликнуть на слот,в котором есть предмет,а у мыши нет элемента, тогда нужно поднять предмет
            else if (equipSlot != null && mouseSlot == null)
            {
                if (KeeperDisplay == null && PrepareQuestUIController == null)
                    return;

                else if (KeeperDisplay != null)
                {
                    _mouseInventoryItem.AssignedInventorySlot = _mouseInventoryItem.SetMouseSlotEquip(_mouseInventoryItem.AssignedInventorySlot,
                         equipSlot_UI.AssignedEquipSlot);
                    EquipSlot copySlot = SetTypeEquipSlot(equipSlot_UI.AssignedEquipSlot, equipSlot_UI.AssignedEquipSlot);
                    EquipSlot_UI copySlot_UI = equipSlot_UI;
                    equipSlot_UI.AssignedEquipSlot.ClearSlot();
                    equipSlot_UI.UpdateUISlot();

                    //if (_mouseInventoryItem.AssignedInventorySlot is EquipSlot equipTrinketSlot && equipTrinketSlot.EquipItemData.EquipType == EquipType.Trinket)
                    //    _mouseInventoryItem.AssignedInventorySlot.RemoveFromStack(_mouseInventoryItem.AssignedInventorySlot.StackSize - 1);

                    OnHeroTakeOfEquip1?.Invoke(_hero, copySlot);

                    _mouseInventoryItem.UpdateMouseSlot(copySlot);

                    KeeperDisplay.UpdateUIInfo(_hero);

                    return;
                }

                else if (PrepareQuestUIController != null)
                {
                    _mouseInventoryItem.AssignedInventorySlot = _mouseInventoryItem.SetMouseSlotEquip(_mouseInventoryItem.AssignedInventorySlot,
                         equipSlot_UI.AssignedEquipSlot);
                    EquipSlot copySlot = SetTypeEquipSlot(equipSlot_UI.AssignedEquipSlot, equipSlot_UI.AssignedEquipSlot);
                    EquipSlot_UI copySlot_UI = equipSlot_UI;
                    equipSlot_UI.AssignedEquipSlot.ClearSlot();
                    equipSlot_UI.UpdateUISlot();

                    OnHeroTakeOfEquip1?.Invoke(_hero, copySlot);

                    _mouseInventoryItem.UpdateMouseSlot(copySlot);

                    PrepareQuestUIController.UpdateUIInfo(_hero);

                    return;
                }
            }

            // Если слот не содержит предмет, а мышь содержит предмет, тогда нужно положить предмет на курсоре мышки в этот пустой слот
            if (mouseSlot != null)
            {
                EquipItemPlayer(equipSlot_UI);
            }
        }
    }

    protected void EquipItemPlayer(EquipSlot_UI equipSlot_UI)
    {
        EquipItemData mouseSlot = (EquipItemData)_mouseInventoryItem.AssignedInventorySlot.ItemData;

        if (equipSlot_UI.ItemType == EquipType.OneHandWeapon && mouseSlot.EquipType == EquipType.OneHandWeapon)
            EquipItemPlayer1(equipSlot_UI);

        else if (equipSlot_UI.ItemType == EquipType.OneHandWeapon && mouseSlot.EquipType == EquipType.TwoHandWeapon)
        {
            if (_weaponCheckSlot_1.AssignedInventorySlot.ItemData == null)
                EquipItemPlayer1(equipSlot_UI);

            else
                return;
        }

        else if (equipSlot_UI.ItemType == EquipType.OffHandWeapon)
        {
            if (_weaponCheckSlot_2.AssignedInventorySlot.ItemData == null || _weaponCheckSlot_2.AssignedEquipSlot.EquipItemData.EquipType == EquipType.OneHandWeapon)
            {
                if (equipSlot_UI.ItemType == mouseSlot.EquipType)
                    EquipItemPlayer1(equipSlot_UI);

                else if (equipSlot_UI.SecondaryItemType == mouseSlot.EquipType)
                    EquipItemPlayer1(equipSlot_UI);

                else return;
            }

            else if (_weaponCheckSlot_2.AssignedEquipSlot.EquipItemData.EquipType == EquipType.TwoHandWeapon)
                return;
        }

        else if (equipSlot_UI.ItemType == mouseSlot.EquipType)
            EquipItemPlayer1(equipSlot_UI);
    }

    protected void EquipItemPlayer1(EquipSlot_UI equipSlot_UI)
    {
        if (equipSlot_UI.AssignedEquipSlot.BaseItemData == null && _mouseInventoryItem.AssignedInventorySlot.BaseItemData != null)
        {
            if (_mouseInventoryItem.AssignedInventorySlot is EquipSlot assignedEquipSlot)
            {
                if (assignedEquipSlot.EquipItemData != null)
                {
                    if (assignedEquipSlot.EquipItemData.EquipType == EquipType.Trinket ||
                    assignedEquipSlot.EquipItemData.EquipType == EquipType.Light)
                    {
                        var newSlot = SetTypeEquipSlot(equipSlot_UI.AssignedEquipSlot, _mouseInventoryItem.AssignedInventorySlot);
                        equipSlot_UI.Init(newSlot);
                        _equipSystem.Slots[equipSlot_UI.EquipIndex] = newSlot;
                        equipSlot_UI.AssignedEquipSlot.RemoveFromStack(newSlot.StackSize - 1);
                        equipSlot_UI.UpdateUISlot();
                    }

                    else
                    {
                        var newSlot = SetTypeEquipSlot(equipSlot_UI.AssignedEquipSlot, _mouseInventoryItem.AssignedInventorySlot);
                        equipSlot_UI.Init(newSlot);
                        _equipSystem.Slots[equipSlot_UI.EquipIndex] = newSlot;
                        equipSlot_UI.UpdateUISlot();
                    }
                }

                else if(_mouseInventoryItem.AssignedInventorySlot is BlankSlot blankSlot)
                {
                    var newSlot = SetTypeEquipSlot(equipSlot_UI.AssignedEquipSlot, _mouseInventoryItem.AssignedInventorySlot);
                    equipSlot_UI.Init(newSlot);
                    _equipSystem.Slots[equipSlot_UI.EquipIndex] = newSlot;
                    //_equipSystem.Slots[equipSlot_UI.EquipIndex].AssignItem(newSlot);
                    equipSlot_UI.UpdateUISlot();
                }
            }


            if (KeeperDisplay == null && PrepareQuestUIController == null)
                return;

            else if (KeeperDisplay != null)
            {
                //OnHeroEquip?.Invoke(KeeperDisplay.SelectedHero.Hero, equipSlot_UI);
                OnHeroEquip?.Invoke(_hero, equipSlot_UI);

                if (_mouseInventoryItem.AssignedInventorySlot.StackSize == 1)
                {
                    _mouseInventoryItem.ClearSlot();
                    KeeperDisplay.UpdateUIInfo(_hero);
                    return;
                }

                else
                {
                    _mouseInventoryItem.AssignedInventorySlot.RemoveFromStack(1);
                    _mouseInventoryItem.UpdateMouseSlot();
                    KeeperDisplay.UpdateUIInfo(_hero);
                    return;
                }


            }

            else if (PrepareQuestUIController != null)
            {
                //OnHeroEquip?.Invoke(PrepareQuestUIController.SelectedHero.Hero, equipSlot_UI);
                OnHeroEquip?.Invoke(_hero, equipSlot_UI);

                if (_mouseInventoryItem.AssignedInventorySlot.StackSize == 1)
                {
                    _mouseInventoryItem.ClearSlot();
                    PrepareQuestUIController.UpdateUIInfo(_hero);
                    return;
                }

                else
                {
                    _mouseInventoryItem.AssignedInventorySlot.RemoveFromStack(1);
                    _mouseInventoryItem.UpdateMouseSlot();
                    PrepareQuestUIController.UpdateUIInfo(_hero);
                    return;
                }

            }

        }

        // Если оба слота содержат предметы, то нужно решить...
        else if (equipSlot_UI.AssignedEquipSlot.BaseItemData != null && _mouseInventoryItem.AssignedInventorySlot.BaseItemData != null)
        {
            SwapSlots(equipSlot_UI);
            return;
        }
    }

    protected void SwapSlots(EquipSlot_UI equipSlot_UI)
    {
        EquipSlot itemInClickedSlot = equipSlot_UI.AssignedEquipSlot;
        InventorySlot itemOnMouse = _mouseInventoryItem.AssignedInventorySlot;

        // Если предметы одинаковые (тот же тип)
        if (itemInClickedSlot.BaseItemData == itemOnMouse.BaseItemData)
        {
            var localMouseBaseData = (EquipItemData)itemOnMouse.BaseItemData;
            int spaceLeftInClickedSlot = itemInClickedSlot.EquipItemData.EquipMaxStackSize - itemInClickedSlot.StackSize;
            int remainingOnMouse = itemOnMouse.StackSize - spaceLeftInClickedSlot;

            // Если можно объединить стаки полностью
            if (itemInClickedSlot.StackSize < itemInClickedSlot.EquipItemData.EquipMaxStackSize)
            {
                Debug.Log("add 1");
                if (_mouseInventoryItem.AssignedInventorySlot.StackSize - 1 == 0)
                {
                    itemInClickedSlot.AddToStack(1);
                    _mouseInventoryItem.AssignedInventorySlot.RemoveFromStack(1);

                    _mouseInventoryItem.ClearSlot();
                    equipSlot_UI.UpdateEquipSlotUI();

                    OnHeroEquip?.Invoke(_hero, equipSlot_UI);
                }

                else
                {
                    itemInClickedSlot.AddToStack(1);
                    _mouseInventoryItem.AssignedInventorySlot.RemoveFromStack(1);

                    _mouseInventoryItem.UpdateMouseSlot();
                    equipSlot_UI.UpdateEquipSlotUI();

                    OnHeroEquip?.Invoke(_hero, equipSlot_UI);
                }
            }

            else
            {
                Debug.Log("add 2");
                if (itemOnMouse.StackSize > localMouseBaseData.EquipMaxStackSize)
                {
                    return;
                }
                if (KeeperDisplay != null)
                {
                    OnHeroTakeOfEquip1?.Invoke(_hero, equipSlot_UI.AssignedEquipSlot);
                    KeeperDisplay.UpdateUIInfo(_hero);
                }

                else if (PrepareQuestUIController != null)
                {
                    OnHeroTakeOfEquip1?.Invoke(_hero, equipSlot_UI.AssignedEquipSlot);
                    PrepareQuestUIController.UpdateUIInfo(_hero);
                }

                var cloneEquipSlot = SetTypeEquipSlot(equipSlot_UI.AssignedEquipSlot, equipSlot_UI.AssignedEquipSlot);
                var cloneMouseSlot = SetTypeEquipSlot((EquipSlot)_mouseInventoryItem.AssignedInventorySlot, _mouseInventoryItem.AssignedInventorySlot);

                equipSlot_UI.Init(cloneMouseSlot);
                _equipSystem.Slots[equipSlot_UI.EquipIndex] = cloneMouseSlot;
                _mouseInventoryItem.AssignedInventorySlot.ClearSlot();
                _mouseInventoryItem.AssignedInventorySlot = _mouseInventoryItem.SetMouseSlotEquip(_mouseInventoryItem.AssignedInventorySlot, cloneEquipSlot);

                equipSlot_UI.UpdateUISlot();

                if (KeeperDisplay != null)
                {
                    if (itemInClickedSlot.EquipItemData.ItemSecondaryType == ItemSecondaryType.Potion)
                        OnHeroPotionsEquip?.Invoke(_hero, equipSlot_UI);

                    else
                        OnHeroEquip?.Invoke(_hero, equipSlot_UI);

                    KeeperDisplay.UpdateUIInfo(_hero);
                }

                else if (PrepareQuestUIController != null)
                {
                    if (itemInClickedSlot.EquipItemData.ItemSecondaryType == ItemSecondaryType.Potion)
                        OnHeroPotionsEquip?.Invoke(_hero, equipSlot_UI);

                    else
                        OnHeroEquip?.Invoke(_hero, equipSlot_UI);

                    PrepareQuestUIController.UpdateUIInfo(_hero);
                }

                _mouseInventoryItem.UpdateMouseSlot(cloneEquipSlot);

                return;
            }
        }

        // Если предметы разные, меняем их местами
        else if (itemInClickedSlot.BaseItemData != itemOnMouse.BaseItemData)
        {
            var localBaseData = (EquipItemData)itemInClickedSlot.BaseItemData;

            if (itemOnMouse.ItemData is EquipItemData equipMouseSlotData && equipMouseSlotData.EquipMaxStackSize >= itemOnMouse.StackSize)
            {
                if (KeeperDisplay != null)
                {
                    OnHeroTakeOfEquip1?.Invoke(_hero, equipSlot_UI.AssignedEquipSlot);
                    KeeperDisplay.UpdateUIInfo(_hero);
                }

                else if (PrepareQuestUIController != null)
                {
                    OnHeroTakeOfEquip1?.Invoke(_hero, equipSlot_UI.AssignedEquipSlot);
                    PrepareQuestUIController.UpdateUIInfo(_hero);
                }

                var cloneEquipSlot = SetTypeEquipSlot(equipSlot_UI.AssignedEquipSlot, equipSlot_UI.AssignedEquipSlot);
                var cloneMouseSlot = SetTypeEquipSlot((EquipSlot)_mouseInventoryItem.AssignedInventorySlot, _mouseInventoryItem.AssignedInventorySlot);

                equipSlot_UI.Init(cloneMouseSlot);
                _equipSystem.Slots[equipSlot_UI.EquipIndex] = cloneMouseSlot;
                _mouseInventoryItem.AssignedInventorySlot.ClearSlot();
                _mouseInventoryItem.AssignedInventorySlot = _mouseInventoryItem.SetMouseSlotEquip(_mouseInventoryItem.AssignedInventorySlot, cloneEquipSlot);

                equipSlot_UI.UpdateUISlot();

                if (KeeperDisplay != null)
                {
                    if (localBaseData.ItemSecondaryType == ItemSecondaryType.Potion)
                        OnHeroPotionsEquip?.Invoke(_hero, equipSlot_UI);

                    else
                        OnHeroEquip?.Invoke(_hero, equipSlot_UI);

                    KeeperDisplay.UpdateUIInfo(_hero);
                }

                else if (PrepareQuestUIController != null)
                {
                    if (localBaseData.ItemSecondaryType == ItemSecondaryType.Potion)
                        OnHeroPotionsEquip?.Invoke(_hero, equipSlot_UI);

                    else
                        OnHeroEquip?.Invoke(_hero, equipSlot_UI);

                    PrepareQuestUIController.UpdateUIInfo(_hero);
                }

                _mouseInventoryItem.UpdateMouseSlot(cloneEquipSlot);
            }
        }
    }

    protected virtual EquipSlot SetTypeEquipSlot(EquipSlot equipSlot, InventorySlot slotToCompare)
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
}
