using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Shop System/Shop Item List")]
public class ShopItemList : ScriptableObject
{
    [Header("Ingredients and Materials Items")]
    [SerializeField] private List<ShopInventoryItem> _items_1;
    [SerializeField] private List<ShopInventoryItem> _items_2;
    [SerializeField] private List<ShopInventoryItem> _items_3;
    [SerializeField] private List<ShopInventoryItem> _items_4;
    [Space]
    [Header("Finished Items")]
    [SerializeField] private List<ShopInventoryItem> _finishedItems_1;
    [SerializeField] private List<ShopInventoryItem> _finishedItems_2;
    [SerializeField] private List<ShopInventoryItem> _finishedItems_3;
    [SerializeField] private List<ShopInventoryItem> _finishedItems_4;
    [Space]
    [Header("Shop Parametres")]
    [SerializeField] private int _maxAllowedGold;
    [SerializeField] private List<ShopInventoryItem> _randomItems;

    public List<ShopInventoryItem> Items_1 => _items_1;
    public List<ShopInventoryItem> Items_2 => _items_2;
    public List<ShopInventoryItem> Items_3 => _items_3;
    public List<ShopInventoryItem> Items_4 => _items_4;
    public List<ShopInventoryItem> FinishedItems_1 => _finishedItems_1;
    public List<ShopInventoryItem> FinishedItems_2 => _finishedItems_2;
    public List<ShopInventoryItem> FinishedItems_3 => _finishedItems_3;
    public List<ShopInventoryItem> FinishedItems_4 => _finishedItems_4;
    public int MaxAllowedGold => _maxAllowedGold;
    public List<ShopInventoryItem> RandomItems => _randomItems;
}

[System.Serializable]
public struct ShopInventoryItem
{
    public SlotData ItemData;
    public int MinAmount;
    public int MaxAmount;
    public int Tier;
    //public ShopSlot Slot;
}
