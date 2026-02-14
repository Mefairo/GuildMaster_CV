using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Equip System/New Item")]
public class EquipItemData : CraftItemData
{
    [Space(5)]
    [Header("EquipMent")]
    public int Tier;
    public int EquipMaxStackSize;
    public EquipType EquipType;
    public List<DebuffList> NeutralizeDebuffs;
    public List<Debuff> NeutralizeDebuffsData;
    public List<AffixList> NeutralizeAffixes;
    public List<BuffList> Buffs;
    public List<HeroStats> Stats;
    public List<ResistanceList> Resistance;
}
