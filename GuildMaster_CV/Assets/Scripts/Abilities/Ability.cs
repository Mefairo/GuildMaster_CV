using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Ability
{
    [SerializeField] protected AbilityData _abilityData;
    [Header("Main Values")]
    [SerializeField] private float _powerValue;
    [SerializeField] private float _defenceValue;
    [SerializeField] private float _healValue;
    [SerializeField] private float _manaCost;
    [SerializeField] private float _coefBuff; // коэффициент повышения силы за каждый применненный бафф к врагу
    [SerializeField] private float _coefAoe; // коэффициент снижения силы за каждое требуемое аое 
    [SerializeField] private List<HeroStats> _stats;
    [SerializeField] private List<Resistance> _resistancesForAttack;
    [SerializeField] private List<Resistance> _resistancesForDefence;
    [SerializeField] private List<Debuff> _debuffsForAttack;
    [SerializeField] private List<Debuff> _debuffsForDefence;
    [SerializeField] private List<Resistance> _resistancesForDecrease;
    [SerializeField] private List<BuffList> _buffs = new List<BuffList>();
    [SerializeField] private List<BuffList> _neutralizeBuffs = new List<BuffList>();
    [SerializeField] private Ability _mainTreeAbility;
    [SerializeField] private Ability _parentAbility;
    [Header("Calculation Values")]
    [SerializeField] private float _abilityPercent;
    [SerializeField] private float _coef2;
    [SerializeField] private float _coef3;

    [SerializeField] public ActivateEnemyAbility ActivateEnemyAbility;
    [SerializeField] public ActivateHeroAbility ActivateHeroAbility;

    public static UnityAction<Hero> OnApplyDebuffNeutralize;
    public static UnityAction<Hero> OnCancelDebuffNeutralize;

    public AbilityData AbilityData => _abilityData;
    public float PowerValue => _powerValue;
    public float DefenceValue => _defenceValue;
    public float HealValue => _healValue;
    public float ManaCost => _manaCost;
    public float СoefBuff => _coefBuff;
    public float СoefAoe => _coefAoe;
    public List<HeroStats> Stats => _stats;
    public List<Resistance> ResistancesForAttack => _resistancesForAttack;
    public List<Resistance> ResistancesForDefence => _resistancesForDefence;
    public List<Debuff> DebuffsForAttack => _debuffsForAttack;
    public List<Debuff> DebuffsForDefence => _debuffsForDefence;
    public List<Resistance> ResistancesForDecrease => _resistancesForDecrease;
    public List<BuffList> Buffs => _buffs;
    public List<BuffList> NeutralizeBuffs => _neutralizeBuffs;
    public Ability MainTreeAbility => _mainTreeAbility;
    //public ActivateEnemyAbility ActivateEnemyAbility => _activateEnemyAbility;


    public Ability(Ability abilityCopy)
    {
        _abilityData = abilityCopy.AbilityData;

        _powerValue = _abilityData.PowerValue;
        _defenceValue = _abilityData.DefenceValue;
        _healValue = _abilityData.HealValue;
        _manaCost = _abilityData.ManaCost;
        _coefBuff = _abilityData.СoefBuff;
        _coefAoe = _abilityData.СoefAoe;
        _stats = _abilityData.Stats;
        _resistancesForAttack = DeepCopyResistance(_abilityData.ResistancesForAttack);
        _resistancesForDefence = DeepCopyResistance(_abilityData.ResistancesForDefence);
        _resistancesForDecrease = DeepCopyResistance(_abilityData.ResistancesForDecrease);
        _debuffsForAttack = DeepCopyDebuffs(_abilityData.DebuffsForAttack);
        _debuffsForDefence = DeepCopyDebuffs(_abilityData.DebuffsForDefence);
        _buffs = _abilityData.BuffList;
        _neutralizeBuffs = _abilityData.NeutralizeBuffList;

        if (abilityCopy.AbilityData.MainTreeAbility != null)
            _mainTreeAbility = new Ability(abilityCopy.AbilityData.MainTreeAbility);
    }

    public Ability(AbilityData abilityCopy)
    {
        _abilityData = abilityCopy;

        _powerValue = _abilityData.PowerValue;
        _defenceValue = _abilityData.DefenceValue;
        _healValue = _abilityData.HealValue;
        _manaCost = _abilityData.ManaCost;
        _stats = _abilityData.Stats;
        _resistancesForAttack = DeepCopyResistance(_abilityData.ResistancesForAttack);
        _resistancesForDefence = DeepCopyResistance(_abilityData.ResistancesForDefence);
        _resistancesForDecrease = DeepCopyResistance(_abilityData.ResistancesForDecrease);
        _debuffsForAttack = DeepCopyDebuffs(_abilityData.DebuffsForAttack);
        _debuffsForDefence = DeepCopyDebuffs(_abilityData.DebuffsForDefence);
        //_mainTreeAbility = new Ability(_abilityData.MainTreeAbility);
    }

    public Ability() { }

    private void SelectEnemyIDAbility(Ability enemyAbility)
    {
        switch (enemyAbility.AbilityData.ID)
        {
            // АНТИХИЛ АБИЛКА ВРАГОВ

            case 1001:
                enemyAbility.ActivateEnemyAbility = new EnemyAntiHealAbility(enemyAbility);
                break;

            // МАНАДРЕЙН АБИЛКА ВРАГОВ

            case 1051:
                enemyAbility.ActivateEnemyAbility = new ManaDrain(enemyAbility);
                break;

            case 4042:
                enemyAbility.ActivateEnemyAbility = new SoulWithering(enemyAbility);
                break;

            case 4012:
                enemyAbility.ActivateEnemyAbility = new Stench(enemyAbility);
                break;
        }
    }

    private void SelectHeroIDAbility(HeroQuestSlot_UI heroSlot, Ability ability, QuestSlot_UI questSlot, List<HeroQuestSlot_UI> heroesSlots)
    {
        //switch (ability.AbilityData.ID)
        //{
        //    // ДАРК НАЙТ

        //    case 21122:
        //        ability.ActivateHeroAbility = new DarkKnightAbility_1_2_2(ability);
        //        break;

        //    case 21222:
        //        ability.ActivateHeroAbility = new DarkKnightAbility_2_2_2(ability);
        //        break;

        //    case 21421:
        //        ability.ActivateHeroAbility = new DarkKnightAbility_4_2_1(ability);
        //        break;

        //    case 21522:
        //        ability.ActivateHeroAbility = new DarkKnightAbility_5_2_2(ability);
        //        break;

        //    // ИНЖЕНЕР

        //    case 71122:
        //        ability.ActivateHeroAbility = new EngineerAbility_1_2_2(ability);
        //        break;

        //    case 71422:
        //        ability.ActivateHeroAbility = new EngineerAbility_4_2_2(ability);
        //        break;

        //    case 71521:
        //        ability.ActivateHeroAbility = new EngineerAbility_5_2_1(ability);
        //        break;

        //    // МАГ

        //    case 51122:
        //        ability.ActivateHeroAbility = new MageAbility_1_2_2(ability);
        //        break;

        //    case 51321:
        //        ability.ActivateHeroAbility = new MageAbility_3_2_1(ability);
        //        break;

        //    // НЕКРОМАНСЕР

        //    case 61121:
        //        ability.ActivateHeroAbility = new NecromancerAbility_1_2_1(ability);
        //        break;

        //    // ПАЛАДИН

        //    case 11121:
        //        ability.ActivateHeroAbility = new PaladinAbility_1_2_1(ability);
        //        break;

        //    case 11321:
        //        ability.ActivateHeroAbility = new PaladinAbility_3_2_1(ability);
        //        break;

        //    case 11622:
        //        ability.ActivateHeroAbility = new PaladinAbility_6_2_2(ability);
        //        break;

        //    // ПРИМАЛИСТ

        //    case 31121:
        //        ability.ActivateHeroAbility = new PrimalistAbility_1_2_1(ability);
        //        break;
        //}
    }

    private List<Resistance> DeepCopyResistance(List<Resistance> original)
    {
        List<Resistance> copy = new List<Resistance>();
        foreach (Resistance res in original)
        {
            Resistance newRes = new Resistance(res);
            copy.Add(newRes);
        }
        return copy;
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

    public float AbilityDamageSingleTarget(QuestSlot_UI questSlot, HeroQuestSlot_UI heroSlot)
    {
        float calculationResult = CalculateAbilityEffectiveness(this, questSlot, heroSlot);
        (int, int) resultAoeRequire = CheckEnemyRequireAoeDamage(questSlot);
        int enemyRequireAoe = resultAoeRequire.Item1;
        int enemyAbilitiesRequireAoe = resultAoeRequire.Item2;
        float result = 0;

        result = calculationResult - (enemyRequireAoe + enemyAbilitiesRequireAoe) * _coefAoe;
        return result;
    }

    private float CalculateAbilityEffectiveness(Ability activeAbility, QuestSlot_UI questSlot, HeroQuestSlot_UI heroSlot)
    {
        if (activeAbility == null || questSlot == null || questSlot.Quest == null ||
            questSlot.Quest.Quest == null || questSlot.Quest.Quest.EnemiesList == null)
        {
            Debug.Log("zero");
            return 0;
        }

        List<TypeDamage> resistHeroAbilityList = CollectHeroResistances(activeAbility);

        float totalEffectiveness = 0;
        int enemyCount = questSlot.Quest.Quest.EnemiesList.Count;

        foreach (Enemy enemy in questSlot.Quest.Quest.EnemiesList)
        {
            float effectivenessAgainstEnemy = CalculateEffectivenessAgainstEnemy(resistHeroAbilityList, enemy, heroSlot);
            totalEffectiveness += effectivenessAgainstEnemy;
        }

        int amountActiveDebuff = 0;

        foreach (Debuff debuff in activeAbility.DebuffsForAttack)
        {
            foreach (Enemy enemy in questSlot.Quest.Quest.EnemiesList)
            {
                List<DebuffList> enemyDebuffImmunType = new List<DebuffList>();

                foreach (Debuff debuffEnemyImmun in enemy.DebuffImmun)
                    enemyDebuffImmunType.Add(debuffEnemyImmun.DebuffData.DebuffList);

                if (debuff.DebuffData.DebuffList == DebuffList.Fracture || debuff.DebuffData.DebuffList == DebuffList.Burn ||
                    debuff.DebuffData.DebuffList == DebuffList.Freezing || debuff.DebuffData.DebuffList == DebuffList.Shock ||
                     debuff.DebuffData.DebuffList == DebuffList.Curse || debuff.DebuffData.DebuffList == DebuffList.Blood_Infection ||
                     debuff.DebuffData.DebuffList == DebuffList.Lightleak)
                    Debug.Log("enemy res debuf");

                else
                {
                    if (!enemyDebuffImmunType.Contains(debuff.DebuffData.DebuffList))
                        amountActiveDebuff++;
                }
            }
        }

        _abilityPercent = 0;
        _abilityPercent = (totalEffectiveness / enemyCount) + (amountActiveDebuff / enemyCount) * _coefBuff; // 20% - коэффициент 20% для дебафов
        return _abilityPercent;
    }

    private List<TypeDamage> CollectHeroResistances(Ability ability)
    {
        List<TypeDamage> resistances = new List<TypeDamage>();

        foreach (Resistance res in ability.ResistancesForAttack)
        {
            resistances.Add(res.TypeDamage);
        }

        return resistances;
    }

    private float CalculateEffectivenessAgainstEnemy(List<TypeDamage> heroResistances, Enemy enemy, HeroQuestSlot_UI heroSlot)
    {
        int matchingResistanceCount = 0;
        float resistanceSum = 0;

        foreach (Resistance resEnemy in enemy.EnemyResistanceSystem.AllRes)
        {
            if (heroResistances.Contains(resEnemy.TypeDamage))
            {
                matchingResistanceCount++;
                resistanceSum += resEnemy.ValueResistance;
            }
        }

        if (matchingResistanceCount == 0) return 100; // Если у врага нет совпадающих сопротивлений, эффективность 100%

        float effectiveness = (100 * matchingResistanceCount - resistanceSum) / matchingResistanceCount;
        return effectiveness;
    }

    private (int, int) CheckEnemyRequireAoeDamage(QuestSlot_UI questSlot)
    {
        int enemyRequireAoe = 0;
        int enemyAbilityRequireAoe = 0;

        foreach (Enemy enemy in questSlot.Quest.Quest.EnemiesList)
        {
            if (enemy.RequireAoeDamage)
                enemyRequireAoe++;

            foreach (Ability ability in enemy.ListAbilities)
            {
                if (ability.AbilityData.TypeAbility == TypeAbilities.Summon)
                    enemyAbilityRequireAoe++;
            }
        }

        return (enemyRequireAoe, enemyAbilityRequireAoe);
    }

    public float AbilityDamageAoeDamage(QuestSlot_UI questSlot, HeroQuestSlot_UI heroSlot)
    {
        float result = CalculateAbilityEffectiveness(this, questSlot, heroSlot);

        return result;
    }

    public float AbilitySummonDamage(QuestSlot_UI questSlot, HeroQuestSlot_UI heroSlot)
    {
        float calculationResultByEnemyRes = CalculateAbilityEffectiveness(this, questSlot, heroSlot);
        (int, int) resultAoeRequire = CheckEnemyRequireAoeDamage(questSlot);
        int enemyRequireAoe = resultAoeRequire.Item1;
        int enemyAbilitiesRequireAoe = resultAoeRequire.Item2;

        List<TypeDamage> abilityEnemyTypes = new List<TypeDamage>();

        foreach (Enemy enemy in questSlot.Quest.Quest.EnemiesList)
        {
            foreach (Ability enemyAbility in enemy.ListAbilities)
            {
                foreach (Resistance resAttack in enemyAbility.ResistancesForAttack)
                {
                    if (!abilityEnemyTypes.Contains(resAttack.TypeDamage))
                        abilityEnemyTypes.Add(resAttack.TypeDamage);
                }
            }
        }

        float result = calculationResultByEnemyRes / (enemyRequireAoe + enemyAbilitiesRequireAoe + 1);
        return result * this.AbilityData.AmountSummon;
    }

    public float AbilityDamageAuraEffect(QuestSlot_UI questSlot, HeroQuestSlot_UI heroSlot)
    {
        float result = CalculateAbilityEffectiveness(this, questSlot, heroSlot);
        int suppresAuraAmount = 0;

        foreach (Enemy enemy in questSlot.Quest.Quest.EnemiesList)
        {
            foreach (Ability enemyAbil in enemy.ListAbilities)
            {
                if (enemyAbil.AbilityData.TypeAbility == TypeAbilities.Suppression_Aura)
                    suppresAuraAmount++;
            }
        }

        if (AbilityData.MasterAura)
            return result;

        else
            return result / (suppresAuraAmount + 1);
    }

    public void AbilityBuffEffect(Ability ability, QuestSlot_UI questSlot, HeroQuestSlot_UI heroSlot, bool apply)
    {

    }



    //////FIX
    ///

    public virtual void ApplyBuffEffect_FIX(HeroQuestSlot_UI heroSlot, SendHeroesSlots heroesSlots)
    {
        heroSlot.Hero.ActiveBuffs.Add(this);

        if (_abilityData.UniqBuff)
            heroSlot.Hero.ActiveUniqueBuffs.Add(this);
    }

    public virtual void ApplyUniqueBuffPower(HeroQuestSlot_UI heroSlot, Quest quest, SendHeroesSlots heroes)
    {

    }

    public virtual void ApplyUniqueBuffDefence(HeroQuestSlot_UI heroSlot, Quest quest, SendHeroesSlots heroes)
    {

    }

    public virtual List<Resistance> ApplyUniqueBuffRes(HeroQuestSlot_UI heroSlot, Quest quest, SendHeroesSlots heroes, List<Resistance> allRes)
    {
        List<Resistance> nullList = new List<Resistance>();
        return nullList;
    }

    public virtual void ApplyAuraEffect_FIX(HeroQuestSlot_UI heroSlot, SendHeroesSlots heroesSlots)
    {
        //foreach (Debuff debuff in this.DebuffsForDefence)
        //{
        //    foreach (HeroQuestSlot_UI activeHeroSlot in heroesSlots.Slots)
        //    {
        //        if (activeHeroSlot.Hero != null)
        //        {
        //            activeHeroSlot.Hero.NeutralizeDebuffs.Add(debuff.DebuffData.DebuffList);
        //        }
        //    }
        //}
    }

    public virtual float GetDynamicPower(HeroQuestSlot_UI heroSlot, Quest quest, SendHeroesSlots heroes)
    {
        return 0f;
    }
}
