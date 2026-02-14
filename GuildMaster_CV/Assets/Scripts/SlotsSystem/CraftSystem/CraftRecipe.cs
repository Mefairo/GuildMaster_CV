using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CraftRecipe
{
    [SerializeField] private List<SlotData> _requiredItems;
    [SerializeField] private List<int> _amountResources;
    [SerializeField] private List<ItemCraft> _craftPrefab;

    [SerializeField] private SlotData _craftedItem;


    public List<SlotData> RequiredItems => _requiredItems;
    public List<int> AmountResources => _amountResources;
    public List<ItemCraft> CraftPrefab => _craftPrefab;
    public SlotData CraftedItem => _craftedItem;

}
