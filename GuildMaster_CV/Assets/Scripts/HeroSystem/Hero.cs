using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Hero
{
    [Header("Main Values")]
    [SerializeField] private HeroData _heroData;
    [SerializeField] private string _heroName;
    [SerializeField] private int _heroLevel;
    [SerializeField] private float _powerPoints;
    [SerializeField] private float _defencePoints;
    [SerializeField] private HeroStats _levelUpHeroStats;
    [SerializeField] private HeroStats _visibleHeroStats;
    [SerializeField] private ResistanceSystem _resistanceSystem;
    [SerializeField] private BuffSystem _buffSystem;
    [SerializeField] private List<Resistance> _resistance;
    [SerializeField] private HeroEquipmentHolder _equipHolder;
    [SerializeField] private HeroAbilityHolder _abilityHolder;
    [SerializeField] private List<Ability> _listAbilities;
    [SerializeField] private int _currentExp;
    [SerializeField] private int _requiredExp;
    [SerializeField] private int _dailyPay;
    [SerializeField] private bool _isRested = true;
    [SerializeField] private bool _inSlot = false;
    [Space]
    [Header("Start Values")]
    [SerializeField] private HeroPowerPoints _heroPowerPoints;
    [SerializeField] private HeroDefencePoints _heroDefencePoints;
    [SerializeField] private LevelSystem _levelSystem;
    [SerializeField] private ResistanceList _resistanceList;
    [Space]
    [Header("Buffs")]
    [SerializeField] private List<Ability> _activeBuffs;
    [SerializeField] private List<Ability> _activeUniqueBuffs;
    [SerializeField] private List<Ability> _activeAuras;
    [Space]
    [Header("Other Values")]
    [SerializeField] private List<DebuffList> _neutralizeDebuffs;
    [SerializeField] private List<AffixList> _neutralizeAffixes;

    public HeroData HeroData => _heroData;
    public string HeroName => _heroName;
    public int HeroLevel => _heroLevel;
    public float PowerPoints => _powerPoints;
    public float DefencePoints => _defencePoints;
    public int CurrentExp => _currentExp;
    public int RequiredExp => _requiredExp;
    public HeroStats LevelUpHeroStats => _levelUpHeroStats;
    public HeroStats VisibleHeroStats => _visibleHeroStats;
    public ResistanceSystem ResistanceSystem => _resistanceSystem;
    public BuffSystem BuffSystem => _buffSystem;
    public List<Resistance> Resistances => _resistance;
    public HeroEquipmentHolder EquipHolder => _equipHolder;
    public HeroAbilityHolder AbilityHolder => _abilityHolder;
    public int DailyPay => _dailyPay;
    public bool IsRested => _isRested;
    public bool InSlot => _inSlot;

    public List<Ability> ActiveBuffs => _activeBuffs;
    public List<Ability> ActiveUniqueBuffs => _activeUniqueBuffs;
    public List<Ability> ActiveAuras => _activeAuras;

    public HeroPowerPoints HeroPowerPoints => _heroPowerPoints;
    public HeroDefencePoints HeroDefencePoints => _heroDefencePoints;
    public LevelSystem LevelSystem => _levelSystem;
    public ResistanceList ResistanceList => _resistanceList;
    public List<Ability> ListAbilities => _listAbilities;
    public List<DebuffList> NeutralizeDebuffs => _neutralizeDebuffs;
    public List<AffixList> NeutralizeAffixes => _neutralizeAffixes;

    public WoundsType WoundType;
    public bool IsSentOnQuest = false;
    public bool IsHealing = false;
    public int WeekTracking;
    public int WeeklyPay;

    private NameGenerator _nameGenerator = new NameGenerator();

    public event UnityAction OnChangePowerPoints;
    public event UnityAction OnChangeDefencePoints;
    public event UnityAction OnChangeResistance;
    public event UnityAction OnChangeEquipped;

    public Hero(Hero copyHero)
    {
        _heroData = copyHero.HeroData;
        _heroName = copyHero.HeroName;

        _levelSystem = new LevelSystem(copyHero.LevelSystem.Level, copyHero.LevelSystem.CurrentExp, copyHero.LevelSystem.RequiredExp);
        _heroLevel = copyHero.LevelSystem.Level;

        _visibleHeroStats = DeepCopyStats(copyHero.VisibleHeroStats);

        _heroPowerPoints = new HeroPowerPoints(copyHero.HeroPowerPoints.AllPower, copyHero.HeroPowerPoints.AllCoefPower);
        _heroDefencePoints = new HeroDefencePoints(copyHero.HeroDefencePoints.AllDefence, copyHero.HeroDefencePoints.AllCoefDefence);
        _resistanceSystem = new ResistanceSystem(copyHero.ResistanceSystem.AllRes);
        _buffSystem = new BuffSystem(copyHero.BuffSystem.AllBuffs, copyHero.BuffSystem.AllNeutralizeDebuffs, copyHero.BuffSystem.AllNeutralizeAffix);

        _equipHolder = new HeroEquipmentHolder(copyHero.EquipHolder.EquipSystem);
        _abilityHolder = new HeroAbilityHolder(copyHero.AbilityHolder.HeroAbilitySystem);
        _listAbilities = DeepCopyAbilities(copyHero.ListAbilities);

        _dailyPay = copyHero.DailyPay;
    }

    public Hero() { }

    public void AssignHero(HeroData heroData, int level, QuestParametresSystemFix questParametresSystemFix)
    {
        _heroData = heroData;
        _heroName = _nameGenerator.GetRandomName();
        _heroLevel = level;

        _levelUpHeroStats = new HeroStats();
        _visibleHeroStats = DeepCopyStats(heroData.VisibleHeroStats);

        _equipHolder = new HeroEquipmentHolder();
        _abilityHolder = new HeroAbilityHolder();
        _heroPowerPoints = new HeroPowerPoints();
        _heroDefencePoints = new HeroDefencePoints();
        _neutralizeDebuffs = new List<DebuffList>();
        _neutralizeAffixes = new List<AffixList>();
        WoundType = WoundsType.Healthy;

        _buffSystem = new BuffSystem();

        _activeBuffs = new List<Ability>();
        _activeUniqueBuffs = new List<Ability>();
        _activeAuras = new List<Ability>();

        _resistanceSystem = new ResistanceSystem();
        _resistance = ResistanceSystem.CalculateBaseRes(this);

        _listAbilities = DeepCopyAbilities(heroData.Abilities);

        _powerPoints = _heroPowerPoints.SumPower(this, questParametresSystemFix);
        _defencePoints = _heroDefencePoints.SumDefence(this, questParametresSystemFix);

        _levelSystem = new LevelSystem(level);
        CheckedDailyPay();

        _levelSystem.OnResTalentTaken += ChangeResistanceForLevelByTalent;
    }

    public bool IsEmpty()
    {
        return this == null;
    }

    private List<Resistance> DeepCopyResistances(List<Resistance> original)
    {
        List<Resistance> copy = new List<Resistance>();
        foreach (Resistance resistance in original)
        {
            Resistance newResistance = new Resistance(resistance);
            copy.Add(newResistance);
        }
        return copy;
    }

    public void UpdateResistance(TypeDamage type, int value)
    {
        Resistance currentRes = _resistance.Find(r => r.TypeDamage == type);
        if (currentRes != null)
        {
            currentRes.ValueResistance += value;
        }
        else
        {
            // Если сопротивление не найдено, добавляем его в список
            _resistance.Add(new Resistance { TypeDamage = type, ValueResistance = value });
        }

        OnChangeResistance?.Invoke();
    }

    public void UpdateResistance1(TypeDamage type, int value)
    {
        Resistance currentRes = _resistance.Find(r => r.TypeDamage == type);
        if (currentRes != null)
        {
            currentRes.ValueResistance += value;
        }
        else
        {
            // Если сопротивление не найдено, добавляем его в список
            _resistance.Add(new Resistance { TypeDamage = type, ValueResistance = value });
        }

        //OnChangeResistance?.Invoke();
    }

    private List<Ability> DeepCopyAbilities(List<Ability> original)
    {
        List<Ability> copy = new List<Ability>();
        foreach (Ability ability in original)
        {
            Ability newAbility = new Ability(ability);
            copy.Add(newAbility);
        }
        return copy;
    }

    private HeroStats DeepCopyStats(HeroStats original)
    {
        return new HeroStats
        {
            StrengthMulti = original.StrengthMulti,
            DexterityMulti = original.DexterityMulti,
            IntelligenceMulti = original.IntelligenceMulti,
            EnduranceMulti = original.EnduranceMulti,
            FlexibilityMulti = original.FlexibilityMulti,
            SanityMulti = original.SanityMulti,

            CritMulti = original.CritMulti,
            HasteMulti = original.HasteMulti,
            AccuracyMulti = original.AccuracyMulti,
            DurabilityMulti = original.DurabilityMulti,
            AdaptabilityMulti = original.AdaptabilityMulti,
            SuppressionMulti = original.SuppressionMulti,

            StrengthValue = original.StrengthValue,
            DexterityValue = original.DexterityValue,
            IntelligenceValue = original.IntelligenceValue,
            EnduranceValue = original.EnduranceValue,
            FlexibilityValue = original.FlexibilityValue,
            SanityValue = original.SanityValue,

            CritValue = original.CritValue,
            HasteValue = original.HasteValue,
            AccuracyValue = original.AccuracyValue,
            DurabilityValue = original.DurabilityValue,
            AdaptabilityValue = original.AdaptabilityValue,
            SuppressionValue = original.SuppressionValue,

            Health = original.Health
        };
    }

    public void ChangedEquip()
    {
        OnChangeEquipped?.Invoke();
    }

    public (int, bool) ChangeExp(int exp)
    {
        // Декомпозируем возвращаемый кортеж на уровень и флаг повышения уровня
        (int newLevel, bool isLevelUp) = _levelSystem.ChangeExperience(exp);

        // Присваиваем новый уровень герою
        _heroLevel = newLevel;
        CheckedDailyPay();

        return (newLevel, isLevelUp);
    }

    public void ChangeRested(bool rest)
    {
        _isRested = rest;
    }

    public void ChangeSlotCondition(bool inSlot)
    {
        _inSlot = inSlot;
    }

    private void ChangeResistanceForLevelByTalent(int resValue)
    {
        foreach (Resistance res in this.Resistances)
        {
            if (res.TypeDamage == TypeDamage.Physical)
                res.ValueResistance += resValue;

            if (res.TypeDamage == TypeDamage.Fire)
                res.ValueResistance += resValue;

            if (res.TypeDamage == TypeDamage.Cold)
                res.ValueResistance += resValue;

            if (res.TypeDamage == TypeDamage.Lightning)
                res.ValueResistance += resValue;

            if (res.TypeDamage == TypeDamage.Dark)
                res.ValueResistance += resValue;

            if (res.TypeDamage == TypeDamage.Light)
                res.ValueResistance += resValue;

            if (res.TypeDamage == TypeDamage.Necrotic)
                res.ValueResistance += resValue;
        }
    }

    private void CheckedDailyPay()
    {
        switch (_heroLevel)
        {
            case 1:
                _dailyPay = Random.Range(20, 40);
                break;

            case 2:
                _dailyPay = Random.Range(40, 100);
                break;

            case 3:
                _dailyPay = Random.Range(100, 250);
                break;

            case 4:
                _dailyPay = Random.Range(250, 600);
                break;
        }
    }

    public void UnsubEvents()
    {
        _levelSystem.OnResTalentTaken -= ChangeResistanceForLevelByTalent;
    }

    //public void UpdateResistance(TypeDamage type, int value)
    //{
    //    switch (type)
    //    {
    //        case TypeDamage.Fire:
    //            Resistance currentRes = _resistance.Find(r => r.TypeDamage == type);
    //            currentRes.ValueResistance += value;
    //            break;

    //        case TypeDamage.Cold:
    //            Resistance currentRes1 = _resistance.Find(r => r.TypeDamage == type);
    //            currentRes1.ValueResistance += value;
    //            break;

    //        case TypeDamage.Lightning:
    //            Resistance currentRes2 = _resistance.Find(r => r.TypeDamage == type);
    //            currentRes2.ValueResistance += value;
    //            break;

    //        case TypeDamage.Nectotic:
    //            Resistance currentRes3 = _resistance.Find(r => r.TypeDamage == type);
    //            currentRes3.ValueResistance += value;
    //            break;

    //        case TypeDamage.Void:
    //            Resistance currentRes4 = _resistance.Find(r => r.TypeDamage == type);
    //            currentRes4.ValueResistance += value;
    //            break;

    //        case TypeDamage.Poison:
    //            Resistance currentRes5 = _resistance.Find(r => r.TypeDamage == type);
    //            currentRes5.ValueResistance += value;
    //            break;

    //        case TypeDamage.Acid:
    //            Resistance currentRes6 = _resistance.Find(r => r.TypeDamage == type);
    //            currentRes6.ValueResistance += value;
    //            break;

    //        case TypeDamage.Explosive:
    //            Resistance currentRes7 = _resistance.Find(r => r.TypeDamage == type);
    //            currentRes7.ValueResistance += value;
    //            break;
    //    }
    //}

    //public void UpdateResistance(ResistanceList resistanceList, bool equip)
    //{
    //    foreach (var resistance in resistanceList.Resistances)
    //    {
    //        var currentResistance = _resistance.Find(r => r.TypeDamage == resistance.TypeDamage);
    //        if (currentResistance != null)
    //        {
    //            currentResistance.ValueResistance += equip ? resistance.ValueResistance : -resistance.ValueResistance;
    //        }
    //        else if (equip)
    //        {
    //            _resistance.Add(new Resistance { TypeDamage = resistance.TypeDamage, ValueResistance = resistance.ValueResistance });
    //        }
    //    }
    //}
}
