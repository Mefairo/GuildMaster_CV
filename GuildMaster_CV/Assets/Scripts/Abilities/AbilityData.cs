using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability System/Ability  Data")]
public class AbilityData : ScriptableObject
{
    public int ID = -1;
    public Sprite Icon;
    public string Name;
    [TextArea(4, 4)]
    public string Description;
    [TextArea(4, 4)]
    public string AbilityTreeDescription;
    public int Tier;
    public float PowerValue;
    public float DefenceValue;
    public float HealValue;
    public float ManaCost;
    public float СoefBuff = 20; // коэффициент повышения силы за каждый применненный бафф к врагу
    public float СoefAoe = 20; // коэффициент снижения силы за каждое требуемое аое 
    public bool UniqBuff;
    public bool PostBuff; // для расчета эффективности задания. До или после основного расчета параметров героев
    public bool MasterAura = false;
    public int AmountSummon = 1;
    public List<HeroStats> Stats;
    public List<Resistance> ResistancesForAttack;
    public List<Resistance> ResistancesForDefence;
    public List<Debuff> DebuffsForAttack;
    public List<Debuff> DebuffsForDefence;
    public List<Resistance> ResistancesForDecrease;
    public GeneralTypeAbility GeneralType;
    public TypeAbilities TypeAbility;
    public List<BuffList> BuffList;
    public List<BuffList> NeutralizeBuffList;
    public List<AffixList> NeutralizeAffixList;
    [Header("Study Abilities")]
    public AbilityData MainTreeAbility;
    public HeroStats RequiredStats;
    public List<Ability> StudyAbilities;
}
