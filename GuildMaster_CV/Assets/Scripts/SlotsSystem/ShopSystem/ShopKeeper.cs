using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//[RequireComponent(typeof(UniqueID))]

// ƒŒÀ∆ÕŒ ¡€“‹ ”Õ¿—À≈ƒŒ¬¿ÕŒ Œ“ IINTERACTABLE
public class ShopKeeper : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Shop Settings")]
    [SerializeField] private ShopItemList _shopItemsHeld;
    [SerializeField] private float _buyMarkUp;
    [SerializeField] private float _sellMarkUp;
    [SerializeField] private ShopSystem _shopSystem;
    [SerializeField] private CraftKeeper _craftKeeper;
    [SerializeField] private PlayerInventoryHolder _playerInvHolder;
    [SerializeField] private ShopTypes _shopTypes;
    [SerializeField] private bool _fullItems;
    [Space]
    [Header("Shop Parametres")]
    [SerializeField] private List<int> _minAmountMaterialsItems = new List<int>();
    [SerializeField] private List<int> _maxAmountMaterialsItems = new List<int>();
    [SerializeField] private List<int> _minAmountFinishedItems = new List<int>();
    [SerializeField] private List<int> _maxAmountFinishedItems = new List<int>();
    [SerializeField] private float _coefAmountItems;
    [Space]
    [Header("Other Settings")]
    [SerializeField] private GuildValutes _guildValutes;
    [SerializeField] private NextDayController _nextDayController;
    [Header("UI Settings")]
    [SerializeField] private SpriteRenderer _glow;
    [SerializeField] private SpriteRenderer _namePanel;
    [Space]
    [Header("List Items")]
    [SerializeField] private List<ShopInventoryItem> _constantItems;

    public static UnityAction<ShopSystem, PlayerInventoryHolder, ShopTypes, CraftSystem> OnShopWindowRequested;
    public static UnityAction OnShopWindowRequestedForLearn;

    private ShopSaveData _shopSaveData;

    public ShopTypes ShopTypes => _shopTypes;
    public float SellMarkUp => _sellMarkUp;
    public float BuyMarkUp => _buyMarkUp;
    public ShopSystem ShopSystem => _shopSystem;
    public List<int> MinAmountMaterialsItems => _minAmountMaterialsItems;
    public List<int> MaxAmountMaterialsItems => _maxAmountMaterialsItems;
    public List<int> MinAmountFinishedItems => _minAmountFinishedItems;
    public List<int> MaxAmountFinishedItems => _maxAmountFinishedItems;

    private void Awake()
    {
        _craftKeeper = GetComponent<CraftKeeper>(); 
        //_id = GetComponent<UniqueID>().ID;
        //_shopSaveData = new ShopSaveData(_shopSystem);
    }

    private void Start()
    {
        StartShop();

        _glow.gameObject.SetActive(false);
        _namePanel.gameObject.SetActive(false);
        //if (!SaveGameManager.data._shopKeeperDictionary.ContainsKey(_id))
        //    SaveGameManager.data._shopKeeperDictionary.Add(_id, _shopSaveData);
    }

    private void OnEnable()
    {
        _nextDayController.OnNextWeek += StartShop;

        //SaveLoad.OnLoadGame += LoadInventory;
        //_roundManager.OnNewRoundStart += SetRandomItems;
    }

    private void OnDisable()
    {
        _nextDayController.OnNextWeek -= StartShop;

        //SaveLoad.OnLoadGame -= LoadInventory;
        //_roundManager.OnNewRoundStart -= SetRandomItems;
    }

    private void StartShop()
    {
        _shopSystem.ShopInventory.Clear();

        switch (_guildValutes.Level)
        {
            case 1:
                SetRandomItems(_shopItemsHeld.Items_1, _shopItemsHeld.FinishedItems_1);
                break;

            case 2:
                SetRandomItems(_shopItemsHeld.Items_2, _shopItemsHeld.FinishedItems_2);
                break;

            case 3:
                SetRandomItems(_shopItemsHeld.Items_3, _shopItemsHeld.FinishedItems_3);
                break;

            case 4:
                SetRandomItems(_shopItemsHeld.Items_4, _shopItemsHeld.FinishedItems_4);
                break;
        }


    }

    private void SetRandomItems(List<ShopInventoryItem> materialItems, List<ShopInventoryItem> finishedItems)
    {
        List<ShopInventoryItem> items = new List<ShopInventoryItem>();

        if (_fullItems)
        {
            foreach (ShopInventoryItem item in materialItems)
                items.Add(item);

            foreach (ShopInventoryItem item in finishedItems)
                items.Add(item);

            foreach (ShopInventoryItem item in _constantItems)
                items.Add(item);

            foreach (ShopInventoryItem item in items)
            {
                int randomAmount = SetRandomValue(item.MinAmount, item.MaxAmount + 1);
                int randomItemAmount = (int)(randomAmount * _coefAmountItems);

                _shopSystem.AddToShop(item.ItemData, randomItemAmount);
            }
        }

        else
        {
            int amountMaterialItems = SetRandomValue(_minAmountMaterialsItems[_guildValutes.Level - 1], _maxAmountMaterialsItems[_guildValutes.Level - 1] + 1);
            int amountFinishedItems = SetRandomValue(_minAmountFinishedItems[_guildValutes.Level - 1], _maxAmountFinishedItems[_guildValutes.Level - 1] + 1);

            _shopSystem = new ShopSystem(amountMaterialItems + amountFinishedItems + _constantItems.Count, _shopItemsHeld.MaxAllowedGold,
                _buyMarkUp, _sellMarkUp);

            SetItems(materialItems, items, amountMaterialItems);
            SetItems(finishedItems, items, amountFinishedItems);

            foreach (ShopInventoryItem item in _constantItems)
            {
                items.Add(item);
            }

            foreach (ShopInventoryItem item in items)
            {
                int randomAmount = SetRandomValue(item.MinAmount, item.MaxAmount + 1);
                int randomItemAmount = (int)(randomAmount * _coefAmountItems);

                _shopSystem.AddToShop(item.ItemData, randomItemAmount);
            }
        }
    }

    private int SetRandomValue(int minValue, int maxValue)
    {
        int randomValue = UnityEngine.Random.Range(minValue, maxValue);
        return randomValue;
    }

    private void SetItems(List<ShopInventoryItem> itemsToAdd, List<ShopInventoryItem> items, int amountIterations)
    {
        while (amountIterations > 0)
        {
            int randomIndex = SetRandomValue(0, itemsToAdd.Count);

            if (!items.Contains(itemsToAdd[randomIndex]))
            {
                items.Add(itemsToAdd[randomIndex]);
                amountIterations--;
            }
        }
    }

    public void ChangeCoefAmountItems(float value)
    {
        _coefAmountItems += value;

        _shopSystem.SplitStackSize(_coefAmountItems);
    }

    //private void LoadInventory(SaveData data)
    //{
    //    if (data._shopKeeperDictionary.TryGetValue(_id, out ShopSaveData shopSaveData))
    //    {
    //        _shopSaveData = shopSaveData;
    //        _shopSystem = _shopSaveData.ShopSystem;
    //    }
    //    else
    //    {
    //        _shopSaveData = new ShopSaveData(_shopSystem);
    //        SaveGameManager.data._shopKeeperDictionary.Add(_id, _shopSaveData);
    //    }
    //}

    private void OpenShop()
    {
        //var playerInv = GetComponent<PlayerInventoryHolder>();
        //inventoryHolder = GetComponent<PlayerInventoryHolder>();

        if (!_nextDayController.InTransition)
        {
            if (_playerInvHolder != null)
            {
                OnShopWindowRequested?.Invoke(_shopSystem, _playerInvHolder, _shopTypes, _craftKeeper.CraftSystem);
                OnShopWindowRequestedForLearn?.Invoke();
            }
            else
            {
                Debug.LogError("Player inventory not found");
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _glow.gameObject.SetActive(true);
        _namePanel.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _glow.gameObject.SetActive(false);
        _namePanel.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OpenShop();
    }

    //public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    //public void Interact(Interactor interactor, out bool interactSuccessful)
    //{
    //    var playerInv = interactor.GetComponent<PlayerInventoryHolder>();

    //    if (playerInv != null)
    //    {
    //        OnShopWindowRequested?.Invoke(_shopSystem, playerInv);
    //        interactSuccessful = true;
    //    }
    //    else
    //    {
    //        interactSuccessful = false;
    //        Debug.LogError("Player inventory not found");
    //    }
    //}

    //public void EndInteraction()
    //{
    //    OnShopWindowClosed?.Invoke();
    //}

    //private void OnTriggerExit2D(Collider2D other)
    //{
    //    if (other.CompareTag("Player"))
    //        EndInteraction();
    //}
}

[System.Serializable]
public class ShopSaveData
{
    public ShopSystem ShopSystem;

    public ShopSaveData(ShopSystem shopSystem)
    {
        ShopSystem = shopSystem;
    }
}
