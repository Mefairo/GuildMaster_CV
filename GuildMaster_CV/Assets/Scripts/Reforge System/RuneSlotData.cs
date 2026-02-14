using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Reforge System/Rune Item")]
public class RuneSlotData : CraftItemData
{
    [Header("Rune Slot Parametres")]
    public int Tier;
    [Space]
    public List<RuneSlotStats> RuneSlotStats = new List<RuneSlotStats>();
    [Space]
    public List<RuneSlotResistance> RuneSlotRes = new List<RuneSlotResistance>();
    [Space]
    public List<Debuff> DebuffLists = new List<Debuff>();
}


[System.Serializable]
public class RuneSlotStats
{
    public StatType StatType;
    public int MinValueStat;
    public int MaxValueStat;
}


[System.Serializable]
public class RuneSlotResistance
{
    public TypeDamage ResType;
    public int MinValueRes;
    public int MaxValueRes;
}