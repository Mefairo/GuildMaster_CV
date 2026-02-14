using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class CraftKeeperDisplay : MonoBehaviour
{
    [SerializeField] private CraftSlotUI _craftSlotPrefab;
    [SerializeField] private Button _craftButton;
    [SerializeField] private ItemCraft _craftMaterialPrefab;
    [SerializeField] private List<CraftSlotUI> _craftSlotList = new List<CraftSlotUI>();
    [Space]
    [Header("Item Preview Section")]
    [SerializeField] private TextMeshProUGUI _itemPreviewDescription;
    [SerializeField] private TextMeshProUGUI _itemPrimaryType;
    [SerializeField] private TextMeshProUGUI _itemSecondaryType;
    [SerializeField] private TextMeshProUGUI _powerStatsNameText;
    [SerializeField] private TextMeshProUGUI _powerStatsValueText;
    [SerializeField] private TextMeshProUGUI _defenceStatsNameText;
    [SerializeField] private TextMeshProUGUI _defenceStatsValueText;
    [Space]
    [Header("Cart")]
    [SerializeField] private GameObject _itemListContentPanel;
    [SerializeField] private GameObject _craftingCartContentPanel;
    [Space]
    [Header("Type Tab Buttons")]
    [SerializeField] private GameObject _alchemtistTabs;
    [SerializeField] private GameObject _cookTabs;
    [SerializeField] private GameObject _jewelerTabs;
    [SerializeField] private GameObject _workshopTabs;
    [SerializeField] private GameObject _blacksmithTabs;
    [SerializeField] private GameObject _tannerTabs;
    [SerializeField] private GameObject _tailorTabs;
    [SerializeField] private GameObject _magicShopTabs;
    [Header("Other")]
    [SerializeField] private ShopKeeperDisplay _shopKeeperDisplay;
    [SerializeField] private ResImage_UI _prefabImage;
    [SerializeField] private Transform _resContainer;
    [SerializeField] private Transform _debuffContainer;
    [SerializeField] private Button _craftSwithButton;

    private CraftSlotUI _selectedCraftSlotUI;
    private ItemCraft _selectedRequiredItem;
    private CraftItemData _selectItemData;
    private CraftSystem _craftSystem;
    private ShopSystem _shopSystem;
    private PlayerInventoryHolder _playerInventoryHolder;
    private ShopTypes _craftType;
    private StatDisplayManager _statDisplayManager = new StatDisplayManager();
    private QuestResistanceSystem _questResistanceSystem;

    private Dictionary<CraftItemData, ItemCraft> _craftingCartUI = new Dictionary<CraftItemData, ItemCraft>();

    private void Awake()
    {
        _questResistanceSystem = GetComponent<QuestResistanceSystem>();
        _craftSwithButton.onClick.AddListener(SwitchOnShopWindow);
    }

    private void OnEnable()
    {
        _questResistanceSystem.ClearIcons();

        ClearContainer(_resContainer);
        ClearContainer(_debuffContainer);
    }

    private void OnDisable()
    {
        _selectItemData = null;
    }

    public void DisplayCraftWindow(CraftSystem craftSystem, PlayerInventoryHolder playerInventoryHolder, ShopTypes craftType, ShopSystem shopSystem)
    {
        _craftSystem = craftSystem;
        _shopSystem = shopSystem;
        _playerInventoryHolder = playerInventoryHolder;
        _craftType = craftType;

        RefreshDisplay();
        DisplayCraftInventory();
    }

    private void RefreshDisplay()
    {
        if (_craftButton != null)
        {
            _craftButton.onClick.RemoveAllListeners();
            _craftButton.onClick.AddListener(CraftItemWrapper);
        }

        ClearSlot();
        ClearResourcesList();
        ClearItemPreview();

        _craftButton.gameObject.SetActive(false);
    }

    private void ClearSlot()
    {
        //_craftCart = new Dictionary<InventoryItemData, int>();
        _craftingCartUI = new Dictionary<CraftItemData, ItemCraft>();
        _craftSlotList.Clear();

        foreach (Transform item in _itemListContentPanel.transform.Cast<Transform>())
        {
            Destroy(item.gameObject);
        }

    }

    private void ClearResourcesList()
    {
        foreach (Transform item in _craftingCartContentPanel.transform.Cast<Transform>())
        {
            Destroy(item.gameObject);
        }
    }

    public void TabClicked(CheckTypeForTabs tab)
    {
        RefreshDisplay();

        foreach (CraftSlot item in _craftSystem.CraftList)
        {
            if (item.ItemData == null)
                continue;

            if (tab.ItemType == ItemType.All)
            {
                CraftSlotUI craftSlot = Instantiate(_craftSlotPrefab, _itemListContentPanel.transform);
                int amountCraftItem = CheckAmountCraftItems(item);
                craftSlot.Init(item, amountCraftItem);
                _craftSlotList.Add(craftSlot);
            }

            else
            {
                if (item.ItemData.ItemType == tab.ItemType)
                {
                    CraftSlotUI craftSlot = Instantiate(_craftSlotPrefab, _itemListContentPanel.transform);
                    int amountCraftItem = CheckAmountCraftItems(item);
                    craftSlot.Init(item, amountCraftItem);
                    _craftSlotList.Add(craftSlot);
                }

                else
                    continue;
            }
        }
    }

    private int CheckAmountCraftItems(CraftSlot item)
    {
        List<int> localAmountHeroMaterials = new List<int>();

        foreach (CraftRecipe itemsRecipe in item.CraftItemData.Recipes)
        {
            for (int i = 0; i < itemsRecipe.RequiredItems.Count; i++)
            {
                int heroAmount = CheckHeroAmountItems(itemsRecipe.RequiredItems[i]);

                int amount = 0;

                if (heroAmount >= itemsRecipe.AmountResources[i])
                    amount = heroAmount / itemsRecipe.AmountResources[i];

                localAmountHeroMaterials.Add(amount);
            }
        }

        if (localAmountHeroMaterials.Any())
        {
            int minAmount = localAmountHeroMaterials.Min();

            return minAmount;
        }

        else
            return 0;
    }

    private void DisplayCraftInventory()
    {
        RefreshDisplay();

        _alchemtistTabs.gameObject.SetActive(false);
        _cookTabs.gameObject.SetActive(false);
        _jewelerTabs.gameObject.SetActive(false);
        _workshopTabs.gameObject.SetActive(false);
        _blacksmithTabs.gameObject.SetActive(false);
        _tannerTabs.gameObject.SetActive(false);
        _tailorTabs.gameObject.SetActive(false);
        _magicShopTabs.gameObject.SetActive(false);

        switch (_craftType)
        {
            case ShopTypes.Alchemist:
                _alchemtistTabs.gameObject.SetActive(true);
                break;

            case ShopTypes.Cook:
                _cookTabs.gameObject.SetActive(true);
                break;

            case ShopTypes.Jeweler:
                _jewelerTabs.gameObject.SetActive(true);
                break;

            case ShopTypes.Workshop:
                _workshopTabs.gameObject.SetActive(true);
                break;

            case ShopTypes.Blacksmith:
                _blacksmithTabs.gameObject.SetActive(true);
                break;

            case ShopTypes.Tanner:
                _tannerTabs.gameObject.SetActive(true);
                break;

            case ShopTypes.Tailor:
                _tailorTabs.gameObject.SetActive(true);
                break;

            case ShopTypes.MagicShop:
                _magicShopTabs.gameObject.SetActive(true);
                break;
        }

        foreach (CraftSlot item in _craftSystem.CraftList)
        {
            if (item.ItemData == null)
                continue;

            CraftSlotUI craftSlot = Instantiate(_craftSlotPrefab, _itemListContentPanel.transform);
            int amountCraftItem = CheckAmountCraftItems(item);
            craftSlot.Init(item, amountCraftItem);
            _craftSlotList.Add(craftSlot);
        }
    }

    public void UpdateItemPreview(CraftSlotUI craftSlotUI)
    {
        ClearItemPreview();

        _itemPreviewDescription.text = craftSlotUI.AssignedItemSlot.CraftItemData.Description;

        _selectedCraftSlotUI = craftSlotUI;
        _selectItemData = craftSlotUI.AssignedItemSlot.CraftItemData;

        if (craftSlotUI.AssignedItemSlot.CraftItemData is RuneSlotData runeData)
        {
            foreach (RuneSlotStats stat in runeData.RuneSlotStats)
            {
                (string, string, string, string) statsText = _statDisplayManager.SetRuneStats(runeData);

                _powerStatsNameText.text = statsText.Item1;
                _powerStatsValueText.text = statsText.Item2;

                _defenceStatsNameText.text = statsText.Item3;
                _defenceStatsValueText.text = statsText.Item4;

                _itemPrimaryType.text = runeData.ItemPrimaryType.ToString();
            }
        }

        else if (craftSlotUI.AssignedItemSlot.CraftItemData is EquipItemData equiptemData)
        {
            foreach (HeroStats stat in equiptemData.Stats)
            {
                if (stat != null)
                {
                    (string, string, string, string) StatsText = _statDisplayManager.SetStatsValueText(stat, false);

                    _powerStatsNameText.text = StatsText.Item1;
                    _powerStatsValueText.text = StatsText.Item2;

                    _defenceStatsNameText.text = StatsText.Item3;
                    _defenceStatsValueText.text = StatsText.Item4;

                    _itemPrimaryType.text = equiptemData.ItemPrimaryType.ToString();
                    _itemSecondaryType.text = equiptemData.EquipType.ToString();

                    if (equiptemData.EquipType == EquipType.Trinket)
                    {
                        _itemSecondaryType.text = equiptemData.ItemSecondaryType.ToString();
                    }
                }
            }
        }

        UpdateResistance();
        UpdateDebuffs();
    }

    public void UpdateItemPreview(ItemCraft itemCraft)
    {
        ClearItemPreview();

        _itemPreviewDescription.text = itemCraft.ItemData.Description;

        _selectedRequiredItem = itemCraft;
        _selectItemData = (CraftItemData)itemCraft.ItemData;

        if (itemCraft.ItemData is RuneSlotData runeData)
        {
            foreach (RuneSlotStats stat in runeData.RuneSlotStats)
            {
                (string, string, string, string) statsText = _statDisplayManager.SetRuneStats(runeData);

                _powerStatsNameText.text = statsText.Item1;
                _powerStatsValueText.text = statsText.Item2;

                _defenceStatsNameText.text = statsText.Item3;
                _defenceStatsValueText.text = statsText.Item4;

                _itemPrimaryType.text = runeData.ItemPrimaryType.ToString();
            }
        }

        else if (itemCraft.ItemData is EquipItemData equiptemData)
        {
            foreach (HeroStats stat in equiptemData.Stats)
            {
                if (stat != null)
                {
                    (string, string, string, string) StatsText = _statDisplayManager.SetStatsValueText(stat, false);

                    _powerStatsNameText.text = StatsText.Item1;
                    _powerStatsValueText.text = StatsText.Item2;

                    _defenceStatsNameText.text = StatsText.Item3;
                    _defenceStatsValueText.text = StatsText.Item4;

                    _itemPrimaryType.text = equiptemData.ItemPrimaryType.ToString();
                    _itemSecondaryType.text = equiptemData.EquipType.ToString();

                    if (equiptemData.EquipType == EquipType.Trinket)
                    {
                        _itemSecondaryType.text = equiptemData.ItemSecondaryType.ToString();
                    }
                }
            }
        }

        if (itemCraft.ItemData is DrawingItemData drawingItemData)
            _itemSecondaryType.text = "Recipe";

        //if()

        UpdateResistance();
        UpdateDebuffs();
    }

    private void UpdateResistance()
    {
        if (_selectItemData is RuneSlotData runeData)
        {
            ClearContainer(_resContainer);

            foreach (RuneSlotResistance res in runeData.RuneSlotRes)
            {
                ResImage_UI imageRes = Instantiate(_prefabImage, _resContainer.transform);

                imageRes.SetTypeDamage(res.ResType, true);
                imageRes.Percent.text = $"{res.MinValueRes.ToString()}-{res.MaxValueRes.ToString()}(%)";

            }
        }

        else if (_selectItemData is EquipItemData equiptemData)
        {
            if (equiptemData.Resistance.Count != 0)
            {
                List<Resistance> resistances = equiptemData.Resistance[0].Resistances;

                _questResistanceSystem.ShowResistance(resistances);
            }
        }
    }

    private void UpdateDebuffs()
    {
        ClearContainer(_debuffContainer);

        if (_selectItemData is RuneSlotData runeData)
        {
            foreach (Debuff debuff in runeData.DebuffLists)
            {
                ResImage_UI imageDebuff = Instantiate(_prefabImage, _debuffContainer.transform);

                imageDebuff.SetTypeDebuff(debuff);
                imageDebuff.ImageSelf.sprite = debuff.DebuffData.Icon;
            }
        }

        else if (_selectItemData is EquipItemData equiptemData)
        {
            if (equiptemData.NeutralizeDebuffsData.Count != 0)
            {
                foreach (Debuff debuff in equiptemData.NeutralizeDebuffsData)
                {
                    ResImage_UI imageDebuff = Instantiate(_prefabImage, _debuffContainer.transform);

                    imageDebuff.SetTypeDebuff(debuff);
                    imageDebuff.ImageSelf.sprite = debuff.DebuffData.Icon;
                }
            }
        }
    }

    private void ClearItemPreview()
    {
        _itemPreviewDescription.text = "";

        _itemPrimaryType.text = "";
        _itemSecondaryType.text = "";

        _powerStatsNameText.text = "";
        _powerStatsValueText.text = "";

        _defenceStatsNameText.text = "";
        _defenceStatsValueText.text = "";

        _questResistanceSystem.ClearIcons();

        ClearContainer(_resContainer);
        ClearContainer(_debuffContainer);
    }

    private int CheckHeroAmountItems(SlotData data)
    {
        int heroAmountItems = 0;

        foreach (InventorySlot invSlot in _playerInventoryHolder.PrimaryInventorySystem.InventorySlots)
        {
            if (data == invSlot.BaseItemData)
                heroAmountItems += invSlot.StackSize;
        }

        return heroAmountItems;
    }

    public void SelectItemCraft(CraftSlotUI craftSlotUI)
    {
        ClearResourcesList();
        _selectedCraftSlotUI = craftSlotUI;

        CraftRecipe data = craftSlotUI.AssignedItemSlot.CraftItemData.Recipes[0];

        if (data.RequiredItems.Count != data.AmountResources.Count)
        {
            Debug.LogError("Количество элементов в списке RequiredItems не соответствует количеству элементов в списке AmountResources.");
            return;
        }

        for (int i = 0; i < data.RequiredItems.Count; i++)
        {
            SlotData requiredItem = data.RequiredItems[i];
            int requiredAmount = data.AmountResources[i];
            int heroAmountItems = CheckHeroAmountItems(requiredItem);
            Sprite requiredImage = data.RequiredItems[i].Icon;
            //Sprite requiredImage = craftSlotUI.AssignedItemSlot.CraftItemData.Icon;
            Sprite requiredImageBackground = data.RequiredItems[i].IconBackground;
            //ItemCraft requiredPrefab = data.CraftPrefab[i];

            ItemCraft requiredPrefab = Instantiate(_craftMaterialPrefab, _craftingCartContentPanel.transform);
            requiredPrefab.SetItemComponents(requiredItem, requiredItem.DisplayName, requiredAmount.ToString(), requiredImage, heroAmountItems);

            if (!CanCraftItem(requiredItem, requiredAmount))
            {
                requiredPrefab.NameComponent.color = Color.red;
                requiredPrefab.AmountComponent.color = Color.red;
            }

            else
            {
                requiredPrefab.NameComponent.color = Color.black;
                requiredPrefab.AmountComponent.color = Color.black;
            }
        }

    }

    public void CraftItem(CraftSlotUI craftSlotUI)
    {
        if (CanCraftItem(craftSlotUI))
        {
            var newSlot = new InventorySlot(craftSlotUI.AssignedItemSlot.CraftItemData, 1);
            var newSlotToAdd = SetTypeNewItem(craftSlotUI.AssignedItemSlot.CraftItemData);

            _playerInventoryHolder.PrimaryInventorySystem.AddToInventory(newSlotToAdd, 1);
            RemoveItemOnCraft(craftSlotUI);
            CanCraftItem(craftSlotUI);
            SelectItemCraft(craftSlotUI);

            foreach (CraftSlotUI craftSlot in _craftSlotList)
            {
                int amountCraft = CheckAmountCraftItems(craftSlot.AssignedItemSlot);
                craftSlot.Init(craftSlot.AssignedItemSlot, amountCraft);
            }
        }
    }

    private InventorySlot SetTypeNewItem(SlotData slotData)
    {
        InventorySlot newSlot;

        if (slotData is BlankSlotData blankData)
            newSlot = new BlankSlot(blankData, 1);

        else if (slotData is RuneSlotData runeData)
            newSlot = new RuneSlot(runeData, runeData.Tier);

        else if (slotData is CatalystSlotData catalystData)
            newSlot = new CatalystSlot(catalystData);

        else if (slotData is EquipItemData equipData)
            newSlot = new EquipSlot(equipData);

        else
            newSlot = new InventorySlot(slotData, 1);

        return newSlot;
    }

    public void CraftItemWrapper()
    {
        if (CanCraftItem(_selectedCraftSlotUI))
        {
            CraftItem(_selectedCraftSlotUI);
        }
    }


    private void RemoveItemOnCraft(CraftSlotUI craftSlotUI)
    {
        CraftItemData data = craftSlotUI.AssignedItemSlot.CraftItemData;

        foreach (CraftRecipe recipe in data.Recipes)
        {
            for (int i = 0; i < recipe.RequiredItems.Count; i++)
            {
                SlotData requiredItem = recipe.RequiredItems[i];
                int requiredAmount = recipe.AmountResources[i];

                _playerInventoryHolder.PrimaryInventorySystem.RemoveItemsFromInventory(requiredItem, requiredAmount);
            }
        }

    }

    public bool CanCraftItem(CraftSlotUI craftSlotUI)
    {
        Dictionary<SlotData, int> inventoryCounts = new Dictionary<SlotData, int>();

        CraftItemData data = craftSlotUI.AssignedItemSlot.CraftItemData;

        foreach (InventorySlot slot in _playerInventoryHolder.PrimaryInventorySystem.InventorySlots)
        {
            if (slot.ItemData != null)
            {
                if (inventoryCounts.ContainsKey(slot.ItemData))
                {
                    inventoryCounts[slot.ItemData] += slot.StackSize;
                }
                else
                {
                    inventoryCounts[slot.ItemData] = slot.StackSize;
                }
            }
        }


        foreach (CraftRecipe recipe in data.Recipes)
        {
            for (int i = 0; i < recipe.RequiredItems.Count; i++)
            {
                SlotData requiredItem = recipe.RequiredItems[i];
                int requiredAmount = recipe.AmountResources[i];
                //ItemCraft requiredPrefab = recipe.CraftPrefab[i];

                if (!inventoryCounts.ContainsKey(requiredItem) || inventoryCounts[requiredItem] < requiredAmount)
                {
                    _craftButton.gameObject.SetActive(false);


                    return false;
                }
            }
        }

        _craftButton.gameObject.SetActive(true);
        return true;
    }

    public bool CanCraftItem(SlotData requiredItem, int requiredAmount)
    {
        Dictionary<SlotData, int> inventoryCounts = new Dictionary<SlotData, int>();

        foreach (InventorySlot slot in _playerInventoryHolder.PrimaryInventorySystem.InventorySlots)
        {
            if (slot.ItemData != null)
            {
                if (inventoryCounts.ContainsKey(slot.ItemData))
                {
                    inventoryCounts[slot.ItemData] += slot.StackSize;
                }
                else
                {
                    inventoryCounts[slot.ItemData] = slot.StackSize;
                }
            }
        }

        if (inventoryCounts.ContainsKey(requiredItem) && inventoryCounts[requiredItem] >= requiredAmount)
        {
            return true;
        }
        return false;
    }

    private void ClearContainer(Transform container)
    {
        if (container != null)
        {
            foreach (Transform child in container)
                Destroy(child.gameObject);
        }
    }

    private void SwitchOnShopWindow()
    {
        this.gameObject.SetActive(false);
        _shopKeeperDisplay.gameObject.SetActive(true);
        _shopKeeperDisplay.DisplayShopWindow(_shopSystem, _playerInventoryHolder, _craftType, _craftSystem);
    }
}
