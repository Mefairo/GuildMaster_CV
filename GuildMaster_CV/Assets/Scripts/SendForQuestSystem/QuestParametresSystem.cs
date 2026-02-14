using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class QuestParametresSystem : MonoBehaviour
{
    [Header("Other Gameobjects")]
    [SerializeField] private WorldEventSystem _worldEventSystem;
    [Header("Main Chances")]
    [SerializeField] private float _fullChanceComplete;
    [SerializeField] private float _visibleChanceComplete;
    [SerializeField] private float _fullChancePower;
    [SerializeField] private float _visibleChancePower;
    [SerializeField] private float _fullChanceDefence;
    [SerializeField] private float _visibleChanceDefence;
    [Header("Secondary Parametres")]
    [SerializeField] private float _totalAvailableFood;
    [SerializeField] private float _totalAvailableLight;
    [SerializeField] private float _totalAvailableHeal;
    [SerializeField] private float _totalAvailableMana;
    [SerializeField] private float _totalRequiredFood;
    [SerializeField] private float _totalRequiredLight;
    [SerializeField] private float _totalRequiredHeal;
    [SerializeField] private float _totalRequiredMana;
    [Header("Secondary Parametres Coef")]
    [SerializeField] private float _foodCoef;
    [SerializeField] private float _lightCoef;
    [SerializeField] private float _healCoef;
    [SerializeField] private float _manaCoef;
    [Header("Secondary Chances")]
    [SerializeField] private float _chanceSupplies;
    [SerializeField] private float _chanceCombat;
    [Header("Local Chances")]
    [SerializeField] private float _chancePower;
    [SerializeField] private float _chanceDefence;
    [SerializeField] private float _chanceRes;
    [SerializeField] private float _chanceDebuffs;
    [SerializeField] private float _chanceAffixes;
    [Header("Resistance")]
    [SerializeField] private float _chanceResComplete;
    [SerializeField] private float _coefLevelRes;

    [SerializeField] private int _fireRes;
    [SerializeField] private int _coldRes;
    [SerializeField] private int _lightRes;
    [SerializeField] private int _necroticRes;
    [SerializeField] private int _voidRes;
    [Header("Hero Parametres")]
    [SerializeField] private SendHeroesSlots _heroes;
    [SerializeField] private float _powerSum;
    [SerializeField] private float _powerQuest;
    [SerializeField] private float _defenceSum;
    [SerializeField] private float _ressAll;
    [SerializeField] private float _ress1;
    [Header("Debuffs")]
    [SerializeField] private float _chanceDebuffsComplete;
    //[SerializeField] private int _applyDebuffsCount;
    [SerializeField] private List<Debuff> _debuffList;
    [SerializeField] private List<BuffList> _buffList;
    [Header("Affixes")]
    [SerializeField] private float _chanceAffixesComplete;
    [Header("Ability Hero")]
    [SerializeField] private CalculationAbilitySystem _calculateAbilitiesSystem;

    private List<TypeDamage> _questRequireRes;
    private Dictionary<Resistance, int> _questDict;
    private QuestSlot_UI _selectedSlot;
    private HeroQuestSlot_UI _selectedHero;
    private DebuffApplyingEffect _debuffApply;
    private AffixApplyingEffect _affixApply;

    public float ManaCoef => _manaCoef;

    public event UnityAction<float> OnChanceCompleteChange;
    public event UnityAction OnPowerChange;
    public event UnityAction OnDefenceChange;

    public CalculationAbilitySystem CalculateAbilitiesSystem => _calculateAbilitiesSystem;
    public float PowerSum
    {
        get => _powerSum;
        private set
        {
            _powerSum = value;
            OnPowerChange?.Invoke();
        }
    }
    public float DefenceSum
    {
        get => _defenceSum;
        private set
        {
            _defenceSum = value;
            OnDefenceChange?.Invoke();
        }
    }

    public float VisibleChanceComplete
    {
        get => _visibleChanceComplete;
        private set
        {
            _visibleChanceComplete = value;
            OnChanceCompleteChange?.Invoke(value);
        }
    }
    public float ChanceSupplies => _chanceSupplies;
    public float ChanceCombat => _chanceCombat;
    public float TotalAvailableFood => _totalAvailableFood;
    public float TotalAvailableLight => _totalAvailableLight;
    public float TotalAvailableHeal => _totalAvailableHeal;
    public float TotalAvailableMana => _totalAvailableMana;
    public float TotalRequiredFood => _totalRequiredFood;
    public float TotalRequiredLight => _totalRequiredLight;
    public float TotalRequiredHeal => _totalRequiredHeal;
    public float TotalRequiredMana => _totalRequiredMana;
    public float PowerQuest => _powerQuest;
    public int FireRes => _fireRes;
    public int ColdRes => _coldRes;
    public int LightRes => _lightRes;
    public int NecroticRes => _necroticRes;
    public int VoidRes => _voidRes;


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

    private void Start()
    {
        _calculateAbilitiesSystem.Start();
    }

    private void OnEnable()
    {
        //MainQuestKeeperDisplay.OnFillHeroSlotClick += SelectHero;
        //RecruitQuestKeeperController.OnChangeRoster += CheckRequiresQuest;
        //QuestParametres.OnCheckRequiresComplete += CheckRequiresQuest;
        //PrepareQuestUIController.OnTakeAbility += CheckRequiresQuest;
        //_calculateAbilitiesSystem.OnRemoveAbility += CheckRequiresQuest;
        //MainQuestKeeperDisplay.OnTakeQuest += ClearPowerDefence;
        //MainQuestKeeperDisplay.OnTakeQuest += ClearAbilitiesEffect;
        //MainQuestKeeperDisplay.OnPanelClose += ClearPowerDefence;
        //MainQuestKeeperDisplay.OnPanelOpen += CheckRequiresQuest;

        //RecruitQuestKeeperController.TakeFreeHero += AddLocalHeroes;

        //EquipItem.OnApplyDebuffNeutralize += ApplyDebuffEffect;
        //EquipItem.OnCancelDebuffNeutralize += CancelDebuffEffect;
    }

    private void OnDisable()
    {
        //RecruitQuestKeeperController.TakeFreeHero -= AddLocalHeroes;


        //MainQuestKeeperDisplay.OnFillHeroSlotClick -= SelectHero;
        //RecruitQuestKeeperController.OnChangeRoster -= CheckRequiresQuest;
        //QuestParametres.OnCheckRequiresComplete -= CheckRequiresQuest;
        //PrepareQuestUIController.OnTakeAbility -= CheckRequiresQuest;
        //_calculateAbilitiesSystem.OnRemoveAbility += CheckRequiresQuest;
        //MainQuestKeeperDisplay.OnTakeQuest -= ClearPowerDefence;
        //MainQuestKeeperDisplay.OnTakeQuest -= ClearAbilitiesEffect;
        //MainQuestKeeperDisplay.OnPanelClose -= ClearPowerDefence;
        //MainQuestKeeperDisplay.OnPanelOpen -= CheckRequiresQuest;

        //EquipItem.OnApplyDebuffNeutralize -= ApplyDebuffEffect;
        //EquipItem.OnCancelDebuffNeutralize -= CancelDebuffEffect;

        //if (_selectedHero != null && _selectedHero.Hero != null)
        //{
        //    _selectedHero.Hero.OnChangePowerPoints -= CheckRequiresQuest;
        //    _selectedHero.Hero.OnChangeDefencePoints -= CheckRequiresQuest;
        //    _selectedHero.Hero.OnChangeResistance -= CheckRequiresQuest;
        //    _selectedHero.Hero.OnChangeEquipped -= CheckRequiresQuest;
        //}
    }

    private void AddLocalHeroes(Hero hero)
    {
        //_calculateAbilitiesSystem.RecalculateStatsAbilities(hero);
    }
    private void SelectHero(HeroQuestSlot_UI heroSlot)
    {
        //if (_selectedHero != null)
        //{
        //    heroSlot.Hero.OnChangePowerPoints -= CheckRequiresQuest;
        //    heroSlot.Hero.OnChangeDefencePoints -= CheckRequiresQuest;
        //    heroSlot.Hero.OnChangeResistance -= CheckRequiresQuest;
        //    heroSlot.Hero.OnChangeEquipped -= CheckRequiresQuest;
        //}

        //_selectedHero = heroSlot;

        //heroSlot.Hero.OnChangePowerPoints += CheckRequiresQuest;
        //heroSlot.Hero.OnChangeDefencePoints += CheckRequiresQuest;
        //heroSlot.Hero.OnChangeResistance += CheckRequiresQuest;
        //heroSlot.Hero.OnChangeEquipped += CheckRequiresQuest;
    }

    public void CheckRequiresQuest()
    {
        //if (MainQuestKeeperDisplay.SelectedQuest == null)
        //    return;

        //QuestSlot_UI questSlot = MainQuestKeeperDisplay.SelectedQuest;
        //QuestSlot_UI lastQuestSlot = MainQuestKeeperDisplay.LastSelectedQuest;

        //ClearChances();
        //UpdateResistanceValues(questSlot);

        //int powerQuest = questSlot.Quest.Quest.Power;
        //int defenceQuest = questSlot.Quest.Quest.Defence;

        //PowerSum = 0;
        //DefenceSum = 0;

        //CheckAbilitiesHero(questSlot);

        //foreach (float powerPoints in _calculateAbilitiesSystem.PowerAbilitiesHero)
        //{
        //    if (!float.IsNaN(powerPoints))
        //        PowerSum += Mathf.FloorToInt(powerPoints);
        //}

        //foreach (float defencePoints in _calculateAbilitiesSystem.DefenceAbilitiesHero)
        //{
        //    if (!float.IsNaN(defencePoints))
        //        DefenceSum += Mathf.FloorToInt(defencePoints);
        //}

        //_chancePower = 0;
        //_chanceDefence = 0;
        //_chanceRes = 0;
        //_chanceDebuffs = 0;
        //_chanceAffixes = 0;

        //_chancePower = CheckPower(powerQuest);
        //_chanceDefence = CheckDefence(defenceQuest);
        //_chanceRes = CheckResistance();
        //_chanceAffixes = CheckAffixes();
        //_chanceDebuffs = CheckDebuffs();

        //_ressAll = _chanceRes;

        //CheckChanceCompleteQuest(_chancePower, _chanceDefence, _chanceRes, _chanceDebuffs, _chanceAffixes);
    }

    private float CheckResistance()
    {
        float sumResChance = 0;

        foreach (TypeDamage typeDamage in _questRequireRes)
        {
            foreach (HeroQuestSlot_UI hero in _heroes.Slots)
            {
                if (hero != null && hero.Hero != null)
                {
                    Resistance matchingHeroRes = hero.Hero.Resistances.Find(res => res.TypeDamage == typeDamage);
                    Resistance matchingQuestRes = _questDict.Keys.FirstOrDefault(res1 => res1.TypeDamage == typeDamage);
                    int matchingQuestResValue = _questDict[matchingQuestRes];

                    if (matchingHeroRes != null)
                    {
                        float chanceRes = (float)matchingHeroRes.ValueResistance / matchingQuestResValue * 100;
                        //Debug.Log(matchingHeroRes.ValueResistance);
                        //Debug.Log(matchingQuestResValue);
                        float visibleRes = 0;

                        if (chanceRes <= 100)
                            visibleRes = chanceRes;

                        else
                            visibleRes = 100;

                        sumResChance += visibleRes;

                    }

                    else
                    {
                        Debug.Log($"Resistance type  not found in heroRes.");
                    }
                }

                else
                    continue;
            }


        }

        int amountActiveHeroes = 0;

        foreach (HeroQuestSlot_UI hero in _heroes.Slots)
        {
            if (hero != null && hero.Hero != null && hero.Hero.HeroData != null)
                amountActiveHeroes++;
        }

        float chanceHeroRes = sumResChance / _questRequireRes.Count;
        _ress1 = chanceHeroRes;
        float chanceCompleteRes = chanceHeroRes / amountActiveHeroes;

        return chanceCompleteRes;
    }

    private float CheckPower(float powerQuest)
    {
        _powerQuest = powerQuest;

        if (_powerSum > 0)
        {
            _fullChancePower = _powerSum / _powerQuest * 100;

            if (_fullChancePower <= 100)
                _visibleChancePower = _fullChancePower;

            else
                _visibleChancePower = 100;

            return _visibleChancePower;
        }

        else
            return _visibleChancePower = 0;
    }

    private float CheckDefence(float defence)
    {
        if (_defenceSum > 0)
        {
            _fullChanceDefence = _defenceSum / defence * 100;

            if (_fullChanceDefence <= 100)
                _visibleChanceDefence = _fullChanceDefence;


            else
                _visibleChanceDefence = 100;

            return _visibleChanceDefence;
        }

        else
            return _visibleChanceDefence = 0;
    }

    private void CheckChanceCompleteQuest(float power, float defence, float resistance, float debuffs, float affixes)
    {
        //_chanceCombat = (power + defence + resistance + debuffs + affixes) / 5;

        ////float foodChance = CalculateHeroAmountFood();
        ////float lightChance = CalculateHeroAmountLight();
        ////float healChance = CalculateHeroAmountHeal();
        ////float manaChance = CalculateHeroAmountMana();

        ////_chanceSupplies = (foodChance + lightChance + healChance + manaChance) / 4;

        //if (_chanceCombat == 0)
        //{
        //    VisibleChanceComplete = 0;
        //    return;
        //}

        //_fullChanceComplete = _chanceCombat * _chanceSupplies / 100;

        //// Проверка на NaN
        //if (float.IsNaN(_fullChanceComplete))
        //{
        //    _fullChanceComplete = 0; // Или какое-то другое значение по умолчанию
        //    VisibleChanceComplete = 0;
        //    return;
        //}

        //if (_fullChanceComplete <= 100)
        //{
        //    VisibleChanceComplete = Mathf.FloorToInt(_fullChanceComplete);
        //}
        //else
        //{
        //    VisibleChanceComplete = 100;
        //}
    }

    private void ClearChances()
    {
        _fullChanceComplete = 0;
        VisibleChanceComplete = 0;

        _fullChancePower = 0;
        _visibleChancePower = 0;

        _fullChanceDefence = 0;
        _visibleChanceDefence = 0;
    }

    private void ClearResistance()
    {
        //_fireRes = 0;
        //_coldRes = 0;
        //_lightRes = 0;
        //_necroticRes = 0;
        //_voidRes = 0;
    }

    private void ClearPowerDefence()
    {
        PowerSum = 0;
        DefenceSum = 0;

        for (int i = 0; i < _calculateAbilitiesSystem.PowerAbilitiesHero.Count; i++)
            _calculateAbilitiesSystem.PowerAbilitiesHero[i] = 0;

        for (int i = 0; i < _calculateAbilitiesSystem.DefenceAbilitiesHero.Count; i++)
            _calculateAbilitiesSystem.DefenceAbilitiesHero[i] = 0;

        ChangeHealCoef(0);
        ChangeManaCoef(0);
    }

    private void UpdateResistanceValues(QuestSlot_UI questSlot)
    {
        ClearResistance();

        int level = questSlot.Quest.Quest.Level;
        List<Enemy> enemiesList = questSlot.Quest.Quest.EnemiesList;
        List<Ability> abilitiesList = new List<Ability>();
        _questRequireRes = new List<TypeDamage>();
        _questDict = new Dictionary<Resistance, int>();

        foreach (Enemy enemy in enemiesList)
        {
            foreach (Ability ability in enemy.ListAbilities)
            {
                abilitiesList.Add(ability);
            }
        }

        foreach (Ability ability in abilitiesList)
        {
            foreach (Resistance res in ability.ResistancesForAttack)
            {
                if (_questRequireRes.Contains(res.TypeDamage))
                    continue;

                else
                {
                    _questRequireRes.Add(res.TypeDamage);
                    _questDict.Add(res, level * 10);
                    //Debug.Log(res.TypeDamage);
                }
            }
        }

        foreach (TypeDamage res in _worldEventSystem.EventResList)
        {
            if (_questRequireRes.Contains(res))
                continue;

            else
            {
                Resistance newRes = new Resistance(res, level * 10);

                _questRequireRes.Add(res);
                _questDict.Add(newRes, level * 10);
            }
        }

        UpdateResistanceUI(_questRequireRes, questSlot);
    }

    public void ApplyDebuffByDeleteHeroAbility(Ability heroAbility, Hero hero)
    {
        //foreach (Enemy enemy in MainQuestKeeperDisplay.SelectedQuest.Quest.Quest.EnemiesList)
        //{
        //    foreach (Ability enemyAbility in enemy.ListAbilities)
        //    {
        //        foreach (Debuff enemyDebuff in enemyAbility.DebuffsForAttack)
        //        {
        //            foreach (Debuff heroDebuffNeutralize in heroAbility.DebuffsForDefence)
        //            {
        //                if (heroDebuffNeutralize.DebuffData.DebuffList == enemyDebuff.DebuffData.DebuffList)
        //                {
        //                    if (heroAbility.AbilityData.TypeAbility == TypeAbilities.Aura)
        //                    {
        //                        foreach (HeroQuestSlot_UI heroSlot in MainQuestKeeperDisplay.HeroesSlots.Slots)
        //                        {
        //                            if (heroSlot.Hero != null)
        //                            {
        //                                if (!heroSlot.Hero.NeutralizeDebuffs.Contains(enemyDebuff.DebuffData.DebuffList))
        //                                    enemyDebuff.DebuffData.ApplyDebuffEffect(MainQuestKeeperDisplay.SelectedQuest.Quest.Quest, heroSlot.Hero);
        //                            }
        //                        }
        //                    }

        //                    else
        //                        if (!hero.NeutralizeDebuffs.Contains(enemyDebuff.DebuffData.DebuffList))
        //                        enemyDebuff.DebuffData.ApplyDebuffEffect(MainQuestKeeperDisplay.SelectedQuest.Quest.Quest, hero);
        //                }
        //            }
        //        }
        //    }
        //}
    }

    public void CancelDebuffByTakeHeroAbility(Ability heroAbility, Hero hero)
    {
        //if (heroAbility.DebuffsForDefence.Count != 0)
        //{
        //    foreach (Enemy enemy in MainQuestKeeperDisplay.SelectedQuest.Quest.Quest.EnemiesList)
        //    {
        //        foreach (Ability enemyAbility in enemy.ListAbilities)
        //        {
        //            foreach (Debuff enemyDebuff in enemyAbility.DebuffsForAttack)
        //            {
        //                foreach (Debuff heroDebuffNeutralize in heroAbility.DebuffsForDefence)
        //                {
        //                    if (heroDebuffNeutralize.DebuffData.DebuffList == enemyDebuff.DebuffData.DebuffList)
        //                    {
        //                        if (heroAbility.AbilityData.TypeAbility == TypeAbilities.Aura)
        //                        {
        //                            foreach (HeroQuestSlot_UI heroSlot in MainQuestKeeperDisplay.HeroesSlots.Slots)
        //                            {
        //                                if (heroSlot.Hero != null)
        //                                {
        //                                    int sameDebuffs = 0;

        //                                    foreach (DebuffList neutralizedHeroDebuffs in heroSlot.Hero.NeutralizeDebuffs)
        //                                    {
        //                                        sameDebuffs++;
        //                                    }

        //                                    if (sameDebuffs == 1)
        //                                    {
        //                                        enemyDebuff.DebuffData.CancelDebuffEffect(MainQuestKeeperDisplay.SelectedQuest.Quest.Quest, heroSlot.Hero);
        //                                    }
        //                                }
        //                            }
        //                        }

        //                        else
        //                        {
        //                            int sameDebuffs = 0;

        //                            foreach (DebuffList neutralizedHeroDebuffs in hero.NeutralizeDebuffs)
        //                            {
        //                                sameDebuffs++;
        //                            }

        //                            if (sameDebuffs == 1)
        //                            {
        //                                enemyDebuff.DebuffData.CancelDebuffEffect(MainQuestKeeperDisplay.SelectedQuest.Quest.Quest, hero);
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
    }

    public void ApplyDebuffEffectByDeleteHeroWithAura()
    {

    }

    public void CancelDebuffEffectByDeleteHeroWithAura(Hero hero)
    {
        //if (MainQuestKeeperDisplay.SelectedQuest != null)
        //{
        //    foreach (Enemy enemy in MainQuestKeeperDisplay.SelectedQuest.Quest.Quest.EnemiesList)
        //    {
        //        foreach (Ability enemyAbility in enemy.ListAbilities)
        //        {
        //            foreach (Debuff debuff in enemyAbility.DebuffsForAttack)
        //            {
        //                if (!hero.NeutralizeDebuffs.Contains(debuff.DebuffData.DebuffList))
        //                    debuff.DebuffData.CancelDebuffEffect(MainQuestKeeperDisplay.SelectedQuest.Quest.Quest, hero);
        //            }
        //        }
        //    }
        //}
    }

    public void ApplyDebuffEffect(Hero hero)
    {
        //if (MainQuestKeeperDisplay.SelectedQuest != null)
        //{
        //    foreach (Enemy enemy in MainQuestKeeperDisplay.SelectedQuest.Quest.Quest.EnemiesList)
        //    {
        //        foreach (Ability enemyAbility in enemy.ListAbilities)
        //        {
        //            foreach (Debuff debuff in enemyAbility.DebuffsForAttack)
        //            {
        //                if (!hero.NeutralizeDebuffs.Contains(debuff.DebuffData.DebuffList))
        //                    debuff.DebuffData.ApplyDebuffEffect(MainQuestKeeperDisplay.SelectedQuest.Quest.Quest, hero);
        //            }
        //        }
        //    }
        //}
    }

    public void ApplyDebuffEffect(Quest quest, Hero hero)
    {
        //foreach (Enemy enemy in quest.EnemiesList)
        //{
        //    foreach (Ability enemyAbility in enemy.ListAbilities)
        //    {
        //        foreach (Debuff debuff in enemyAbility.DebuffsForAttack)
        //        {
        //            if (hero == null)
        //            {
        //                for (int i = 0; i < MainQuestKeeperDisplay.HeroesSlots.Slots.Count; i++)
        //                {
        //                    if (MainQuestKeeperDisplay.HeroesSlots.Slots[i] != null && MainQuestKeeperDisplay.HeroesSlots.Slots[i].Hero != null)
        //                    {
        //                        if (!MainQuestKeeperDisplay.HeroesSlots.Slots[i].Hero.NeutralizeDebuffs.Contains(debuff.DebuffData.DebuffList))
        //                            debuff.DebuffData.ApplyDebuffEffect(quest, MainQuestKeeperDisplay.HeroesSlots.Slots[i].Hero);
        //                    }
        //                }
        //            }

        //            else
        //            {
        //                if (!hero.NeutralizeDebuffs.Contains(debuff.DebuffData.DebuffList))
        //                    debuff.DebuffData.ApplyDebuffEffect(quest, hero);
        //            }
        //        }
        //    }
        //}
    }

    public void CancelDebuffEffect(Hero hero)
    {
        //if (MainQuestKeeperDisplay.SelectedQuest != null)
        //{
        //    foreach (Enemy enemy in MainQuestKeeperDisplay.SelectedQuest.Quest.Quest.EnemiesList)
        //    {
        //        foreach (Ability enemyAbility in enemy.ListAbilities)
        //        {
        //            foreach (Debuff debuff in enemyAbility.DebuffsForAttack)
        //            {
        //                if (hero.NeutralizeDebuffs.Contains(debuff.DebuffData.DebuffList))
        //                    debuff.DebuffData.CancelDebuffEffect(MainQuestKeeperDisplay.SelectedQuest.Quest.Quest, hero);
        //            }
        //        }
        //    }
        //}
    }

    public void CancelDebuffEffect(Quest quest, Hero hero)
    {
        //foreach (Enemy enemy in quest.EnemiesList)
        //{
        //    foreach (Ability enemyAbility in enemy.ListAbilities)
        //    {
        //        foreach (Debuff debuff in enemyAbility.DebuffsForAttack)
        //        {
        //            if (hero == null)
        //            {
        //                for (int i = 0; i < MainQuestKeeperDisplay.HeroesSlots.Slots.Count; i++)
        //                {
        //                    if (MainQuestKeeperDisplay.HeroesSlots.Slots[i] != null && MainQuestKeeperDisplay.HeroesSlots.Slots[i].Hero != null)
        //                    {
        //                        if (!MainQuestKeeperDisplay.HeroesSlots.Slots[i].Hero.NeutralizeDebuffs.Contains(debuff.DebuffData.DebuffList))
        //                            debuff.DebuffData.CancelDebuffEffect(quest, MainQuestKeeperDisplay.HeroesSlots.Slots[i].Hero);
        //                    }
        //                }
        //            }

        //            else
        //            {
        //                if (!hero.NeutralizeDebuffs.Contains(debuff.DebuffData.DebuffList))
        //                    debuff.DebuffData.CancelDebuffEffect(quest, hero);
        //            }
        //        }
        //    }
        //}
    }

    public void DecreaseHeroResistance(Quest quest, bool applyEffect, Hero hero)
    {
        //Quest activeQuest;

        //if (quest != null)
        //    activeQuest = quest;

        //else
        //    activeQuest = new Quest(_selectedSlot.Quest.Quest.QuestData, _selectedSlot.Quest.Quest.RegionData);

        //foreach (Enemy enemy in activeQuest.EnemiesList)
        //{
        //    foreach (Ability enemyAbility in enemy.ListAbilities)
        //    {
        //        foreach (Resistance res in enemyAbility.ResistancesForDecrease)
        //        {
        //            if (hero == null)
        //            {
        //                foreach (HeroQuestSlot_UI heroSlot in _heroes.Slots)
        //                {
        //                    if (heroSlot != null && heroSlot.Hero != null && heroSlot.Hero.HeroData != null)
        //                    {
        //                        Resistance matchingHeroRes = heroSlot.Hero.Resistances.Find(resHero => resHero.TypeDamage == res.TypeDamage);

        //                        if (matchingHeroRes != null)
        //                        {
        //                            if (applyEffect)
        //                                matchingHeroRes.ValueResistance += res.ValueResistance;

        //                            else
        //                                matchingHeroRes.ValueResistance -= res.ValueResistance;
        //                        }
        //                    }
        //                }
        //            }

        //            else
        //            {
        //                Resistance matchingHeroRes = hero.Resistances.Find(resHero => resHero.TypeDamage == res.TypeDamage);

        //                if (matchingHeroRes != null)
        //                {
        //                    if (applyEffect)
        //                        matchingHeroRes.ValueResistance += res.ValueResistance;

        //                    else
        //                        matchingHeroRes.ValueResistance -= res.ValueResistance;
        //                }
        //            }
        //        }
        //    }
        //}

        //CheckRequiresQuest();
    }

    public void ChangeEnemyResistance(Quest quest, bool applyEffect)
    {
        //foreach (HeroQuestSlot_UI heroSlot in MainQuestKeeperDisplay.HeroesSlots.Slots)
        //{
        //    if (heroSlot != null && heroSlot.Hero != null && heroSlot.Hero.HeroData != null)
        //    {
        //        foreach (Ability heroAbility in heroSlot.Hero.AbilityHolder.HeroAbilitySystem.AbilityList)
        //        {
        //            if (heroAbility != null && heroAbility.AbilityData != null)
        //            {
        //                foreach (Resistance decreaseRes in heroAbility.ResistancesForDecrease)
        //                {
        //                    foreach (Enemy enemy in quest.EnemiesList)
        //                    {
        //                        Resistance existingResist = enemy.Resistances.Find(r => r.TypeDamage == decreaseRes.TypeDamage);

        //                        if (existingResist != null)
        //                        {
        //                            if (applyEffect)
        //                                // Если резист уже есть, увеличиваем его значение
        //                                existingResist.ValueResistance += decreaseRes.ValueResistance;

        //                            else
        //                            {
        //                                existingResist.ValueResistance -= decreaseRes.ValueResistance;

        //                                if (existingResist.ValueResistance == 0)
        //                                    enemy.Resistances.Remove(existingResist);
        //                            }
        //                        }
        //                        else
        //                        {
        //                            // Если резиста нет, создаем новый
        //                            Resistance newEnemyRes = new Resistance(decreaseRes);
        //                            newEnemyRes.ValueResistance = 0;

        //                            enemy.Resistances.Add(newEnemyRes);

        //                            if (applyEffect)
        //                                newEnemyRes.ValueResistance += decreaseRes.ValueResistance;

        //                            else
        //                            {
        //                                newEnemyRes.ValueResistance -= decreaseRes.ValueResistance;

        //                                if (newEnemyRes.ValueResistance == 0)
        //                                    enemy.Resistances.Remove(newEnemyRes);
        //                            }
        //                        }
        //                    }
        //                }
        //            }

        //            CheckRequiresQuest();
        //        }
        //    }
        //}
    }

    public void ChangeEnemyResistance(Quest quest, Ability heroAbility, bool applyEffect)
    {
        //foreach (Resistance decreaseRes in heroAbility.ResistancesForDecrease)
        //{
        //    foreach (Enemy enemy in quest.EnemiesList)
        //    {
        //        Resistance existingResist = enemy.Resistances.Find(r => r.TypeDamage == decreaseRes.TypeDamage);

        //        if (existingResist != null)
        //        {
        //            if (applyEffect)
        //                // Если резист уже есть, увеличиваем его значение
        //                existingResist.ValueResistance += decreaseRes.ValueResistance;

        //            else
        //            {
        //                existingResist.ValueResistance -= decreaseRes.ValueResistance;

        //                if (existingResist.ValueResistance == 0)
        //                    enemy.Resistances.Remove(existingResist);
        //            }
        //        }
        //        else
        //        {
        //            // Если резиста нет, создаем новый
        //            Resistance newEnemyRes = new Resistance(decreaseRes);
        //            newEnemyRes.ValueResistance = 0;

        //            enemy.Resistances.Add(newEnemyRes);

        //            if (applyEffect)
        //                newEnemyRes.ValueResistance += decreaseRes.ValueResistance;

        //            else
        //            {
        //                newEnemyRes.ValueResistance -= decreaseRes.ValueResistance;

        //                if (newEnemyRes.ValueResistance == 0)
        //                    enemy.Resistances.Remove(newEnemyRes);
        //            }
        //        }
        //    }
        //}
    }

    private void UpdateResistanceUI(List<TypeDamage> resistance, QuestSlot_UI questSlot)
    {
        //int level = questSlot.Quest.Quest.Level;

        //// Проверяем каждый резист в списке и устанавливаем значение 10, если он есть
        //foreach (TypeDamage res in resistance)
        //{
        //    switch (res)
        //    {
        //        case TypeDamage.Fire:
        //            _fireRes = level * 10;
        //            break;
        //        case TypeDamage.Cold:
        //            _coldRes = level * 10;
        //            break;
        //        case TypeDamage.Lightning:
        //            _lightRes = level * 10;
        //            break;
        //        case TypeDamage.Necrotic:
        //            _necroticRes = level * 10;
        //            break;
        //        case TypeDamage.Dark:
        //            _voidRes = level * 10;
        //            break;
        //        default:
        //            break;
        //    }
        //}

        //QuestParametres.ShowAllAbilitiesRes(resistance, _coefLevelRes * level);
    }

    private float CheckDebuffs()
    {
        List<Enemy> enemiesList = MainQuestKeeperDisplay.SelectedQuest.Quest.Quest.EnemiesList;
        List<DebuffData> debuffDataList = new List<DebuffData>();
        _debuffList = new List<Debuff>();
        _buffList = new List<BuffList>();

        foreach (Enemy enemy in enemiesList)
        {
            foreach (Ability ability in enemy.ListAbilities)
            {
                AddDebuffList(ability, debuffDataList);
                AddBuffList(ability);
            }
        }

        //_applyDebuffsCount = 0;
        int heroCount = 0;
        int debuffsCount = 0;
        List<Hero> heroes = new List<Hero>();

        foreach (Debuff debuff in _debuffList)
        {
            debuffsCount++;
        }

        foreach (HeroQuestSlot_UI heroSlot in _heroes.Slots)
        {
            Hero hero = heroSlot.Hero;

            if (hero == null || hero.HeroData == null)
            {
                continue;
            }

            heroCount++;
            heroes.Add(hero);
        }

        _debuffApply = new DebuffApplyingEffect();
        int applyingDebuffs = _debuffApply.CheckDebuffs(_debuffList, heroes);
        int applyingBuffs = _debuffApply.CheckBuffs(_buffList, heroes);

        if (heroes.Count == 0)
            return 0;
        if (_debuffList.Count == 0)
            return 100;

        else
            _chanceDebuffsComplete = 100 - ((applyingDebuffs + applyingBuffs) * 100 / (_debuffList.Count + _buffList.Count) / heroes.Count);

        return _chanceDebuffsComplete;
    }

    private float CheckAffixes()
    {
        List<Affix> affixesList = MainQuestKeeperDisplay.SelectedQuest.Quest.Quest.AffixesList;

        if (affixesList.Count == 0)
            return 100;

        List<Hero> heroes = new List<Hero>();

        List<Affix> unConsiderAffixes = new List<Affix>();
        int heroCount = 0;

        foreach (HeroQuestSlot_UI heroSlot in _heroes.Slots)
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

        _chanceAffixesComplete = 100 - (applyingAffixes * 100 / (affixesList.Count - unConsiderAffixes.Count) / heroes.Count);

        return _chanceAffixesComplete;
    }

    private void CheckAbilitiesHero(QuestSlot_UI questSlot)
    {
        _calculateAbilitiesSystem.RecalculateAttackAbilities(_heroes, questSlot);
    }

    private float CalculateHeroAmountFood()
    {
        SendHeroesSlots heroes = MainQuestKeeperDisplay.HeroesSlots;
        QuestSlot_UI questSlot = MainQuestKeeperDisplay.SelectedQuest;
        _totalRequiredFood = questSlot.Quest.Quest.AmountDaysToComplete * heroes.AmountActiveHeroes; // Общая потребность в еде

        _totalAvailableFood = 0;

        foreach (HeroQuestSlot_UI heroSlot in heroes.Slots)
        {
            if (heroSlot != null && heroSlot.Hero != null && heroSlot.Hero.HeroData != null)
            {
                float localAmountFood = 0;

                foreach (EquipSlot equipSlot in heroSlot.Hero.EquipHolder.EquipSystem.Slots)
                {
                    if (equipSlot.EquipItemData is ConsumableItemData consumItem &&
                        consumItem.TypeConsumItem == TypeConsumableItem.Food)
                    {
                        localAmountFood += equipSlot.StackSize * consumItem.ItemValue; // Предположим, что есть свойство Amount
                    }
                }

                // Если герой имеет больше еды, чем ему нужно, учитываем только необходимое количество
                float heroRequiredFood = Mathf.Min(questSlot.Quest.Quest.AmountDaysToComplete, localAmountFood);
                _totalAvailableFood += heroRequiredFood;
            }
        }

        if (_totalAvailableFood >= _totalRequiredFood)
        {
            //Debug.Log("У героев достаточно еды для задания.");
        }
        else
        {
            //Debug.Log($"Не хватает еды для задания. Нужно ещё {totalRequiredFood - totalAvailableFood} еды.");
        }

        QuestParametres.UpdateFoodInfoUI(_totalRequiredFood, _totalAvailableFood);

        float chanceFood = 0;

        if (_totalRequiredFood > 0)
        {
            chanceFood = (float)_totalAvailableFood / _totalRequiredFood * 100;
        }

        return chanceFood;
    }

    private float CalculateHeroAmountLight()
    {
        //SendHeroesSlots heroes = MainQuestKeeperDisplay.HeroesSlots;
        //QuestSlot_UI questSlot = MainQuestKeeperDisplay.SelectedQuest;
        //_totalRequiredLight = questSlot.Quest.Quest.AmountDaysToComplete * heroes.AmountActiveHeroes; // Общая потребность в свете

        //_totalAvailableLight = 0;

        //foreach (HeroQuestSlot_UI heroSlot in heroes.Slots)
        //{
        //    if (heroSlot != null && heroSlot.Hero != null && heroSlot.Hero.HeroData != null)
        //    {
        //        //bool hasLightBuff = heroSlot.Hero.ApplyingBuffs.Contains(BuffList.Light);
        //        bool hasLightBuff = false;
        //        int localAmountLight = 0;

        //        if (hasLightBuff)
        //        {
        //            _totalAvailableLight += questSlot.Quest.Quest.AmountDaysToComplete;
        //        }
        //        else
        //        {
        //            foreach (EquipSlot equipSlot in heroSlot.Hero.EquipHolder.EquipSystem.Slots)
        //            {
        //                if (equipSlot.EquipItemData is ConsumableItemData consumItem &&
        //                    consumItem.TypeConsumItem == TypeConsumableItem.Light)
        //                {
        //                    localAmountLight += equipSlot.StackSize; // Предполагается, что есть свойство StackSize
        //                }
        //            }

        //            // Если герой имеет больше света, чем ему нужно, учитываем только необходимое количество
        //            int heroRequiredLight = Mathf.Min(questSlot.Quest.Quest.AmountDaysToComplete, localAmountLight);
        //            _totalAvailableLight += heroRequiredLight;
        //        }
        //    }
        //}

        //if (_totalAvailableLight >= _totalRequiredLight)
        //{
        //    //Debug.Log("У героев достаточно света для задания.");
        //}
        //else
        //{
        //    //Debug.Log($"Не хватает света для задания. Нужно ещё {totalRequiredLight - totalAvailableLight} света.");
        //}

        //QuestParametres.UpdateLightInfoUI(_totalRequiredLight, _totalAvailableLight);

        //float chanceLight = 0;

        //if (_totalRequiredLight > 0)
        //{
        //    chanceLight = (float)_totalAvailableLight / _totalRequiredLight * 100;
        //}

        //return chanceLight;
        return 0;
    }

    private float CalculateHeroAmountHeal()
    {
        return 0;
        //SendHeroesSlots heroes = MainQuestKeeperDisplay.HeroesSlots;
        //QuestSlot_UI questSlot = MainQuestKeeperDisplay.SelectedQuest;
        //float totalRequiredHeal = 0;
        //_totalAvailableHeal = 0;
        //_totalRequiredHeal = 0;

        //List<float> availableHeals = new List<float>();

        //foreach (HeroQuestSlot_UI heroSlot in heroes.Slots)
        //{
        //    if (heroSlot != null && heroSlot.Hero != null && heroSlot.Hero.HeroData != null)
        //    {
        //        float heroRequiredHeal = heroSlot.Hero.HeroStats.Health * _healCoef;
        //        totalRequiredHeal += heroRequiredHeal;
        //        float localAmountHeal = 0;

        //        // Подсчёт лечения от предметов
        //        foreach (EquipSlot equipSlot in heroSlot.Hero.EquipHolder.EquipSystem.Slots)
        //        {
        //            if (equipSlot.EquipItemData is ConsumableItemData consumItem &&
        //                consumItem.TypeConsumItem == TypeConsumableItem.Heal)
        //            {
        //                localAmountHeal += consumItem.ItemValue * equipSlot.StackSize;
        //            }
        //        }

        //        // Подсчёт лечения от способностей
        //        foreach (Ability ability in heroSlot.Hero.AbilityHolder.HeroAbilitySystem.AbilityList)
        //        {
        //            if (ability != null && ability.AbilityData != null)
        //                localAmountHeal += ability.AbilityData.HealValue;
        //        }

        //        // Учитываем только то количество лечения, которое нужно конкретному герою
        //        float actualHealForHero = Mathf.Min(heroRequiredHeal, localAmountHeal);
        //        availableHeals.Add(actualHealForHero);
        //    }
        //}

        //// Складываем всё лечение, доступное для всех героев
        //_totalAvailableHeal = availableHeals.Sum();

        //if (_totalAvailableHeal >= totalRequiredHeal)
        //{
        //    //Debug.Log("У героев достаточно лечения для задания.");
        //}
        //else
        //{
        //    //Debug.Log($"Не хватает лечения для задания. Нужно ещё {totalRequiredHeal - totalAvailableHeal} лечения.");
        //}

        //if (_healCoef == 1)
        //    _totalRequiredHeal = totalRequiredHeal;

        //else
        //    _totalRequiredHeal = totalRequiredHeal;

        //QuestParametres.UpdateHealInfoUI(_totalRequiredHeal, _totalAvailableHeal);

        //float chanceHeal = 0;

        //if (_totalRequiredHeal != 0)
        //    chanceHeal = (float)_totalAvailableHeal / _totalRequiredHeal * 100;

        //return chanceHeal;
    }

    private float CalculateHeroAmountMana()
    {
        return 0;
        //SendHeroesSlots heroes = MainQuestKeeperDisplay.HeroesSlots;
        //QuestSlot_UI questSlot = MainQuestKeeperDisplay.SelectedQuest;
        //float totalRequiredMana = 0;
        //_totalAvailableMana = 0;
        //_totalRequiredMana = 0;

        //List<float> availableMana = new List<float>();

        //foreach (HeroQuestSlot_UI heroSlot in heroes.Slots)
        //{
        //    if (heroSlot != null && heroSlot.Hero != null && heroSlot.Hero.HeroData != null)
        //    {
        //        float heroRequiredMana = 0;
        //        float localAmountMana = 0;

        //        // Подсчёт требуемой маны для каждого героя (суммируем стоимость всех способностей героя)
        //        foreach (Ability ability in heroSlot.Hero.AbilityHolder.HeroAbilitySystem.AbilityList)
        //        {
        //            if (ability != null && ability.AbilityData != null)
        //                heroRequiredMana += ability.ManaCost * _manaCoef;  // Предполагаем, что есть свойство ManaCost у AbilityData
        //        }

        //        totalRequiredMana += heroRequiredMana;

        //        // Подсчёт доступной маны из экипировки (если есть предметы, дающие ману)
        //        foreach (EquipSlot equipSlot in heroSlot.Hero.EquipHolder.EquipSystem.Slots)
        //        {
        //            if (equipSlot.EquipItemData is ConsumableItemData consumItem &&
        //                consumItem.TypeConsumItem == TypeConsumableItem.Mana)
        //            {
        //                localAmountMana += consumItem.ItemValue * equipSlot.StackSize;
        //            }
        //        }

        //        // Учитываем только то количество маны, которое требуется конкретному герою
        //        float actualManaForHero = Mathf.Min(heroRequiredMana, localAmountMana);
        //        availableMana.Add(actualManaForHero);
        //    }
        //}

        //// Складываем всё доступное количество маны
        //_totalAvailableMana = availableMana.Sum();

        //if (_totalAvailableMana >= totalRequiredMana)
        //{
        //    //Debug.Log("У героев достаточно маны для задания.");
        //}
        //else
        //{
        //    //Debug.Log($"Не хватает маны для задания. Нужно ещё {totalRequiredMana - totalAvailableMana} маны.");
        //}

        //if (_manaCoef == 1)
        //    _totalRequiredMana = totalRequiredMana;

        //else
        //    _totalRequiredMana = totalRequiredMana;

        //QuestParametres.UpdateManaInfoUI(_totalRequiredMana, _totalAvailableMana); // Обновление UI для маны

        //float chanceMana = 0;

        //if (_totalRequiredMana != 0)
        //    chanceMana = (float)_totalAvailableMana / _totalRequiredMana * 100;

        //return chanceMana;
    }

    public void ChangeHealCoef(float healCoef)
    {
        //if (healCoef > 0)
        //    _healCoef += healCoef;

        //else
        //    _healCoef = 1;

        //CalculateHeroAmountHeal();
    }

    public void ChangeManaCoef(float manaCoef)
    {
        //if (manaCoef > 0)
        //    _manaCoef += manaCoef;

        //else
        //    _manaCoef = 1;

        //CalculateHeroAmountMana();
    }


    public void ChangeLightCoef(float lightCoef)
    {
        //if (lightCoef > 0)
        //    _lightCoef += lightCoef;

        //else
        //    _lightCoef = 1;

        //CalculateHeroAmountLight();
    }

    private void AddDebuffList(Ability ability, List<DebuffData> debuffDataList)
    {
        //foreach (Debuff debuff in ability.DebuffsForAttack)
        //{
        //    if (debuffDataList.Contains(debuff.DebuffData))
        //        continue;

        //    else
        //    {
        //        debuffDataList.Add(debuff.DebuffData);
        //        _debuffList.Add(debuff);
        //    }
        //}
    }

    private void AddBuffList(Ability ability)
    {
        //foreach (BuffList buff in ability.Buffs)
        //{
        //    if (!_buffList.Contains(buff))
        //        _buffList.Add(buff);
        //}
    }

    public void ApplyEnemyAbilityEffect(QuestSlot_UI questSlot)
    {
        //QuestSlot_UI lastQuestSlot = MainQuestKeeperDisplay.LastSelectedQuest;
        //List<Hero> heroList = new List<Hero>();

        //foreach (HeroQuestSlot_UI heroSlot in MainQuestKeeperDisplay.HeroesSlots.Slots)
        //{
        //    if (heroSlot.Hero != null && heroSlot.Hero.HeroData != null)
        //        heroList.Add(heroSlot.Hero);
        //}

        //if (lastQuestSlot != null && lastQuestSlot.Quest != null && lastQuestSlot.Quest.Quest != null)
        //{
        //    foreach (Enemy enemy in lastQuestSlot.Quest.Quest.EnemiesList)
        //    {
        //        foreach (Ability enemyAbility in enemy.ListAbilities)
        //        {
        //            if (enemyAbility.AbilityData.UniqAbility)
        //                enemyAbility.DisActivateEnemyAbilities(lastQuestSlot.Quest.Quest, enemy, heroList, enemyAbility, this);
        //        }
        //    }
        //}

        //foreach (Enemy enemy in questSlot.Quest.Quest.EnemiesList)
        //{
        //    foreach (Ability enemyAbility in enemy.ListAbilities)
        //    {
        //        enemyAbility.ActivateEnemyAbilities(questSlot.Quest.Quest, enemy, heroList, enemyAbility, this);
        //    }
        //}
    }

    private void ClearAbilitiesEffect()
    {
        _calculateAbilitiesSystem.ClearAuraList();
    }

    //private DebuffApplyingEffect CreateDebuffEffect(DebuffList debuffList)
    //{
    //    switch (debuffList)
    //    {
    //        case DebuffList.None:
    //            return null;
    //        case DebuffList.Bleeding:
    //            return new DebuffBleed();
    //        case DebuffList.Poisoning:
    //            return new DebuffPoison(); // пример, создайте этот класс
    //        case DebuffList.Freeze:
    //            return new DebuffFreeze(); // пример, создайте этот класс
    //        case DebuffList.Burn:
    //            return new DebuffBurn(); // пример, создайте этот класс
    //        default:
    //            return null;
    //    }
    //}

}
