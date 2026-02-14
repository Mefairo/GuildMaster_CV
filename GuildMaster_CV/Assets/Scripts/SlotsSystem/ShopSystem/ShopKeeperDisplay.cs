using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using UnityEngine.Events;

public class ShopKeeperDisplay : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private ShopSlotUI _shopSlotPrefab;
    [SerializeField] private ShoppingCartItemUI _shoppingCartItemPrefab;
    [SerializeField] private ResImage_UI _prefabImage;
    [SerializeField] private List<ShopSlotUI> _slots = new List<ShopSlotUI>();
    [SerializeField] private List<ShoppingCartItemUI> _cartSlots = new List<ShoppingCartItemUI>();
    [Space]
    [Header("Shop Tabs")]
    [SerializeField] private Button _buyTab;
    [SerializeField] private Button _sellTab;
    [Space]
    [Header("Shopping Cart")]
    [SerializeField] private TextMeshProUGUI _basketTotalText;
    [SerializeField] private TextMeshProUGUI _playerGoldText;
    [SerializeField] private TextMeshProUGUI _shopGoldText;
    [SerializeField] private Button _buyButton;
    [SerializeField] private TextMeshProUGUI _buyButtonText;
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
    [SerializeField] private GameObject _shoppingCartContentPanel;
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
    [SerializeField] private GuildValutes _guildValutes;
    [SerializeField] private CraftKeeperDisplay _craftKeeperDisplay;
    [SerializeField] protected DynamicInventoryDisplay _backPackInventory;
    [SerializeField] protected ReforgeController _reforgeController;
    [SerializeField] private Transform _resContainer;
    [SerializeField] private Transform _debuffContainer;
    [SerializeField] private Button _craftSwithButton;
    [SerializeField] private Button _reforgeWIndowButton;

    [SerializeField] private int _basketTotal;
    private bool _isSelling;

    private ShopSystem _shopSystem;
    private CraftSystem _craftSystem;
    private PlayerInventoryHolder _playerInventoryHolder;
    private ShopTypes _shopType;
    private CraftItemData _selectItemData;
    private InventorySlot _selectInvSlot;
    private StatDisplayManager _statDisplayManager = new StatDisplayManager();
    private QuestResistanceSystem _questResistanceSystem;

    private Dictionary<SlotData, int> _shoppingCart = new Dictionary<SlotData, int>();
    private Dictionary<SlotData, ShoppingCartItemUI> _shoppingCartUI = new Dictionary<SlotData, ShoppingCartItemUI>();

    public static UnityAction OnShopWindowClosed;
    public static UnityAction OnCraftWindowOpen;

    private void Awake()
    {
        _questResistanceSystem = GetComponent<QuestResistanceSystem>();
        _craftSwithButton.onClick.AddListener(SwitchOnCraftWindow);
    }

    private void Start()
    {
        _reforgeWIndowButton?.onClick.AddListener(OpenReforegeWindow);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!LearningGameManager.Instance.Panel.gameObject.activeInHierarchy)
                this.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        _questResistanceSystem.ClearIcons();
    }

    private void OnDisable()
    {
        _selectItemData = null;
    }

    public void DisplayShopWindow(ShopSystem shopSystem, PlayerInventoryHolder playerInventoryHolder, ShopTypes shopType, CraftSystem craftSystem)
    {
        _shopSystem = shopSystem;
        _craftSystem = craftSystem;
        _playerInventoryHolder = playerInventoryHolder;
        _shopType = shopType;

        RefreshDisplay();
        DisplayShopInventory();
    }

    private void RefreshDisplay()
    {
        if (_buyButton != null)
        {
            _buyButtonText.text = _isSelling ? "Sell Items" : "Buy Items";
            _buyButton.onClick.RemoveAllListeners();
            if (_isSelling)
                _buyButton.onClick.AddListener(SellItems);
            else
                _buyButton.onClick.AddListener(BuyItems);
        }

        ClearSlots();
        ClearItemPreview();

        _basketTotalText.enabled = false;
        _buyButton.gameObject.SetActive(false);
        _basketTotal = 0;
        _playerGoldText.text = $"Player Gold: {_guildValutes.Gold}";
        //_shopGoldText.text = $"Shop Gold: {_shopSystem.AvailableGold}";
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

    private void BuyItems()
    {
        if (_guildValutes.Gold < _basketTotal)
            return;

        if (!_playerInventoryHolder.PrimaryInventorySystem.CheckInventoryRemaining(_shoppingCart))
            return;

        foreach (var kvp in _shoppingCart)
        {
            _shopSystem.PurchaseItem(kvp.Key, kvp.Value);
            var newSlot = new InventorySlot(kvp.Key, kvp.Value);
            var newSlotToAdd = SetTypeNewItem(kvp.Key);

            for (int i = 0; i < kvp.Value; i++)
            {
                _playerInventoryHolder.PrimaryInventorySystem.AddToInventory(newSlotToAdd, 1);
                //_playerInventoryHolder.PrimaryInventorySystem.AddToInventory(kvp.Key, 1);
            }
        }

        _shopSystem.GainGold(_basketTotal);
        _guildValutes.ChangeGold(-_basketTotal);
        //_playerInventoryHolder.PrimaryInventorySystem.SpendGold(_basketTotal);

        RefreshDisplay();
        DisplayShopInventory();

        _backPackInventory.RefreshDynamicInventory(_playerInventoryHolder.PrimaryInventorySystem, 0);
    }

    private void SellItems()
    {
        //if (_shopSystem.AvailableGold < _basketTotal)
        //    return;

        foreach (var kvp in _shoppingCart)
        {
            var price = GetModifiedPrice(kvp.Key, kvp.Value, _shopSystem.SellMarkUp);

            _shopSystem.SellItem(kvp.Key, kvp.Value, price);

            //_guildValutes.ChangeGold(_basketTotal);
            //_shopSystem.GainGold(-_basketTotal);
            _playerInventoryHolder.PrimaryInventorySystem.RemoveItemsFromInventory(kvp.Key, kvp.Value);
        }
        _guildValutes.ChangeGold(_basketTotal);

        RefreshDisplay();
        DisplayShopInventory();

        _backPackInventory.RefreshDynamicInventory(_playerInventoryHolder.PrimaryInventorySystem, 0);
    }

    private void ClearSlots()
    {
        _shoppingCart = new Dictionary<SlotData, int>();
        _shoppingCartUI = new Dictionary<SlotData, ShoppingCartItemUI>();

        foreach (Transform item in _itemListContentPanel.transform.Cast<Transform>())
        {
            Destroy(item.gameObject);
        }

        foreach (Transform item in _shoppingCartContentPanel.transform.Cast<Transform>())
        {
            Destroy(item.gameObject);
        }

        _slots.Clear();
        _cartSlots.Clear();
    }

    private void CreatePlayerItemSlot(KeyValuePair<SlotData, int> item)
    {
        ShopSlot tempSlot = new ShopSlot();
        tempSlot.AssignItem(item.Key, item.Value);

        ShopSlotUI shopSlot = Instantiate(_shopSlotPrefab, _itemListContentPanel.transform);
        shopSlot.Init(tempSlot, _shopSystem.SellMarkUp);
    }

    //private void CreatePlayerItemSlot(KeyValuePair<InventorySlot, int> item)
    //{
    //    ShopSlot tempSlot = new ShopSlot(item.Key);
    //    tempSlot.AssignItem(item.Key);

    //    ShopSlotUI shopSlot = Instantiate(_shopSlotPrefab, _itemListContentPanel.transform);

    //    if (tempSlot.AssignedInvSlot == null)
    //        Debug.Log("inv null 001");
    //    if (tempSlot.AssignedInvSlot.BaseItemData == null)
    //        Debug.Log("inv null 002");
    //    shopSlot.Init(tempSlot, _shopSystem.SellMarkUp);
    //}

    private void CreateShopItemSlot(ShopSlot item)
    {
        ShopSlotUI shopSlot = Instantiate(_shopSlotPrefab, _itemListContentPanel.transform);
        shopSlot.Init(item, _shopSystem.BuyMarkUp);

        _slots.Add(shopSlot);
    }

    public void TabClicked(CheckTypeForTabs tab)
    {
        RefreshDisplay();

        if (_isSelling)
        {
            foreach (KeyValuePair<SlotData, int> item in _playerInventoryHolder.PrimaryInventorySystem.GetAllItemHeld())
            {
                if (tab.ItemType == ItemType.All)
                    CreatePlayerItemSlot(item);

                else
                {
                    if (item.Key.ItemType == tab.ItemType)
                        CreatePlayerItemSlot(item);

                    else
                        continue;
                }
            }
        }

        else
        {
            foreach (ShopSlot item in _shopSystem.ShopInventory)
            {
                if (item.ItemData == null)
                    continue;

                if (tab.ItemType == ItemType.All)
                    CreateShopItemSlot(item);

                else
                {
                    if (item.ItemData.ItemType == tab.ItemType)
                        CreateShopItemSlot(item);

                    else
                        continue;
                }
            }
        }
    }

    private void DisplayShopInventory()
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

        switch (_shopType)
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

        if (_isSelling)
        {
            foreach (var item in _playerInventoryHolder.PrimaryInventorySystem.GetAllItemHeld())
            {
                Debug.Log("display shop");
                CreatePlayerItemSlot(item);
            }
        }

        else
        {
            foreach (ShopSlot item in _shopSystem.ShopInventory)
            {
                if (item.ItemData == null)
                    continue;

                CreateShopItemSlot(item);
            }
        }

    }

    public void RemoveItemFromCart(ShopSlotUI shopSlotUI)
    {
        SlotData data = shopSlotUI.AssignedItemSlot.ItemData;
        int price = GetModifiedPrice(data, 1, shopSlotUI.MarkUp);

        if (_shoppingCart.ContainsKey(data))
        {
            _shoppingCart[data]--;

            string newAmount = $"x{_shoppingCart[data]}";
            ShoppingCartItemUI shoppongCartSlot = _shoppingCartUI[data];
            Sprite backgroundImage = data.IconBackground;/////////////////////////////

            //shoppongCartSlot.SetItemText(data, newAmount);
            shoppongCartSlot.SetItemText(data, newAmount);

            if (_shoppingCart[data] <= 0)
            {
                _shoppingCart.Remove(data);
                GameObject tempObj = _shoppingCartUI[data].gameObject;
                _shoppingCartUI.Remove(data);
                Destroy(tempObj);
            }
        }

        _basketTotal -= price;
        _basketTotalText.text = $"Total: {_basketTotal}";

        if (_basketTotal <= 0 && _basketTotalText.IsActive())
        {
            _basketTotalText.enabled = false;
            _buyButton.gameObject.SetActive(false);
            ClearItemPreview();
            return;
        }

        CheckCartVsAvailableGold();
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

        ClearContainer(_resContainer);
        ClearContainer(_debuffContainer);
    }

    public void AddItemToCart(ShopSlotUI shopSlotUI)
    {
        SlotData data = shopSlotUI.AssignedItemSlot.ItemData;
        ShoppingCartItemUI shoppongCartItem = _shoppingCartItemPrefab;

        if (shopSlotUI.AssignedItemSlot.AssignedInvSlot == null)
            UpdateItemPreview(data);

        else
            UpdateItemPreview(shopSlotUI.AssignedItemSlot.ItemData);

        int price = GetModifiedPrice(data, 1, shopSlotUI.MarkUp);

        if (_shoppingCart.ContainsKey(data))
        {
            _shoppingCart[data]++;

            string newAmount = $"x{_shoppingCart[data]}";
            ShoppingCartItemUI shoppongCartSlot = _shoppingCartUI[data];

            shoppongCartSlot.SetItemText(data, newAmount);
        }

        else
        {
            _shoppingCart.Add(data, 1);

            ShoppingCartItemUI shoppingCartTextObj = Instantiate(shoppongCartItem, _shoppingCartContentPanel.transform);

            string newAmount = $"x1";

            shoppingCartTextObj.SetItemText(data, newAmount);

            _shoppingCartUI.Add(data, shoppingCartTextObj);
        }

        _basketTotal += price;
        _basketTotalText.text = $"Total: {_basketTotal}";

        if (_basketTotal > 0 && !_basketTotalText.IsActive())
        {
            _basketTotalText.enabled = true;
            _buyButton.gameObject.SetActive(true);
        }

        CheckCartVsAvailableGold();
    }


    private void CheckCartVsAvailableGold()
    {
        // Проверяем, хватает ли золота у игрока/гильдии
        bool hasEnoughGold = _basketTotal <= _guildValutes.Gold;
        _basketTotalText.color = hasEnoughGold ? Color.gray : Color.red;

        // Если это не продажа и в инвентаре нет места - показываем ошибку
        if (!_isSelling && !_playerInventoryHolder.PrimaryInventorySystem.CheckInventoryRemaining(_shoppingCart))
        {
            _basketTotalText.text = "Not enough room in inventory";
            _basketTotalText.color = Color.red;
        }
        else if (!hasEnoughGold && !_isSelling)
        {
            _basketTotalText.color = Color.red;
            _basketTotalText.text = $"Total: {_basketTotal}"; // Опциональное сообщение
        }

        else if (!hasEnoughGold && _isSelling)
        {
            _basketTotalText.color = Color.green;
            _basketTotalText.text = $"Total: {_basketTotal}";
        }
    }

    public static int GetModifiedPrice(SlotData data, int amount, float markUp)
    {
        float baseValue = data.GoldValue * amount;

        return Mathf.FloorToInt(baseValue * markUp);
        //return Mathf.FloorToInt(baseValue + baseValue * markUp);
    }

    public void UpdateItemPreview(SlotData itemData)
    {
        ClearItemPreview();

        _selectItemData = (CraftItemData)itemData;

        _itemPreviewDescription.text = itemData.Description;

        if (itemData is RuneSlotData runeData)
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

        else if (itemData is EquipItemData equiptemData)
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

        if (itemData is DrawingItemData drawingItemData)
            _itemSecondaryType.text = "Recipe";

        UpdateResistance();
        UpdateDebuffs();
    }

    public void UpdateBlankItemPreview(BlankSlot blankSlot)
    {
        ClearItemPreview();

        _itemPreviewDescription.text = blankSlot.BaseItemData.Description;

        foreach (HeroStats stat in blankSlot.ItemStats)
        {
            if (stat != null)
            {
                (string, string, string, string) StatsText = _statDisplayManager.SetStatsValueText(stat, false);

                _powerStatsNameText.text = StatsText.Item1;
                _powerStatsValueText.text = StatsText.Item2;

                _defenceStatsNameText.text = StatsText.Item3;
                _defenceStatsValueText.text = StatsText.Item4;

                _itemPrimaryType.text = blankSlot.BaseItemData.ItemPrimaryType.ToString();
                _itemSecondaryType.text = blankSlot.BlankSlotData.EquipType.ToString();

                if (blankSlot.BlankSlotData.EquipType == EquipType.Trinket)
                {
                    _itemSecondaryType.text = blankSlot.BlankSlotData.ItemSecondaryType.ToString();
                }
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

    private void ClearContainer(Transform container)
    {
        if (container != null)
        {
            foreach (Transform child in container)
                Destroy(child.gameObject);
        }
    }

    private void CloseWindow()
    {
        OnShopWindowClosed?.Invoke();
    }

    public void OnBuyTabPressed()
    {
        _isSelling = false;
        RefreshDisplay();
        DisplayShopInventory();
    }

    public void OnSellTabPressed()
    {
        _isSelling = true;
        RefreshDisplay();
        //DisplayPlayerInventory();
        DisplayShopInventory();
    }

    private void SwitchOnCraftWindow()
    {
        this.gameObject.SetActive(false);
        _craftKeeperDisplay.gameObject.SetActive(true);
        OnCraftWindowOpen?.Invoke();
        _craftKeeperDisplay.DisplayCraftWindow(_craftSystem, _playerInventoryHolder, _shopType, _shopSystem);
    }

    private void OpenReforegeWindow()
    {
        if (!_reforgeController.ReforgePanel.gameObject.activeSelf)
            _reforgeController.ReforgeWindowOpen();
    }
}
