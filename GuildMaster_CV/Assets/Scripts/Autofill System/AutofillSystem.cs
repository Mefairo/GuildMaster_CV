using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AutofillSystem : MonoBehaviour
{
    [Header("Main Parametres")]
    [SerializeField] private AutoFillSlot_UI _slotPrefab;
    [SerializeField] private PlayerInventoryHolder _playerInventory;
    [SerializeField] private DynamicEquipDisplay _dynamicEquipDisplay;
    [Header("Panels")]
    [SerializeField] private GameObject _foodPanel;
    [SerializeField] private GameObject _lightPanel;
    [SerializeField] private GameObject _healPanel;
    [SerializeField] private GameObject _manaPanel;
    [Header("Show Panel Buttons")]
    [SerializeField] private Button _foodButton;
    [SerializeField] private Button _lightButton;
    [SerializeField] private Button _healButton;
    [SerializeField] private Button _manaButton;
    [Header("Item Lists")]
    [SerializeField] private List<AutoFillSlot_UI> _foodSlots = new List<AutoFillSlot_UI>();
    [SerializeField] private List<AutoFillSlot_UI> _lightSlots = new List<AutoFillSlot_UI>();
    [SerializeField] private List<AutoFillSlot_UI> _healSlots = new List<AutoFillSlot_UI>();
    [SerializeField] private List<AutoFillSlot_UI> _manaSlots = new List<AutoFillSlot_UI>();

    public MainQuestKeeperDisplay MainQuestKeeperDisplay { get; private set; }
    public QuestParametresSystemFix QuestParametresSystemFix { get; private set; }

    public static UnityAction OnAutoFillSupplies;

    private void Awake()
    {
        _foodButton?.onClick.RemoveAllListeners();
        _lightButton?.onClick.RemoveAllListeners();
        _healButton?.onClick.RemoveAllListeners();
        _manaButton?.onClick.RemoveAllListeners();

        _foodButton?.onClick.AddListener(ShowFoodPanel);
        _lightButton?.onClick.AddListener(ShowLightPanel);
        _healButton?.onClick.AddListener(ShowHealPanel);
        _manaButton?.onClick.AddListener(ShowManaPanel);

        MainQuestKeeperDisplay = GetComponentInParent<MainQuestKeeperDisplay>();
        QuestParametresSystemFix = GetComponentInParent<QuestParametresSystemFix>();
    }

    private void Start()
    {
        ClearSlots(_foodPanel.transform);
        ClearSlots(_lightPanel.transform);
        ClearSlots(_healPanel.transform);
        ClearSlots(_manaPanel.transform);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!IsPointerOverSlot())
            {
                ClearInfo(_foodPanel, _foodSlots);
                ClearInfo(_lightPanel, _lightSlots);
                ClearInfo(_healPanel, _healSlots);
                ClearInfo(_manaPanel, _manaSlots);
            }

        }
    }

    private bool IsPointerOverSlot()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        // Проверяем, есть ли среди результатов слоты
        return results.Exists(r => r.gameObject.GetComponent<AutoFillSlot_UI>() != null);
    }

    public void AutoFillItem(AutoFillSlot_UI item)
    {
        if (item.AutoFillSlot.BaseItemData is ConsumableItemData consumData)
        {
            switch (consumData.TypeConsumItem)
            {
                case TypeConsumableItem.Food:
                    AutoFillItems(item, consumData, QuestParametresSystemFix.FoodAvailable, QuestParametresSystemFix.FoodRequired, false);
                    ClearInfo(_foodPanel, _foodSlots);
                    _foodSlots = new List<AutoFillSlot_UI>();
                    break;

                case TypeConsumableItem.Light:
                    AutoFillItems(item, consumData, QuestParametresSystemFix.LightAvailable, QuestParametresSystemFix.LightRequired, true);
                    ClearInfo(_lightPanel, _lightSlots);
                    _lightSlots = new List<AutoFillSlot_UI>();
                    break;

                case TypeConsumableItem.Heal:
                    AutoFillItems(item, consumData, QuestParametresSystemFix.HealAvailable, QuestParametresSystemFix.HealRequired, false);
                    ClearInfo(_healPanel, _healSlots);
                    _healSlots = new List<AutoFillSlot_UI>();
                    break;

                case TypeConsumableItem.Mana:
                    AutoFillItems(item, consumData, QuestParametresSystemFix.ManaAvailable, QuestParametresSystemFix.ManaRequired, false);
                    ClearInfo(_manaPanel, _manaSlots);
                    _manaSlots = new List<AutoFillSlot_UI>();
                    break;
            }
        }

        if (MainQuestKeeperDisplay.SelectedHero != null && MainQuestKeeperDisplay.SelectedHero.Hero != null)
            _dynamicEquipDisplay.RefreshDynamicInventory(MainQuestKeeperDisplay.SelectedHero.Hero.EquipHolder.EquipSystem, 0);

        OnAutoFillSupplies?.Invoke();
    }

    private void AutoFillItems(AutoFillSlot_UI item, ConsumableItemData consumData, List<float> listAvailable, List<float> listRequired, bool isLight)
    {
        var testItem = SetTypeEquipSlot(item.AutoFillSlot);
        int amount = item.ActiveStackSize;

        for (int i = 0; i < MainQuestKeeperDisplay.HeroesSlots.Slots.Count; i++)
        {
            HeroQuestSlot_UI heroSlot = MainQuestKeeperDisplay.HeroesSlots.Slots[i];

            if (heroSlot.Hero != null && heroSlot.Hero.HeroData != null)
            {
                CalculateItemsForHero(heroSlot, testItem, consumData, ref amount, listAvailable, listRequired, isLight);
            }
        }
    }

    private void CalculateItemsForHero(HeroQuestSlot_UI heroSlot, EquipSlot item, ConsumableItemData consumData, ref int amount,
        List<float> listAvailable, List<float> listRequired, bool isLight)
    {
        if (item.BaseItemData == null)
            Debug.Log("fill null 3");
        if (listAvailable[heroSlot.SlotIndex] < listRequired[heroSlot.SlotIndex])
        {
            float valueRequired = listRequired[heroSlot.SlotIndex] - listAvailable[heroSlot.SlotIndex];
            float itemsRequired = valueRequired / consumData.ItemValue;
            int itemsToAdd = Mathf.CeilToInt(itemsRequired);

            List<EquipSlot> trinketList = SetEquipSlotType(heroSlot, isLight);

            bool canAddToAmountItem = false;

            for (int t = 0; t < trinketList.Count; t++)
            {
                if (trinketList[t] == null && trinketList[t].BaseItemData == null)
                    continue;

                else
                {
                    if (trinketList[t].BaseItemData != consumData)
                        continue;

                    else
                    {
                        canAddToAmountItem = true;

                        if (amount >= itemsToAdd && amount != 0)
                        {
                            AddTrinketAmountToStack(trinketList[t], item, itemsToAdd, trinketList, heroSlot);
                            amount -= itemsToAdd;
                        }

                        else if(amount < itemsToAdd && amount != 0)
                        {
                            AddTrinketAmountToStack(trinketList[t], item, amount, trinketList, heroSlot);
                            amount = 0;
                        }

                        break;
                    }
                }
            }

            if (!canAddToAmountItem)
            {
                var freeSlot = trinketList.FirstOrDefault(f => f.BaseItemData == null);

                if (freeSlot == null)
                    return;

                // если предметов в инвентаре ДОСТАТОЧНО для полного покрытия автофила
                if (amount >= itemsToAdd && amount != 0)
                {
                    // если необходимое количество автофила равно или меньше макс. колву в эквип слоте
                    if (itemsToAdd <= consumData.EquipMaxStackSize)
                    {
                        AddToStackForEquipSlot(freeSlot, item, heroSlot, itemsToAdd);
                        amount -= itemsToAdd;
                        _playerInventory.PrimaryInventorySystem.RemoveItemsFromInventory(item.BaseItemData, itemsToAdd);
                    }

                    else
                    {
                        if (!isLight)
                        {
                            AddItemsOverMaxStack(trinketList, item, heroSlot, itemsToAdd, freeSlot);
                            amount -= itemsToAdd;
                        }
                    }
                }

                // если НЕДОСТАТОЧНО предметов в инвентаре для полного покрытия автофила
                else if (amount < itemsToAdd && amount != 0)
                {
                    int fillAmount = itemsToAdd - amount;

                    // если необходимое количество автофила равно или меньше макс. колву в эквип слоте
                    if (amount <= consumData.EquipMaxStackSize)
                    {
                        AddToStackForEquipSlot(freeSlot, item, heroSlot, amount);
                        _playerInventory.PrimaryInventorySystem.RemoveItemsFromInventory(item.BaseItemData, amount);
                        amount = 0;
                    }

                    else
                    {
                        if (!isLight)
                        {
                            AddItemsOverMaxStack(trinketList, item, heroSlot, amount, freeSlot);
                            amount = 0;

                        }
                    }
                }
            }
        }
    }

    private void AddItemsOverMaxStack(List<EquipSlot> trinketList, EquipSlot item, HeroQuestSlot_UI heroSlot, int itemsToAdd, EquipSlot freeSlot)
    {
        int maxStackSize = item.EquipItemData.EquipMaxStackSize;
        int remainingItems = itemsToAdd - maxStackSize;

        AddToStackForEquipSlot(freeSlot, item, heroSlot, maxStackSize);

        var nextFreeSlot = trinketList.FirstOrDefault(f => f.BaseItemData == null);

        if (nextFreeSlot == null)
            return;

        AddToStackForEquipSlot(nextFreeSlot, item, heroSlot, remainingItems);

        _playerInventory.PrimaryInventorySystem.RemoveItemsFromInventory(item.BaseItemData, itemsToAdd);
    }

    private void AddToStackForEquipSlot(EquipSlot freeSlot, EquipSlot item, HeroQuestSlot_UI heroSlot, int amount)
    {
        var newSlot = SetTypeEquipSlot(item);
        heroSlot.Hero.EquipHolder.EquipSystem.Slots[freeSlot.EquipIndex].AssignItem(newSlot);

        heroSlot.Hero.EquipHolder.EquipSystem.Slots[freeSlot.EquipIndex].AddToStack(amount - 1);
    }

    private void AddTrinketAmountToStack(EquipSlot trinketSlot, EquipSlot item, int itemsToAdd, List<EquipSlot> trinketList, HeroQuestSlot_UI heroSlot)
    {
        int spaceLeftInClickedSlot = trinketSlot.EquipItemData.EquipMaxStackSize - trinketSlot.StackSize;

        if (spaceLeftInClickedSlot >= itemsToAdd)
        {
            trinketSlot.AddToStack(itemsToAdd);

            _playerInventory.PrimaryInventorySystem.RemoveItemsFromInventory(trinketSlot.BaseItemData, itemsToAdd);
        }

        else
        {
            trinketSlot.AddToStack(spaceLeftInClickedSlot);
            int remainingItems = itemsToAdd - spaceLeftInClickedSlot;

            var freeSlot = trinketList.FirstOrDefault(f => f.BaseItemData == null);

            if (freeSlot == null)
                return;

            AddToStackForEquipSlot(freeSlot, item, heroSlot, remainingItems);

            _playerInventory.PrimaryInventorySystem.RemoveItemsFromInventory(trinketSlot.BaseItemData, itemsToAdd);
        }
    }

    #region SHOW_PANELS
    private void ShowFoodPanel()
    {
        if (!_foodPanel.gameObject.activeInHierarchy)
        {
            _foodPanel.SetActive(true);

            foreach (InventorySlot invSlot in _playerInventory.PrimaryInventorySystem.InventorySlots)
            {
                if (invSlot != null && invSlot.BaseItemData != null)
                {
                    if (invSlot.ItemData is ConsumableItemData consumData && consumData.ItemType == ItemType.Food)
                        CheckSlots(invSlot, _foodSlots, _foodPanel.transform);
                }
            }
        }

        else
            ClearInfo(_foodPanel, _foodSlots);
    }

    private void ShowLightPanel()
    {
        if (!_lightPanel.gameObject.activeInHierarchy)
        {
            _lightPanel.SetActive(true);

            foreach (InventorySlot invSlot in _playerInventory.PrimaryInventorySystem.InventorySlots)
            {
                if (invSlot != null && invSlot.BaseItemData != null)
                {
                    if (invSlot.ItemData is ConsumableItemData consumData && consumData.TypeConsumItem == TypeConsumableItem.Light)
                        CheckSlots(invSlot, _lightSlots, _lightPanel.transform);
                }
            }
        }

        else
            ClearInfo(_lightPanel, _lightSlots);
    }

    private void ShowHealPanel()
    {
        if (!_healPanel.gameObject.activeInHierarchy)
        {
            _healPanel.SetActive(true);

            foreach (InventorySlot invSlot in _playerInventory.PrimaryInventorySystem.InventorySlots)
            {
                if (invSlot != null && invSlot.BaseItemData != null)
                {
                    if (invSlot.ItemData is ConsumableItemData consumData && consumData.TypeConsumItem == TypeConsumableItem.Heal)
                        CheckSlots(invSlot, _healSlots, _healPanel.transform);
                }
            }
        }

        else
            ClearInfo(_healPanel, _healSlots);
    }

    private void ShowManaPanel()
    {
        if (!_manaPanel.gameObject.activeInHierarchy)
        {
            _manaPanel.SetActive(true);

            foreach (InventorySlot invSlot in _playerInventory.PrimaryInventorySystem.InventorySlots)
            {
                if (invSlot != null && invSlot.BaseItemData != null)
                {
                    if (invSlot.ItemData is ConsumableItemData consumData && consumData.TypeConsumItem == TypeConsumableItem.Mana)
                        CheckSlots(invSlot, _manaSlots, _manaPanel.transform);
                }
            }
        }

        else
        {
            ClearInfo(_manaPanel, _manaSlots);
        }
    }
    #endregion

    private void CreateAutoFillSlot(InventorySlot invSlot, List<AutoFillSlot_UI> contentList, Transform panelTransform)
    {
        AutoFillSlot_UI newSlot = Instantiate(_slotPrefab, panelTransform);
        newSlot.Init(invSlot);

        contentList.Add(newSlot);
    }

    private void ClearSlots(Transform tr)
    {
        foreach (Transform slot in tr)
        {
            Destroy(slot.gameObject);
        }
    }

    private void CheckSlots(InventorySlot invSlot, List<AutoFillSlot_UI> contentList, Transform panelTransform)
    {
        if (contentList.Count == 0)
        {
            CreateAutoFillSlot(invSlot, contentList, panelTransform);
        }

        else
        {
            // Буфер для новых слотов, которые нужно создать
            List<InventorySlot> slotsToAdd = new List<InventorySlot>();

            bool found = false;

            foreach (AutoFillSlot_UI slot in contentList)
            {
                if (slot.AutoFillSlot.BaseItemData == invSlot.BaseItemData)
                {
                    slot.AddToStack(invSlot.StackSize);
                    found = true;
                    break; // Если нашёл подходящий — выходим
                }
            }

            // Если не нашли подходящий слот — добавляем новый после цикла
            if (!found)
            {
                CreateAutoFillSlot(invSlot, contentList, panelTransform);
            }
        }
    }

    private void ClearInfo(GameObject panel, List<AutoFillSlot_UI> list)
    {
        //list = new List<AutoFillSlot_UI>();
        list.Clear();
        ClearSlots(panel.transform);
        panel.SetActive(false);
    }

    protected EquipSlot SetTypeEquipSlot(InventorySlot slotToCompare)
    {
        EquipSlot newSlot;

        if (slotToCompare.BaseItemData is BlankSlotData blankData)
            newSlot = new BlankSlot(blankData, 1);

        else if (slotToCompare.BaseItemData is EquipItemData equipData)
            newSlot = new EquipSlot(equipData);

        else
            newSlot = new EquipSlot();

        return newSlot;
    }

    private List<EquipSlot> SetEquipSlotType(HeroQuestSlot_UI heroSlot, bool isLight)
    {
        List<EquipSlot> trinketList = new List<EquipSlot>();

        if (isLight)
        {
            trinketList.Add(heroSlot.Hero.EquipHolder.EquipSystem.Slots[18]);
        }

        else
        {
            trinketList.Add(heroSlot.Hero.EquipHolder.EquipSystem.Slots[9]);
            trinketList.Add(heroSlot.Hero.EquipHolder.EquipSystem.Slots[10]);
            trinketList.Add(heroSlot.Hero.EquipHolder.EquipSystem.Slots[11]);
            trinketList.Add(heroSlot.Hero.EquipHolder.EquipSystem.Slots[12]);
            trinketList.Add(heroSlot.Hero.EquipHolder.EquipSystem.Slots[13]);
            trinketList.Add(heroSlot.Hero.EquipHolder.EquipSystem.Slots[14]);
            trinketList.Add(heroSlot.Hero.EquipHolder.EquipSystem.Slots[15]);
            trinketList.Add(heroSlot.Hero.EquipHolder.EquipSystem.Slots[16]);
            trinketList.Add(heroSlot.Hero.EquipHolder.EquipSystem.Slots[17]);
        }

        return trinketList;
    }
}
