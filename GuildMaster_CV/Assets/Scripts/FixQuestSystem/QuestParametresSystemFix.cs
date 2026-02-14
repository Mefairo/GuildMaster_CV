using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestParametresSystemFix : MonoBehaviour
{
    [Header("Other Objects")]
    [SerializeField] private WorldEventSystem _worldEventSystem;
    [SerializeField] private GuildTalentSystem _guildTalentSystem;
    [SerializeField] private DynamicHeroAbilityDisplay _heroAbilityDisplay;
    [Header("Main Chances")]
    [SerializeField] private float _fullChanceComplete;
    [SerializeField] private float _chanceSupplies;
    [SerializeField] private float _chanceCombat;
    [SerializeField] private float _chanceRes;
    [SerializeField] private float _chanceAffixes;
    [Header("Combat Chances")]
    [SerializeField] private float _fullChancePower;
    [SerializeField] private float _fullChanceDefence;
    [Header("Supply Chances")]
    [SerializeField] private float _foodChance;
    [SerializeField] private float _lightChance;
    [SerializeField] private float _healChance;
    [SerializeField] private float _manaChance;
    [Header("Main Parametres")]
    [SerializeField] private float _totalHeroesPower;
    [SerializeField] private float _totalHeroesDefence;
    [Header("Secondary Parametres")]
    [SerializeField] private float _totalAvailableFood;
    [SerializeField] private float _totalAvailableLight;
    [SerializeField] private float _totalAvailableHeal;
    [SerializeField] private float _totalAvailableMana;
    [SerializeField] private float _totalRequiredFood;
    [SerializeField] private float _totalRequiredLight;
    [SerializeField] private float _totalRequiredHeal;
    [SerializeField] private float _totalRequiredMana;
    [Header("Hero Parametres")]
    [SerializeField] private List<float> _powerHeroes;
    [SerializeField] private List<float> _powerHeroesByAbil;
    [SerializeField] private List<float> _powerHeroesEffectivenessByAbil;
    [SerializeField] private List<float> _defenceHeroes;
    [SerializeField] private List<float> _foodAvailable;
    [SerializeField] private List<float> _lightAvailable;
    [SerializeField] private List<float> _healAvailable;
    [SerializeField] private List<float> _manaAvailable;
    [Header("Hero Supplies Required")]
    [SerializeField] private List<float> _foodRequired;
    [SerializeField] private List<float> _lightRequired;
    [SerializeField] private List<float> _healRequired;
    [SerializeField] private List<float> _manaRequired;
    [Header("Hero Supplies Required")]
    [SerializeField] private float _foodCoef;
    [SerializeField] private float _lightCoef;
    [SerializeField] private float _healCoef;
    [SerializeField] private float _manaCoef;
    [Header("Ability Hero")]
    [SerializeField] private List<Ability> _heroAuras;
    [SerializeField] private CalculattionAbilitiesSystem _calculateAbilitiesSystem = new CalculattionAbilitiesSystem();
    [Header("Others")]
    [SerializeField] private List<Resistance> _allQuestResistances = new List<Resistance>();


    private AffixApplyingEffect _affixApply;

    public float FullChanceComplete => _fullChanceComplete;
    public float ChanceSupplies => _chanceSupplies;
    public float ChanceCombat => _chanceCombat;
    public float FoodCoef => _foodCoef;
    public float LightCoef => _lightCoef;
    public float HealCoef => _healCoef;
    public float ManaCoef => _manaCoef;
    public List<Ability> HeroAuras => _heroAuras;
    public GuildTalentSystem GuildTalentSystem => _guildTalentSystem;
    public List<float> FoodRequired => _foodRequired;
    public List<float> LightRequired => _lightRequired;
    public List<float> HealRequired => _healRequired;
    public List<float> ManaRequired => _manaRequired;
    public List<float> FoodAvailable => _foodAvailable;
    public List<float> LightAvailable => _lightAvailable;
    public List<float> HealAvailable => _healAvailable;
    public List<float> ManaAvailable => _manaAvailable;
    public float TotalAvailableFood => _totalAvailableFood;
    public float TotalAvailableLight => _totalAvailableLight;
    public float TotalAvailableHeal => _totalAvailableHeal;
    public float TotalAvailableMana => _totalAvailableMana;
    public float TotalRequiredFood => _totalRequiredFood;
    public float TotalRequiredLight => _totalRequiredLight;
    public float TotalRequiredHeal => _totalRequiredHeal;
    public float TotalRequiredMana => _totalRequiredMana;
    public float TotalHeroesPower => _totalHeroesPower;
    public float TotalHeroesDefence => _totalHeroesDefence;
    public List<float> PowerHeroesByAbil => _powerHeroesByAbil;
    public List<float> DefenceHeroes => _defenceHeroes;

    public QuestParametres QuestParametres { get; private set; }
    public MainQuestKeeperDisplay MainQuestKeeperDisplay { get; private set; }
    public RecruitQuestKeeperController RecruitQuestKeeperController { get; private set; }
    public PrepareQuestUIController PrepareQuestUIController { get; private set; }

    private void Awake()
    {
        QuestParametres = GetComponentInParent<QuestParametres>();
        MainQuestKeeperDisplay = GetComponentInParent<MainQuestKeeperDisplay>();
        RecruitQuestKeeperController = GetComponentInParent<RecruitQuestKeeperController>();
        PrepareQuestUIController = GetComponentInParent<PrepareQuestUIController>();
    }

    private void OnEnable()
    {
        PrepareQuestUIController.OnTakeAbility += CheckRequiresQuest;
        HeroAbilitySlot_UI.OnDeleteAbility += CheckRequiresQuest;

        RecruitQuestKeeperController.OnChangeRoster += CheckRequiresQuest;
        HeroQuestSlot_UI.OnDeleteHero += CheckRequiresQuest;

        EquipItem.OnChangeStat += CheckRequiresQuest;

        AutofillSystem.OnAutoFillSupplies += CheckRequiresQuest;
    }

    private void OnDisable()
    {
        PrepareQuestUIController.OnTakeAbility -= CheckRequiresQuest;
        HeroAbilitySlot_UI.OnDeleteAbility -= CheckRequiresQuest;

        RecruitQuestKeeperController.OnChangeRoster -= CheckRequiresQuest;
        HeroQuestSlot_UI.OnDeleteHero -= CheckRequiresQuest;

        EquipItem.OnChangeStat -= CheckRequiresQuest;

        AutofillSystem.OnAutoFillSupplies -= CheckRequiresQuest;
    }

    public void CheckRequiresQuest()
    {
        if (MainQuestKeeperDisplay.SelectedQuest == null)
            return;

        QuestParametres.ClearInfo();
        ResetQuestParametres();

        ClearChances();

        CheckHeroPreBuffs();
        CheckHeroAuras();

        CheckUniqueDebuffs(MainQuestKeeperDisplay.SelectedQuest.Quest.Quest);

        SetEnemyParametres(MainQuestKeeperDisplay.SelectedQuest.Quest.Quest);

        foreach (HeroQuestSlot_UI heroSlot in MainQuestKeeperDisplay.HeroesSlots.Slots)
        {
            if (heroSlot.Hero != null && heroSlot.Hero.HeroData != null)
            {
                ResetHeroParametres(heroSlot);

                heroSlot.Hero.BuffSystem.SumBuffsEffects(heroSlot, MainQuestKeeperDisplay.SelectedQuest.Quest.Quest, this);

                CheckUniqueDebuffs(heroSlot.Hero, MainQuestKeeperDisplay.SelectedQuest.Quest.Quest);

                _powerHeroes[heroSlot.SlotIndex] = Mathf.FloorToInt(heroSlot.Hero.HeroPowerPoints.SumPower(heroSlot.Hero,
                    MainQuestKeeperDisplay.SelectedQuest.Quest.Quest, this, heroSlot));

                _defenceHeroes[heroSlot.SlotIndex] = Mathf.FloorToInt(heroSlot.Hero.HeroDefencePoints.SumDefence(heroSlot.Hero,
                    MainQuestKeeperDisplay.SelectedQuest.Quest.Quest, this, heroSlot));

                heroSlot.Hero.ResistanceSystem.SumRes(heroSlot.Hero, MainQuestKeeperDisplay.SelectedQuest.Quest.Quest, this, heroSlot);

                CalculateHeroPowerByAbilities(heroSlot);

                CalculateHeroAmountFood(heroSlot);
                CalculateHeroAmountLight(heroSlot);
                CalculateHeroAmountHeal(heroSlot);
                CalculateHeroAmountMana(heroSlot);

                //CheckHeroPostBuffs(heroSlot);

                heroSlot.UpdateUI(_powerHeroesByAbil[heroSlot.SlotIndex], _defenceHeroes[heroSlot.SlotIndex]);

            }
        }

        if (MainQuestKeeperDisplay.SelectedHero != null && MainQuestKeeperDisplay.SelectedHero.Hero != null)
            PrepareQuestUIController.UpdateUIInfo(MainQuestKeeperDisplay.SelectedHero.Hero);

        CalculateSupplyChance();
        CalculateCombatChance();
        CalculateCompleteChance();

        UpdateUIInfo();
    }

    private void SetEnemyParametres(Quest quest)
    {
        foreach (Enemy enemy in quest.EnemiesList)
        {
            enemy.ActiveBuffs.Clear();
            enemy.DebuffImmun.Clear();

            foreach (Ability enemyAbil in enemy.ListAbilities)
            {
                if(enemyAbil.AbilityData.GeneralType == GeneralTypeAbility.Stats)
                {
                    enemy.ActiveBuffs.Add(enemyAbil);

                    foreach (Debuff immunDebuff in enemyAbil.DebuffsForDefence)
                        enemy.DebuffImmun.Add(immunDebuff);
                }
            }

            enemy.EnemyResistanceSystem.SumRes(enemy, quest, this);
        }
    }

    private void CheckHeroPreBuffs()
    {
        foreach (HeroQuestSlot_UI heroSlot in MainQuestKeeperDisplay.HeroesSlots.Slots)
        {
            if (heroSlot.Hero != null && heroSlot.Hero.HeroData != null)
            {
                heroSlot.Hero.ActiveBuffs.Clear();
                heroSlot.Hero.ActiveUniqueBuffs.Clear();
            }
        }

        foreach (HeroQuestSlot_UI heroSlot in MainQuestKeeperDisplay.HeroesSlots.Slots)
        {
            if (heroSlot.Hero != null && heroSlot.Hero.HeroData != null)
            {
                foreach (Ability heroAbility in heroSlot.Hero.AbilityHolder.HeroAbilitySystem.AbilityList)
                {
                    if (heroAbility != null & heroAbility.AbilityData != null)
                    {
                        if (heroAbility.AbilityData.GeneralType == GeneralTypeAbility.Stats && heroAbility.AbilityData.TypeAbility == TypeAbilities.Buff)
                        {
                            heroAbility.ApplyBuffEffect_FIX(heroSlot, MainQuestKeeperDisplay.HeroesSlots);
                        }
                    }
                }
            }
        }
    }

    private void CheckHeroPostBuffs(HeroQuestSlot_UI heroSlot)
    {
        foreach (Ability heroAbility in heroSlot.Hero.AbilityHolder.HeroAbilitySystem.AbilityList)
        {
            if (heroAbility != null & heroAbility.AbilityData != null)
            {
                if (heroAbility.AbilityData.PostBuff)
                {
                    if (heroAbility.AbilityData.GeneralType == GeneralTypeAbility.Stats && heroAbility.AbilityData.TypeAbility == TypeAbilities.Buff)
                    {
                        heroAbility.ApplyBuffEffect_FIX(heroSlot, MainQuestKeeperDisplay.HeroesSlots);
                    }
                }
            }
        }
    }

    private void CheckHeroAuras()
    {
        _heroAuras = new List<Ability>();

        foreach (HeroQuestSlot_UI heroSlot in MainQuestKeeperDisplay.HeroesSlots.Slots)
        {
            if (heroSlot.Hero != null && heroSlot.Hero.HeroData != null)
            {
                foreach (Ability heroAbility in heroSlot.Hero.AbilityHolder.HeroAbilitySystem.AbilityList)
                {
                    if (heroAbility != null & heroAbility.AbilityData != null)
                    {
                        if (heroAbility.AbilityData.GeneralType == GeneralTypeAbility.Stats && heroAbility.AbilityData.TypeAbility == TypeAbilities.Aura)
                        {
                            _heroAuras.Add(heroAbility);
                            heroAbility.ApplyAuraEffect_FIX(heroSlot, MainQuestKeeperDisplay.HeroesSlots);
                        }
                    }
                }
            }
        }

        SetAurasForHeroes();
    }

    private void SetAurasForHeroes()
    {
        foreach (HeroQuestSlot_UI heroSlot in MainQuestKeeperDisplay.HeroesSlots.Slots)
        {
            if (heroSlot.Hero != null && heroSlot.Hero.HeroData != null)
                heroSlot.Hero.ActiveAuras.Clear();
        }

        foreach (HeroQuestSlot_UI heroSlot in MainQuestKeeperDisplay.HeroesSlots.Slots)
        {
            if (heroSlot.Hero != null && heroSlot.Hero.HeroData != null)
            {
                foreach (Ability aura in _heroAuras)
                {
                    heroSlot.Hero.ActiveAuras.Add(aura);
                }
            }
        }
    }

    private void CheckUniqueDebuffs(Hero hero, Quest quest)
    {
        foreach (Enemy enemy in quest.EnemiesList)
        {
            foreach (Ability enemyAbility in enemy.ListAbilities)
            {
                foreach (Debuff enemyDebuff in enemyAbility.DebuffsForAttack)
                {
                    if (enemyDebuff.DebuffData.IsUnique && !enemyDebuff.DebuffData.IsGlobal)
                    {
                        if (!hero.BuffSystem.AllNeutralizeDebuffs.Contains(enemyDebuff.DebuffData.DebuffList))
                        {
                            enemyDebuff.DebuffData.ApplyDebuffEffect(hero, quest, this);
                        }
                    }
                }
            }
        }
    }

    private void CheckUniqueDebuffs(Quest quest)
    {
        foreach (Enemy enemy in quest.EnemiesList)
        {
            foreach (Ability enemyAbility in enemy.ListAbilities)
            {
                foreach (Debuff enemyDebuff in enemyAbility.DebuffsForAttack)
                {
                    if (enemyDebuff.DebuffData.IsUnique && enemyDebuff.DebuffData.IsGlobal)
                    {
                        enemyDebuff.DebuffData.ApplyDebuffEffect(null, quest, this);
                    }
                }
            }
        }
    }

    private void ResetQuestParametres()
    {
        _powerHeroes = new List<float> { 0, 0, 0, 0 };
        _powerHeroesByAbil = new List<float> { 0, 0, 0, 0 };
        _powerHeroesEffectivenessByAbil = new List<float> { 0, 0, 0, 0 };
        _defenceHeroes = new List<float> { 0, 0, 0, 0 };

        _foodAvailable = new List<float> { 0, 0, 0, 0 };
        _lightAvailable = new List<float> { 0, 0, 0, 0 };
        _healAvailable = new List<float> { 0, 0, 0, 0 };
        _manaAvailable = new List<float> { 0, 0, 0, 0 };

        _foodRequired = new List<float> { 0, 0, 0, 0 };
        _lightRequired = new List<float> { 0, 0, 0, 0 };
        _healRequired = new List<float> { 0, 0, 0, 0 };
        _manaRequired = new List<float> { 0, 0, 0, 0 };

        _totalHeroesPower = 0;
        _totalHeroesDefence = 0;

        _totalAvailableFood = 0;
        _totalAvailableLight = 0;
        _totalAvailableHeal = 0;
        _totalAvailableMana = 0;

        _totalRequiredFood = 0;
        _totalRequiredLight = 0;
        _totalRequiredHeal = 0;
        _totalRequiredMana = 0;

        _foodCoef = 1;
        _lightCoef = 1;
        _healCoef = 1;
        _manaCoef = 1;
    }
    private void UpdateUIInfo()
    {
        QuestParametres.UpdateFoodInfoUI(_totalRequiredFood, _totalAvailableFood);
        QuestParametres.UpdateLightInfoUI(_totalRequiredLight, _totalAvailableLight);
        QuestParametres.UpdateHealInfoUI(_totalRequiredHeal, _totalAvailableHeal);
        QuestParametres.UpdateManaInfoUI(_totalRequiredMana, _totalAvailableMana);

        QuestParametres.UpdateSupplyInfoUI(_chanceSupplies);
        QuestParametres.UpdateCombatInfoUI(_chanceCombat);
        QuestParametres.UpdateCompleteInfoUI(_fullChanceComplete);

        QuestParametres.UpdateResistanceInfoUI(_allQuestResistances);

        QuestParametres.UpdateQuestInfoUI(_totalHeroesPower, _totalHeroesDefence);

        if (MainQuestKeeperDisplay.SelectedHero != null && MainQuestKeeperDisplay.SelectedHero.Hero != null && MainQuestKeeperDisplay.SelectedHero.Hero.HeroData != null)
            _heroAbilityDisplay.RefreshDynamicAbilities(MainQuestKeeperDisplay.SelectedHero.Hero.AbilityHolder.HeroAbilitySystem);
    }

    private void ClearChances()
    {
        _fullChanceComplete = 0;

        _chanceSupplies = 0;
        _chanceCombat = 0;
        _chanceRes = 0;
        _chanceAffixes = 0;

        _fullChancePower = 0;
        _fullChanceDefence = 0;

        _foodChance = 0;
        _lightChance = 0;
        _healChance = 0;
        _manaChance = 0;
    }

    private void CalculateCompleteChance()
    {
        //if (float.IsNaN(_fullChanceComplete))
        //    _fullChanceComplete = 0;

        //else
        //    _fullChanceComplete = _chanceCombat * _chanceSupplies / 100;

        _fullChanceComplete = CalculateSafeValue((_chanceCombat * _chanceSupplies), 100);
    }

    public float CalculateSafeValue(float numerator, float denominator)
    {
        // Проверка на валидность и расчет
        float result = (denominator == 0 || float.IsNaN(numerator) || float.IsNaN(denominator))
            ? 0f
            : (numerator / denominator);

        // Замена невалидных значений
        return float.IsNaN(result) || float.IsInfinity(result) ? 0f : result;
    }

    private void CalculateSupplyChance()
    {
        if (_totalRequiredFood <= 0)
            _foodChance = 0;

        else
            _foodChance = (_totalAvailableFood / _totalRequiredFood) * 100; //%

        if (_totalRequiredLight <= 0)
            _lightChance = 0;

        else
            _lightChance = (_totalAvailableLight / _totalRequiredLight) * 100; //%

        if (_totalRequiredHeal <= 0)
            _healChance = 0;

        else
            _healChance = (_totalAvailableHeal / _totalRequiredHeal) * 100; //%

        if (_totalRequiredMana <= 0)
            _manaChance = 0;

        else
            _manaChance = (_totalAvailableMana / _totalRequiredMana) * 100; //%

        _chanceSupplies = Mathf.Round(CalculateSafeValue((_foodChance + _lightChance + _healChance + _manaChance), 4));  // 4 - количество компонентов

        //if (float.IsNaN(_foodChance + _lightChance + _healChance + _manaChance))
        //    _chanceSupplies = 0;

        //else
        //    _chanceSupplies = Mathf.Round((_foodChance + _lightChance + _healChance + _manaChance) / 4);
    }

    private void CalculateCombatChance()
    {
        #region POWER
        foreach (float power in _powerHeroesByAbil)
            _totalHeroesPower += power;

        if (_totalHeroesPower < MainQuestKeeperDisplay.SelectedQuest.Quest.Quest.Power)
        {
            if (_totalHeroesPower == 0)
                _fullChancePower = 0;

            else
                _fullChancePower = (_totalHeroesPower / MainQuestKeeperDisplay.SelectedQuest.Quest.Quest.Power) * 100; //%
        }

        else
            _fullChancePower = 100;
        #endregion


        #region DEFENCE
        foreach (float defence in _defenceHeroes)
            _totalHeroesDefence += defence;

        if (_totalHeroesDefence < MainQuestKeeperDisplay.SelectedQuest.Quest.Quest.Defence)
        {
            if (_totalHeroesDefence == 0)
                _fullChanceDefence = 0;

            else
                _fullChanceDefence = (_totalHeroesDefence / MainQuestKeeperDisplay.SelectedQuest.Quest.Quest.Defence) * 100; //%
        }

        else
            _fullChanceDefence = 100;
        #endregion

        CheckResistance();

        _chanceAffixes = CheckAffixes();

        //if (float.IsNaN(_fullChancePower + _fullChanceDefence + _chanceRes + _chanceAffixes))
        //    _chanceCombat = 0;

        //else
        //    _chanceCombat = Mathf.Round((_fullChancePower + _fullChanceDefence + _chanceRes + _chanceAffixes) / 4); // 4 - количество компонентов

        _chanceCombat = Mathf.Round(CalculateSafeValue((_fullChancePower + _fullChanceDefence + _chanceRes + _chanceAffixes), 4));
    }

    private float CheckAffixes()
    {
        List<Affix> affixesList = MainQuestKeeperDisplay.SelectedQuest.Quest.Quest.AffixesList;

        if (affixesList.Count == 0)
            return 100;

        List<Hero> heroes = new List<Hero>();

        List<Affix> unConsiderAffixes = new List<Affix>();
        int heroCount = 0;

        foreach (HeroQuestSlot_UI heroSlot in MainQuestKeeperDisplay.HeroesSlots.Slots)
        {
            Hero hero = heroSlot.Hero;

            if (hero == null || hero.HeroData == null)
            {
                continue;
            }

            heroCount++;
            heroes.Add(hero);
        }

        if (heroes.Count == 0)
            return 0;

        _affixApply = new AffixApplyingEffect();
        int applyingAffixes = _affixApply.CheckAffixes(affixesList, heroes);

        foreach (Affix affix in affixesList)
        {
            if (affix.AffixData.ConsiderCountAffix == false)
                unConsiderAffixes.Add(affix);
        }

        float chanceAffixesComplete = 100 - (applyingAffixes * 100 / (affixesList.Count - unConsiderAffixes.Count) / heroes.Count);

        return chanceAffixesComplete;
    }

    private void CheckResistance()
    {
        float sumResChance = 0;
        int amountActiveHeroes = 0;
        _allQuestResistances = new List<Resistance>();

        foreach (Resistance questRes in MainQuestKeeperDisplay.SelectedQuest.Quest.Quest.QuestResistance)
        {
            bool alreadyExists = _allQuestResistances.Any(res => res.TypeDamage == questRes.TypeDamage);

            if (!alreadyExists)
                _allQuestResistances.Add(questRes);
        }

        foreach (Resistance questRes in _allQuestResistances)
        {
            foreach (HeroQuestSlot_UI heroSlot in MainQuestKeeperDisplay.HeroesSlots.Slots)
            {
                if (heroSlot != null && heroSlot.Hero != null)
                {
                    Resistance matchingHeroRes = heroSlot.Hero.ResistanceSystem.AllRes.Find(res => res.TypeDamage == questRes.TypeDamage);

                    //float localChanceRes = CalculateSafeValue((float)matchingHeroRes.ValueResistance, questRes.ValueResistance);
                    //float chanceRes = localChanceRes * 100;
                    float chanceRes = (float)matchingHeroRes.ValueResistance / questRes.ValueResistance * 100;
                    float visibleRes = 0;

                    if (chanceRes <= 100)
                        visibleRes = chanceRes;

                    else
                        visibleRes = 100;

                    sumResChance += visibleRes;
                }
            }
        }

        foreach (HeroQuestSlot_UI heroSlot in MainQuestKeeperDisplay.HeroesSlots.Slots)
        {
            if (heroSlot != null && heroSlot.Hero != null)
                amountActiveHeroes++;
        }

        if (sumResChance == 0)
            _chanceRes = 0;

        else
        {
            float chanceHeroRes = sumResChance / _allQuestResistances.Count;
            _chanceRes = chanceHeroRes / amountActiveHeroes;
        }
    }

    private void CalculateHeroAmountFood(HeroQuestSlot_UI heroSlot)
    {
        QuestSlot_UI questSlot = MainQuestKeeperDisplay.SelectedQuest;

        _foodAvailable[heroSlot.SlotIndex] = 0;
        _foodRequired[heroSlot.SlotIndex] = questSlot.Quest.Quest.AmountDaysToComplete;

        foreach (EquipSlot equipSlot in heroSlot.Hero.EquipHolder.EquipSystem.Slots)
        {
            if (equipSlot.EquipItemData is ConsumableItemData consumItem &&
                consumItem.TypeConsumItem == TypeConsumableItem.Food)
            {
                _foodAvailable[heroSlot.SlotIndex] += equipSlot.StackSize * consumItem.ItemValue; // Предположим, что есть свойство Amount
            }
        }

        // Если герой имеет больше еды, чем ему нужно, учитываем только необходимое количество
        float heroRequiredFood = Mathf.Min(questSlot.Quest.Quest.AmountDaysToComplete, _foodAvailable[heroSlot.SlotIndex]);

        _totalAvailableFood += heroRequiredFood;
        _totalRequiredFood += _foodRequired[heroSlot.SlotIndex];
    }

    private void CalculateHeroAmountLight(HeroQuestSlot_UI heroSlot)
    {
        QuestSlot_UI questSlot = MainQuestKeeperDisplay.SelectedQuest;
        List<DebuffList> lightList = new List<DebuffList>();

        _lightAvailable[heroSlot.SlotIndex] = 0;

        bool hasLightBuff = heroSlot.Hero.BuffSystem.AllNeutralizeDebuffs.Contains(DebuffList.Fear);

        if (hasLightBuff)
            _lightRequired[heroSlot.SlotIndex] = questSlot.Quest.Quest.AmountDaysToComplete;

        else
            _lightRequired[heroSlot.SlotIndex] = questSlot.Quest.Quest.AmountDaysToComplete * _lightCoef;



        bool hasLightBuff1 = heroSlot.Hero.BuffSystem.AllBuffs.Contains(BuffList.Light);

        if (hasLightBuff1)
            _lightAvailable[heroSlot.SlotIndex] = _lightRequired[heroSlot.SlotIndex];

        else
        {
            foreach (EquipSlot equipSlot in heroSlot.Hero.EquipHolder.EquipSystem.Slots)
            {
                if (equipSlot.EquipItemData is ConsumableItemData consumItem &&
                    consumItem.TypeConsumItem == TypeConsumableItem.Light)
                {
                    _lightAvailable[heroSlot.SlotIndex] += equipSlot.StackSize * consumItem.ItemValue;
                }
            }
        }

        // Если герой имеет больше еды, чем ему нужно, учитываем только необходимое количество
        float heroRequiredLight = Mathf.Min(_lightRequired[heroSlot.SlotIndex], _lightAvailable[heroSlot.SlotIndex]);

        _totalAvailableLight += heroRequiredLight;
        _totalRequiredLight += _lightRequired[heroSlot.SlotIndex];
    }

    private void CalculateHeroAmountHeal(HeroQuestSlot_UI heroSlot)
    {
        QuestSlot_UI questSlot = MainQuestKeeperDisplay.SelectedQuest;
        List<DebuffList> bleedList = new List<DebuffList>();

        _healAvailable[heroSlot.SlotIndex] = 0;

        bool hasHealBuff = heroSlot.Hero.BuffSystem.AllNeutralizeDebuffs.Contains(DebuffList.Bleeding);

        if (hasHealBuff)
            _healRequired[heroSlot.SlotIndex] = heroSlot.Hero.VisibleHeroStats.Health;

        else
            _healRequired[heroSlot.SlotIndex] = heroSlot.Hero.VisibleHeroStats.Health * _healCoef;

        foreach (EquipSlot equipSlot in heroSlot.Hero.EquipHolder.EquipSystem.Slots)
        {
            if (equipSlot.EquipItemData is ConsumableItemData consumItem &&
                consumItem.TypeConsumItem == TypeConsumableItem.Heal)
            {
                _healAvailable[heroSlot.SlotIndex] += equipSlot.StackSize * consumItem.ItemValue;
            }
        }

        foreach (Ability ability in heroSlot.Hero.ActiveBuffs)
            _healAvailable[heroSlot.SlotIndex] += ability.AbilityData.HealValue;

        foreach (Ability ability in heroSlot.Hero.ActiveAuras)
            _healAvailable[heroSlot.SlotIndex] += ability.AbilityData.HealValue;

        // Если герой имеет больше еды, чем ему нужно, учитываем только необходимое количество
        float heroRequiredHeal = Mathf.Min(_healRequired[heroSlot.SlotIndex], _healAvailable[heroSlot.SlotIndex]);

        _totalAvailableHeal += heroRequiredHeal;
        _totalRequiredHeal += _healRequired[heroSlot.SlotIndex];
    }

    private void CalculateHeroAmountMana(HeroQuestSlot_UI heroSlot)
    {
        QuestSlot_UI questSlot = MainQuestKeeperDisplay.SelectedQuest;
        List<DebuffList> bleedList = new List<DebuffList>();

        _manaAvailable[heroSlot.SlotIndex] = 0;

        bool hasManaBuff = heroSlot.Hero.BuffSystem.AllNeutralizeDebuffs.Contains(DebuffList.Withering);

        foreach (Ability heroAbil in heroSlot.Hero.AbilityHolder.HeroAbilitySystem.AbilityList)
        {
            if (heroAbil != null && heroAbil.AbilityData != null)
            {
                if (hasManaBuff)
                    _manaRequired[heroSlot.SlotIndex] += heroAbil.ManaCost;

                else
                    _manaRequired[heroSlot.SlotIndex] += heroAbil.ManaCost * _manaCoef;
            }
        }

        foreach (EquipSlot equipSlot in heroSlot.Hero.EquipHolder.EquipSystem.Slots)
        {
            if (equipSlot.EquipItemData is ConsumableItemData consumItem &&
                consumItem.TypeConsumItem == TypeConsumableItem.Mana)
            {
                _manaAvailable[heroSlot.SlotIndex] += equipSlot.StackSize * consumItem.ItemValue;
            }
        }

        // Если герой имеет больше еды, чем ему нужно, учитываем только необходимое количество
        float heroRequiredMana = Mathf.Min(_manaRequired[heroSlot.SlotIndex], _manaAvailable[heroSlot.SlotIndex]);

        _totalAvailableMana += heroRequiredMana;
        _totalRequiredMana += _manaRequired[heroSlot.SlotIndex];
    }

    private void CalculateHeroPowerByAbilities(HeroQuestSlot_UI heroSlot)
    {
        int amountAttackAbils = 0;
        _powerHeroesEffectivenessByAbil[heroSlot.SlotIndex] = 0;

        float localEffectiveness = 0;
        float localPowerByAbil = 0;



        foreach (Ability abil in heroSlot.Hero.AbilityHolder.HeroAbilitySystem.AbilityList)
        {
            if (abil != null && abil.AbilityData != null)
            {
                if (abil.AbilityData.GeneralType == GeneralTypeAbility.Attack)
                {
                    amountAttackAbils++;

                    localEffectiveness += _calculateAbilitiesSystem.CheckAbilitiesHero(MainQuestKeeperDisplay.SelectedQuest,
                        heroSlot, abil);

                    float dynamicPower = abil.GetDynamicPower(heroSlot, MainQuestKeeperDisplay.SelectedQuest.Quest.Quest,
                        MainQuestKeeperDisplay.HeroesSlots);

                    localPowerByAbil += Mathf.FloorToInt(_powerHeroes[heroSlot.SlotIndex] + dynamicPower); //100%
                }
            }
        }

        if (amountAttackAbils > 0)
        {
            _powerHeroesEffectivenessByAbil[heroSlot.SlotIndex] = localEffectiveness / amountAttackAbils;
            _powerHeroesByAbil[heroSlot.SlotIndex] = Mathf.FloorToInt((_powerHeroesEffectivenessByAbil[heroSlot.SlotIndex] *
                   localPowerByAbil) / 100); //100%
        }

        else
        {
            _powerHeroesEffectivenessByAbil[heroSlot.SlotIndex] = 0;
            _powerHeroesByAbil[heroSlot.SlotIndex] = 0;
        }
    }

    public void ChangeLightCoef(float coef)
    {
        _lightCoef += coef;
    }

    public void ChangeHealCoef(float coef)
    {
        _healCoef += coef;
    }

    public void ChangeManaCoef(float coef)
    {
        _manaCoef += coef;
    }

    private void ResetHeroParametres(HeroQuestSlot_UI heroSlot)
    {
        heroSlot.Hero.HeroPowerPoints.ResetValues(heroSlot.Hero);
        heroSlot.Hero.HeroDefencePoints.ResetValues(heroSlot.Hero);
    }

    public void RecalculatePower()
    {

    }
}
