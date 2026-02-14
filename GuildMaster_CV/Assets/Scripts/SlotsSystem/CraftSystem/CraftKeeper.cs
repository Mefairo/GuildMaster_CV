using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using UnityEngine.UI;
//using static UnityEngine.Rendering.DynamicArray<T>;

//Должно быть наследовано от IINTERACTABLE
public class CraftKeeper : MonoBehaviour
{
    [Header("Craft Settings")]
    [SerializeField] private CraftItemList _craftItemsHeld;
    [SerializeField] private Button _buttonSelf;
    [SerializeField] private PlayerInventoryHolder _playerInvHolder;
    [SerializeField] private ShopTypes _craftType;
    [SerializeField] private CraftSystem _craftSystem;
    [SerializeField] private ShopKeeper _shopKeeper;
    [Space]
    [Header("Other Settings")]
    [SerializeField] private int _amountRandomItems;
    [Space]
    [Header("List Items")]
    [SerializeField] private List<CraftItemData> _randomItems;

    public CraftItemList CraftItemsHeld => _craftItemsHeld;
    public CraftSystem CraftSystem => _craftSystem;

    public static UnityAction<CraftSystem, PlayerInventoryHolder, ShopTypes, ShopSystem> OnCraftWindowRequested;
    public static UnityAction  OnCraftWindowRequestedForLearn;
    public static UnityAction OnCraftWindowClosed;

    private void Awake()
    {
        _shopKeeper = GetComponent<ShopKeeper>();

        _craftSystem = new CraftSystem(_craftItemsHeld.Items.Count);

        foreach (var item in _craftItemsHeld.Items)
        {
            _craftSystem.AddToCraft(item);
        }

        SetRandomItems();

        //_buttonSelf.onClick.AddListener(OpenWindowCraft);
    }

    private void SetRandomItems()
    {
        int itemsToAdd = Mathf.Min(_amountRandomItems, _randomItems.Count);

        for (int i = 0; i < itemsToAdd; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, _randomItems.Count);

            _craftSystem.AddToCraft(_randomItems[randomIndex]);
            _randomItems.RemoveAt(randomIndex);
        }
    }

    private void OpenWindowCraft()
    {
        OnCraftWindowRequested?.Invoke(_craftSystem, _playerInvHolder, _craftType, _shopKeeper.ShopSystem);
        OnCraftWindowRequestedForLearn?.Invoke();
    }


    //public UnityAction<IInteractable> OnInteractionComplete { get; set; }


    //public void Interact(Interactor interactor, out bool interactSuccessful)
    //{
    //    var playerInv = interactor.GetComponent<PlayerInventoryHolder>();

    //    if (playerInv != null)
    //    {
    //        OnCraftWindowRequested?.Invoke(_craftSystem, playerInv);
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
    //    OnCraftWindowClosed?.Invoke();
    //}

    //private void OnTriggerExit2D(Collider2D other)
    //{
    //    if (other.CompareTag("Player"))
    //        EndInteraction();
    //}

}
