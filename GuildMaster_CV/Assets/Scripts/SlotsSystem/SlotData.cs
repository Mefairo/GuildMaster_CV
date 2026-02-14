using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Slot System/Slot")]
public class SlotData : ScriptableObject
{
    [Header("Display Parametres")]
    public int ID = -1;
    public string DisplayName;
    [TextArea(4, 4)]
    public string Description;
    [TextArea(4, 4)]
    public string StatsDescription;
    public Sprite Icon;
    public Sprite IconBackground;
    public GameObject ItemPrefab;

    [Header("Game Parametres")]
    public int MaxStackSize;
    public float GoldValue;
    [Header("Type")]
    public ItemType ItemType;
    public ItemPrimaryType ItemPrimaryType;

}
