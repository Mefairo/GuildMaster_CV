using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Craft System/Craft Item")]
public class CraftItemData : SlotData
{
    [Header("Craft Parametres")]
    public List<CraftRecipe> Recipes;
    public ItemSecondaryType ItemSecondaryType;
}
