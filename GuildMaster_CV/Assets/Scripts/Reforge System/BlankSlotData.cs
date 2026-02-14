using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(menuName = "Reforge System/Blank Item")]
public class BlankSlotData : EquipItemData
{
    [Header("Blank Slot Parametres")]
    public List<BlankSlotParametres> BlankParametres = new List<BlankSlotParametres>();
    public List<BlankBaseStatsList> BaseStats;
    public Sprite ReforgeSprite;
    //public HeroStats BaseStats;
}

[System.Serializable]
public class BlankSlotParametres
{
    public int MinPropertyAmount;
    public int MaxPropertyAmount;
    [Space]
    public int MinCraftValue;
    public int MaxCraftValue;
}

[System.Serializable]
public class BlankBaseStatsList
{
    public List<BlankBaseStats> BaseStats;
}


[System.Serializable]
public class BlankBaseStats
{
    public StatType StatType;
    public int MinValueStat;
    public int MaxValueStat;
}
