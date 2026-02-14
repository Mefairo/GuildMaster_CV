using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class BlankSlot : EquipSlot
{
    [SerializeField] private BlankSlotData _blankSlotData;
    [SerializeField] private int _blankTier;
    [SerializeField] private int _propertyAmount;
    [SerializeField] private int _craftValue;
    [SerializeField] private Color _colorBack;
    [SerializeField] private List<RuneSlotData> _usedRuneSlots = new List<RuneSlotData>();
    [SerializeField] private ItemType _itemType;
    [Header("Base Properties")]
    [SerializeField] private HeroStats _baseStats = new HeroStats();
    [Header("Neutral Properties")]
    [SerializeField] private HeroStats _neutralStats = new HeroStats();
    [SerializeField] private List<Resistance> _neutralRes = new List<Resistance>();
    [SerializeField] private List<Debuff> _neutralDebuffs = new List<Debuff>();
    [Header("Sealed Properties")]
    [SerializeField] private HeroStats _sealedStats = new HeroStats();
    [SerializeField] private List<Resistance> _sealedRes = new List<Resistance>();
    [SerializeField] private List<Debuff> _sealedDebuffs = new List<Debuff>();

    public override SlotData BaseItemData => _blankSlotData;

    public int BlankTier => _blankTier;
    public int PropertyAmount => _propertyAmount;
    public int CraftValue => _craftValue;
    public List<RuneSlotData> UsedRuneSlots => _usedRuneSlots;
    public ItemType ItemType => _itemType;
    public Color ColorBack => _colorBack;
    public BlankSlotData BlankSlotData => _blankSlotData;
    public HeroStats BaseStats => _baseStats;
    public HeroStats NeutralStats => _neutralStats;
    public List<Resistance> NeutralRes => _neutralRes;
    public List<Debuff> NeutralDebuffs => _neutralDebuffs;
    public HeroStats SealedStats => _sealedStats;
    public List<Resistance> SealedRes => _sealedRes;
    public List<Debuff> SealedDebuffs => _sealedDebuffs;

    public BlankSlot(BlankSlotData blankSlotData, int level)
    {
        _blankSlotData = blankSlotData;
        _stackSize = 1;

        _itemData = blankSlotData;
        _itemID = blankSlotData.ID;

        SetTier(level);
        SetPropertyAmount(level);
        SetCraftValue(level);

        _itemType = blankSlotData.ItemType;

        _iconSlot = blankSlotData.Icon;
    }

    public BlankSlot(BlankSlot blankSlot, int index)
    {
        _blankSlotData = blankSlot.BlankSlotData;
        _stackSize = blankSlot.StackSize;
        _index = index;

        _itemData = blankSlot.BaseItemData;
        _itemID = blankSlot.BaseItemData.ID;

        _blankTier = blankSlot.BlankTier;
        _propertyAmount = blankSlot.PropertyAmount;
        _craftValue = blankSlot.CraftValue;
        _itemType = blankSlot.ItemType;
        _usedRuneSlots = blankSlot.UsedRuneSlots;

        _colorBack = blankSlot.ColorBack;
        //_usedRuneSlots = DeepCopyRuneSlots(blankSlot.UsedRuneSlots);

        _baseStats = DeepCopyStats(blankSlot.BaseStats);
        _neutralStats = DeepCopyStats(blankSlot.NeutralStats);
        _sealedStats = DeepCopyStats(blankSlot.SealedStats);

        _neutralRes = DeepCopyResistances(blankSlot.NeutralRes);
        _sealedRes = DeepCopyResistances(blankSlot.SealedRes);

        _neutralDebuffs = DeepCopyDebuffs(blankSlot.NeutralDebuffs);
        _sealedDebuffs = DeepCopyDebuffs(blankSlot.SealedDebuffs);

        _itemStats = blankSlot.ItemStats;
        _itemDefRes = blankSlot.ItemDefRes;
        _itemDefDebuffs = blankSlot.ItemDefDebuffs;
        _itemDefDebuffsData = blankSlot.ItemDefDebuffsData;
        _itemDefAffix = blankSlot.ItemDefAffix;
        _itemBuffs = blankSlot.ItemBuffs;

        _iconSlot = blankSlot.IconSlot;
    }

    public BlankSlot() { }

    public void ApplyReforgeEffect()
    {
        _itemStats = new List<HeroStats>();
        _itemStats.Add(new HeroStats());

        _itemDefRes = new List<ResistanceList>();
        _itemDefRes.Add(new ResistanceList());
        _itemDefRes[0].Resistances = new List<Resistance>();

        _itemDefDebuffs = new List<DebuffList>();
        _itemDefDebuffsData = new List<Debuff>();
        _itemDefAffix = new List<AffixList>();
        _itemBuffs = new List<BuffList>();

        ApplyReforgeEffectStats(_baseStats);
        ApplyReforgeEffectStats(_neutralStats);
        ApplyReforgeEffectStats(_sealedStats);

        ChangeBackColor();

        if (_neutralRes.Count != 0)
            foreach (Resistance res in _neutralRes)
                ApplyReforgeEffectRes(res);

        if (_sealedRes.Count != 0)
            foreach (Resistance res in _sealedRes)
                ApplyReforgeEffectRes(res);

        if (_neutralDebuffs.Count != 0)
            foreach (Debuff debuff in _neutralDebuffs)
                ApplyReforgeEffectDebuffs(debuff);

        if (_sealedDebuffs.Count != 0)
            foreach (Debuff debuff in _sealedDebuffs)
                ApplyReforgeEffectDebuffs(debuff);
    }

    public void AddUsedRunes(List<RuneSlot_UI> runeSlots)
    {
        _usedRuneSlots = new List<RuneSlotData>();

        foreach (RuneSlot_UI runeSlot in runeSlots)
        {
            if(runeSlot.RuneSlot != null && runeSlot.RuneSlot.RuneSlotData != null)
                _usedRuneSlots.Add(runeSlot.RuneSlot.RuneSlotData);
        }
    }

    private void ApplyReforgeEffectStats(HeroStats stats)
    {
        _itemStats[0].ChangeStrengthValue(stats.StrengthValue);
        _itemStats[0].ChangeDexterityValue(stats.DexterityValue);
        _itemStats[0].ChangeIntelligenceValue(stats.IntelligenceValue);

        _itemStats[0].ChangeEnduranceValue(stats.EnduranceValue);
        _itemStats[0].ChangeFlexibilityValue(stats.FlexibilityValue);
        _itemStats[0].ChangeSanityValue(stats.SanityValue);

        _itemStats[0].ChangeHasteValue(stats.HasteValue);
        _itemStats[0].ChangeCritValue(stats.CritValue);
        _itemStats[0].ChangeAccuracyValue(stats.AccuracyValue);

        _itemStats[0].ChangeDurabilityValue(stats.DurabilityValue);
        _itemStats[0].ChangeAdaptabilityValue(stats.AdaptabilityValue);
        _itemStats[0].ChangeSuppressionValue(stats.SuppressionValue);
    }

    private void ApplyReforgeEffectRes(Resistance itemRes)
    {
        Resistance currentRes = _itemDefRes[0].Resistances.Find(r => r.TypeDamage == itemRes.TypeDamage);
        if (currentRes != null)
        {
            currentRes.ValueResistance += itemRes.ValueResistance;
        }
        else
        {
            // ≈ÒÎË ÒÓÔÓÚË‚ÎÂÌËÂ ÌÂ Ì‡È‰ÂÌÓ, ‰Ó·‡‚ÎˇÂÏ Â„Ó ‚ ÒÔËÒÓÍ
            _itemDefRes[0].Resistances.Add(new Resistance { TypeDamage = itemRes.TypeDamage, ValueResistance = itemRes.ValueResistance });
        }
    }

    private void ApplyReforgeEffectDebuffs(Debuff debuff)
    {
        if (debuff?.DebuffData == null || debuff.DebuffData.DebuffList == DebuffList.None)
        {
            Debug.LogError("Invalid debuff data!");
            return;
        }

        // ﬂ‚ÌÓÂ Ò‡‚ÌÂÌËÂ ÁÌ‡˜ÂÌËÈ enum
        bool alreadyExists = _itemDefDebuffs.Any(d => d == debuff.DebuffData.DebuffList);

        if (!alreadyExists)
        {
            _itemDefDebuffs.Add(debuff.DebuffData.DebuffList);
            _itemDefDebuffsData.Add(debuff);
            Debug.Log($"Added debuff: {debuff.DebuffData.DebuffList}");
        }
    }

    private void SetTier(int level)
    {
        //_tier = Random.Range(1, level + 1);
        _blankTier = level;
    }

    private void SetPropertyAmount(int level)
    {
        _propertyAmount = Random.Range(_blankSlotData.BlankParametres[level - 1].MinPropertyAmount, _blankSlotData.BlankParametres[level - 1].MaxPropertyAmount + 1);
    }

    private void SetCraftValue(int level)
    {
        _craftValue = Random.Range(_blankSlotData.BlankParametres[level - 1].MinCraftValue, _blankSlotData.BlankParametres[level - 1].MaxCraftValue + 1);
    }

    public int SetRandomValues(int minValue, int maxValue)
    {
        int newValue = Random.Range(minValue, maxValue);
        return newValue;
    }

    public override void AssignItem(InventorySlot slot)
    {
        _itemData = slot.BaseItemData;
        _itemID = slot.BaseItemData.ID;
        _stackSize = slot.StackSize;

        _iconSlot = slot.IconSlot;

        if (slot is BlankSlot blankSlot)
        {
            _blankSlotData = blankSlot.BlankSlotData;
            _blankTier = blankSlot.BlankTier;
            _propertyAmount = blankSlot.PropertyAmount;
            _craftValue = blankSlot.CraftValue;
            _usedRuneSlots = blankSlot.UsedRuneSlots;

            _colorBack = blankSlot.ColorBack;

            //_usedRuneSlots = DeepCopyRuneSlots(blankSlot.UsedRuneSlots);

            _baseStats = DeepCopyStats(blankSlot.BaseStats);
            _neutralStats = DeepCopyStats(blankSlot.NeutralStats);
            _sealedStats = DeepCopyStats(blankSlot.SealedStats);

            _neutralRes = DeepCopyResistances(blankSlot.NeutralRes);
            _sealedRes = DeepCopyResistances(blankSlot.SealedRes);

            _neutralDebuffs = DeepCopyDebuffs(blankSlot.NeutralDebuffs);
            _sealedDebuffs = DeepCopyDebuffs(blankSlot.SealedDebuffs);

            _itemStats = blankSlot.ItemStats;
            _itemDefRes = blankSlot.ItemDefRes;
            _itemDefDebuffs = blankSlot.ItemDefDebuffs;
            _itemDefDebuffsData = blankSlot.ItemDefDebuffsData;
            _itemDefAffix = blankSlot.ItemDefAffix;
            _itemBuffs = blankSlot.ItemBuffs;
        }

        else
            _blankSlotData = null;
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

    private List<Debuff> DeepCopyDebuffs(List<Debuff> original)
    {
        List<Debuff> copy = new List<Debuff>();
        foreach (Debuff debuff in original)
        {
            Debuff newDebuff = new Debuff(debuff);
            copy.Add(newDebuff);
        }
        return copy;
    }

    private List<RuneSlotData> DeepCopyRuneSlots(List<RuneSlotData> original)
    {
        List<RuneSlotData> copy = new List<RuneSlotData>();

        foreach (RuneSlotData runeSlotData in original)
            _usedRuneSlots.Add(runeSlotData);

        return copy;
    }

    public void SetStatType(StatType statType, int statValue, HeroStats blankStats)
    {
        switch (statType)
        {
            // —»À¿ «Õ¿◊≈Õ»ﬂ œ≈–¬»◊Õ€≈

            case StatType.StrengthValue:
                blankStats.ChangeStrengthValue(statValue);
                break;

            case StatType.DexterityValue:
                blankStats.ChangeDexterityValue(statValue);
                break;

            case StatType.IntelligenceValue:
                blankStats.ChangeIntelligenceValue(statValue);
                break;



            // «¿Ÿ»“¿ «Õ¿◊≈Õ»ﬂ œ≈–¬»◊Õ€≈

            case StatType.EnduranceValue:
                blankStats.ChangeEnduranceValue(statValue);
                break;

            case StatType.FlexibilityValue:
                blankStats.ChangeFlexibilityValue(statValue);
                break;

            case StatType.SanityValue:
                blankStats.ChangeSanityValue(statValue);
                break;



            // —»À¿ «Õ¿◊≈Õ»ﬂ ¬“Œ–»◊Õ€≈

            case StatType.HasteValue:
                blankStats.ChangeHasteValue(statValue);
                break;

            case StatType.CritValue:
                blankStats.ChangeCritValue(statValue);
                break;

            case StatType.AccuracyValue:
                blankStats.ChangeAccuracyValue(statValue);
                break;



            // «¿Ÿ»“¿ «Õ¿◊≈Õ»ﬂ ¬“Œ–»◊Õ€≈

            case StatType.DurabilityValue:
                blankStats.ChangeDurabilityValue(statValue);
                break;

            case StatType.AdaptabilityValue:
                blankStats.ChangeAdaptabilityValue(statValue);
                break;

            case StatType.SuppressionValue:
                blankStats.ChangeSuppressionValue(statValue);
                break;
        }
    }

    public void SetTypeRes(TypeDamage typeRes, int resValue)
    {
        Resistance currentRes = _neutralRes.Find(r => r.TypeDamage == typeRes);
        if (currentRes != null)
            currentRes.ValueResistance += resValue;

        else
        {
            // ≈ÒÎË ÒÓÔÓÚË‚ÎÂÌËÂ ÌÂ Ì‡È‰ÂÌÓ, ‰Ó·‡‚ÎˇÂÏ Â„Ó ‚ ÒÔËÒÓÍ
            _neutralRes.Add(new Resistance { TypeDamage = typeRes, ValueResistance = resValue });
        }
    }

    public void SetNeutalStats(HeroStats stats)
    {
        _neutralStats = stats;
    }

    public void SetSealedStats(HeroStats stats)
    {
        _sealedStats = stats;
    }

    public void ReduceCraftValue(int value)
    {
        _craftValue -= value;
    }

    public void ReducePropertyAmount(int value)
    {
        _propertyAmount -= value;
    }

    public override void ClearSlot()
    {
        base.ClearSlot();

        _blankSlotData = null;
        _blankTier = -1;
        _propertyAmount = -1;
        _craftValue = -1;
        _usedRuneSlots = new List <RuneSlotData>();

        _baseStats = new HeroStats();

        _neutralStats = new HeroStats();
        _neutralRes = new List<Resistance>();
        _neutralDebuffs = new List<Debuff>();

        _sealedRes = new List<Resistance>();
        _sealedDebuffs = new List<Debuff>();
    }

    public void ClearAllParametres()
    {
        _baseStats = new HeroStats();

        _neutralStats = new HeroStats();
        _neutralRes = new List<Resistance>();
        _neutralDebuffs = new List<Debuff>();

        _sealedStats = new HeroStats();
        _sealedRes = new List<Resistance>();
        _sealedDebuffs = new List<Debuff>();
    }

    public void ClearBaseParametres()
    {
        _baseStats = new HeroStats();
    }

    public void ClearNeutralParametres()
    {
        _neutralStats = new HeroStats();
        _neutralRes = new List<Resistance>();
        _neutralDebuffs = new List<Debuff>();
    }

    public void ClearSealedParametres()
    {
        _sealedStats = new HeroStats();
        _sealedRes = new List<Resistance>();
        _sealedDebuffs = new List<Debuff>();
    }

    public void ChangeBackColor()
    {
        _colorBack = new Color(
    Random.Range(0f, 1f),
    Random.Range(0f, 1f),
    Random.Range(0f, 1f)
);
    }
}
