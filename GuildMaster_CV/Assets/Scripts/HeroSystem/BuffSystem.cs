using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuffSystem
{
    [Header("Others")]
    [SerializeField] private bool _silenceDebuff;
    [Header("Buffs")]
    [SerializeField] private List<BuffList> _baseBuffs;
    [SerializeField] private List<BuffList> _equipBuffs;
    [SerializeField] private List<BuffList> _abilityBuffs;
    [SerializeField] private List<BuffList> _auraBuffs;
    [SerializeField] private List<BuffList> _allBuffs;
    [Header("Neutralize Debuffs")]
    [SerializeField] private List<DebuffList> _baseNeutralizeDebuffs;
    [SerializeField] private List<DebuffList> _equipNeutralizeDebuffs;
    [SerializeField] private List<DebuffList> _abilityNeutralizeDebuffs;
    [SerializeField] private List<DebuffList> _auraNeutralizeDebuffs;
    [SerializeField] private List<DebuffList> _allNeutralizeDebuffs;
    [Header("Neutralize Affix")]
    [SerializeField] private List<AffixList> _baseNeutralizeAffix;
    [SerializeField] private List<AffixList> _equipNeutralizeAffix;
    [SerializeField] private List<AffixList> _abilityNeutralizeAffix;
    [SerializeField] private List<AffixList> _auraNeutralizeAffix;
    [SerializeField] private List<AffixList> _allNeutralizeAffix;

    public List<BuffList> AllBuffs => _allBuffs;
    public List<DebuffList> AllNeutralizeDebuffs => _allNeutralizeDebuffs;
    public List<AffixList> AllNeutralizeAffix => _allNeutralizeAffix;
    public bool SilenceDebuff => _silenceDebuff;

    public BuffSystem(List<BuffList> allBuffs, List<DebuffList> allNeutralizeDebuffs, List<AffixList> allNeutralizeAffix) // ƒÀﬂ  Œœ»» √≈–Œﬂ ¬«ﬂ“Œ√Œ  ¬≈—“¿
    {
        _allBuffs = new List<BuffList>();
        _allNeutralizeDebuffs = new List<DebuffList>();
        _allNeutralizeAffix = new List<AffixList>();

        _allBuffs.Clear();
        _allNeutralizeDebuffs.Clear();
        _allNeutralizeAffix.Clear();

        foreach (BuffList buff in allBuffs)
            _allBuffs.Add(buff);

        foreach (DebuffList deb in allNeutralizeDebuffs)
            _allNeutralizeDebuffs.Add(deb);

        foreach (AffixList affix in allNeutralizeAffix)
            _allNeutralizeAffix.Add(affix);
    }

    public BuffSystem()
    {
        _baseBuffs = new List<BuffList>();
        _baseNeutralizeDebuffs = new List<DebuffList>();
        _baseNeutralizeAffix = new List<AffixList>();

        _equipBuffs = new List<BuffList>();
        _equipNeutralizeDebuffs = new List<DebuffList>();
        _equipNeutralizeAffix = new List<AffixList>();

        _abilityBuffs = new List<BuffList>();
        _abilityNeutralizeDebuffs = new List<DebuffList>();
        _abilityNeutralizeAffix = new List<AffixList>();

        _auraBuffs = new List<BuffList>();
        _auraNeutralizeDebuffs = new List<DebuffList>();
        _auraNeutralizeAffix = new List<AffixList>();

        _allBuffs = new List<BuffList>();
        _allNeutralizeDebuffs = new List<DebuffList>();
        _allNeutralizeAffix = new List<AffixList>();
    }

    private void CalculateBaseBuffs(Hero hero, QuestParametresSystemFix questSystem)
    {
        _baseBuffs = new List<BuffList>();
        _baseNeutralizeDebuffs = new List<DebuffList>();
        _baseNeutralizeAffix = new List<AffixList>();

        //if (questSystem.GuildTalentSystem.ExtraAbilitySlot.IsTakenTalent)
        //{

        //}
    }

    private void EquipBuffs(Hero hero)
    {
        _equipBuffs = new List<BuffList>();
        _equipNeutralizeDebuffs = new List<DebuffList>();
        _equipNeutralizeAffix = new List<AffixList>();

        foreach (EquipSlot equipItemSlot in hero.EquipHolder.EquipSystem.Slots)
        {
            if (equipItemSlot != null && equipItemSlot.BaseItemData != null)
            {
                foreach (BuffList equipBuff in equipItemSlot.ItemBuffs)
                    _equipBuffs.Add(equipBuff);

                foreach (DebuffList equipDebuff in equipItemSlot.ItemDefDebuffs)
                {
                    _equipNeutralizeDebuffs.Add(equipDebuff);
                }

                foreach (AffixList equipAffix in equipItemSlot.ItemDefAffix)
                    _equipNeutralizeAffix.Add(equipAffix);
            }
        }
    }

    private void AbilityBuffs(Hero hero)
    {
        _abilityBuffs = new List<BuffList>();
        _abilityNeutralizeDebuffs = new List<DebuffList>();
        _abilityNeutralizeAffix = new List<AffixList>();

        foreach (Ability heroAbility in hero.ActiveBuffs)
        {
            foreach (BuffList abilBuff in heroAbility.Buffs)
                _abilityBuffs.Add(abilBuff);

            foreach (Debuff abilDebuff in heroAbility.DebuffsForDefence)
                _abilityNeutralizeDebuffs.Add(abilDebuff.DebuffData.DebuffList);

            foreach (AffixList abilAffix in heroAbility.AbilityData.NeutralizeAffixList)
                _abilityNeutralizeAffix.Add(abilAffix);
        }
    }

    private void AuraBuffs(HeroQuestSlot_UI heroSlot, Quest quest, List<Ability> heroAuras)
    {
        _auraBuffs = new List<BuffList>();
        _auraNeutralizeDebuffs = new List<DebuffList>();
        _auraNeutralizeAffix = new List<AffixList>();

        if (heroAuras.Count != 0)
        {
            int amountSuppresionAura = 0;

            foreach (Enemy enemy in quest.EnemiesList)
            {
                foreach (Ability enemyAbility in enemy.ListAbilities)
                {
                    if (enemyAbility.AbilityData.TypeAbility == TypeAbilities.Suppression_Aura)
                        amountSuppresionAura++;
                }
            }

            foreach (Ability heroAura in heroAuras)
            {
                if (heroAura.AbilityData.MasterAura)
                {
                    foreach (BuffList auraBuff in heroAura.Buffs)
                        _auraBuffs.Add(auraBuff);

                    foreach (Debuff auraDebuff in heroAura.DebuffsForDefence)
                        _abilityNeutralizeDebuffs.Add(auraDebuff.DebuffData.DebuffList);

                    foreach (AffixList auraAffix in heroAura.AbilityData.NeutralizeAffixList)
                        _abilityNeutralizeAffix.Add(auraAffix);
                }

                else
                {
                    if (amountSuppresionAura >= 4)
                    {

                    }

                    else if (amountSuppresionAura == 0)
                    {
                        foreach (BuffList auraBuff in heroAura.Buffs)
                            _auraBuffs.Add(auraBuff);

                        foreach (Debuff auraDebuff in heroAura.DebuffsForDefence)
                            _abilityNeutralizeDebuffs.Add(auraDebuff.DebuffData.DebuffList);

                        foreach (AffixList auraAffix in heroAura.AbilityData.NeutralizeAffixList)
                            _abilityNeutralizeAffix.Add(auraAffix);
                    }

                    else
                    {
                        if (heroSlot.SlotIndex == amountSuppresionAura - 1)
                        {
                            foreach (BuffList auraBuff in heroAura.Buffs)
                                _auraBuffs.Add(auraBuff);

                            foreach (Debuff auraDebuff in heroAura.DebuffsForDefence)
                                _abilityNeutralizeDebuffs.Add(auraDebuff.DebuffData.DebuffList);

                            foreach (AffixList auraAffix in heroAura.AbilityData.NeutralizeAffixList)
                                _abilityNeutralizeAffix.Add(auraAffix);
                        }
                    }
                }
            }
        }
    }

    public void SumBuffsEffects(HeroQuestSlot_UI heroSlot, Quest quest, QuestParametresSystemFix questSystem)
    {
        _silenceDebuff = false;
        _allBuffs = new List<BuffList>();
        _allNeutralizeDebuffs = new List<DebuffList>();
        _allNeutralizeAffix = new List<AffixList>();

        CalculateBaseBuffs(heroSlot.Hero, questSystem);
        EquipBuffs(heroSlot.Hero);
        AbilityBuffs(heroSlot.Hero);
        AuraBuffs(heroSlot, quest, questSystem.HeroAuras);

        SumBuffs();
        SumDebuffs();
        SumAffixes();
    }

    private void SumBuffs()
    {
        foreach (BuffList buff in _baseBuffs)
            _allBuffs.Add(buff);

        foreach (BuffList buff in _equipBuffs)
            _allBuffs.Add(buff);

        foreach (BuffList buff in _abilityBuffs)
            _allBuffs.Add(buff);

        foreach (BuffList buff in _auraBuffs)
            _allBuffs.Add(buff);
    }

    private void SumDebuffs()
    {
        foreach (DebuffList debuff in _baseNeutralizeDebuffs)
            _allNeutralizeDebuffs.Add(debuff);

        foreach (DebuffList debuff in _equipNeutralizeDebuffs)
            _allNeutralizeDebuffs.Add(debuff);

        foreach (DebuffList debuff in _abilityNeutralizeDebuffs)
            _allNeutralizeDebuffs.Add(debuff);

        foreach (DebuffList debuff in _auraNeutralizeDebuffs)
            _allNeutralizeDebuffs.Add(debuff);
    }

    private void SumAffixes()
    {
        foreach (AffixList affix in _baseNeutralizeAffix)
            _allNeutralizeAffix.Add(affix);

        foreach (AffixList affix in _equipNeutralizeAffix)
            _allNeutralizeAffix.Add(affix);

        foreach (AffixList affix in _abilityNeutralizeAffix)
            _allNeutralizeAffix.Add(affix);

        foreach (AffixList affix in _auraNeutralizeAffix)
            _allNeutralizeAffix.Add(affix);
    }

    public void ChangeSilenceDebuff(Hero hero, QuestParametresSystemFix questSystem)
    {
        _silenceDebuff = true;

        if (questSystem.GuildTalentSystem.ExtraAbilitySlot.IsTakenTalent)
        {
            hero.AbilityHolder.HeroAbilitySystem.RemoveAbility(2);
        }

        else
            hero.AbilityHolder.HeroAbilitySystem.RemoveAbility(1);
    }
}
