using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class Quest
{
    [SerializeField] protected QuestData _questData;
    [SerializeField] protected RegionData _regionData;

    [SerializeField] protected int _level;
    [Header("Power")]
    [SerializeField] protected int _minPower;
    [SerializeField] protected int _maxPower;
    [SerializeField] protected int _power;
    [Header("Defence")]
    [SerializeField] protected int _minDefence;
    [SerializeField] protected int _maxDefence;
    [SerializeField] protected int _defence;
    [Header("Resistance")]
    [SerializeField] protected List<Resistance> _questResistance = new List<Resistance>();
    [Header("Region and Days To Complete")]
    [SerializeField] protected RegionQuest _regionQuest;
    [SerializeField] protected int _minAmountDaysToComplete;
    [SerializeField] protected int _maxAmountDaysToComplete;
    [SerializeField] protected int _amountDaysToComplete;
    [Header("Enemy")]
    [SerializeField] protected int _minAmountEnemy;
    [SerializeField] protected int _maxAmountEnemy;
    [SerializeField] protected int _randomAmountEnemy;
    [SerializeField] protected List<Enemy> _enemiesList;
    [Header("Affix")]
    [SerializeField] protected int _minAmountAffixes;
    [SerializeField] protected int _maxAmountAffixes;
    [SerializeField] protected int _randomAmountAffixes;
    [SerializeField] protected List<Affix> _affixesList;
    [Header("Send Main Parametres")]
    [SerializeField] protected float _chanceToCompleteQuest;
    [SerializeField] protected float _chanceSupplies;
    [SerializeField] protected float _chanceCombat;
    [Header("Send List Parametres")]
    [SerializeField] protected Dictionary<int, Hero> _visibleHeroesSlots = new Dictionary<int, Hero>(); // ДЛЯ ОТОБРАЖЕНИЯ В ДИСПЛЕЕ
    [SerializeField] protected List<Hero> _testHeroesSlots = new List<Hero>(); // ГЕРОИ ДЛЯ просмотра в словаре выше
    public List<Hero> TestHeroesSlots => _testHeroesSlots;
    [SerializeField] protected List<Hero> _heroesSlots = new List<Hero>(); // ГЕРОИ ДЛЯ РАСЧЕТА РЕЗУЛЬТАТА КВЕСТА
    [SerializeField] protected float _powerHeroes;
    [SerializeField] protected float _defenceHeroes;
    [SerializeField] protected Dictionary<float, float> _foodAmount = new Dictionary<float, float>();
    [SerializeField] protected Dictionary<float, float> _lightAmount = new Dictionary<float, float>();
    [SerializeField] protected Dictionary<float, float> _healAmount = new Dictionary<float, float>();
    [SerializeField] protected Dictionary<float, float> _manaAmount = new Dictionary<float, float>();
    [SerializeField] protected List<int> _questResistanceHollow = new List<int>();
    [SerializeField] protected float _coefLight;
    [SerializeField] protected float _coefHeal;
    [SerializeField] protected float _coefMana;
    [Header("Reward Parametres")]
    [SerializeField] protected int _minRewardGuildReputation;
    [SerializeField] protected int _maxRewardGuildReputation;
    [SerializeField] protected int _rewardGuildReputation;
    [SerializeField] protected int _minRewardGold;
    [SerializeField] protected int _maxRewardGold;
    [SerializeField] protected int _rewardGold;
    [SerializeField] protected int _minRewardHeroExp;
    [SerializeField] protected int _maxRewardHeroExp;
    [SerializeField] protected int _rewardHeroExp;
    [SerializeField] protected List<RewardItems> _rewardItems = new List<RewardItems>();
    [Header("Save Reward Parametres")]
    [SerializeField] protected int _guildCurrentRep;
    [SerializeField] protected int _guildRequiredRep;
    [SerializeField] protected bool _upLevelGuild = false;
    [SerializeField] protected List<int> _heroesCurrentExp = new List<int>();
    [SerializeField] protected List<int> _heroesRequiredExp = new List<int>();
    [SerializeField] protected List<bool> _upLevelHeroes = new List<bool>();

    public QuestData QuestData => _questData;
    public int Level => _level;
    public int MinPower => _minPower;
    public int MaxPower => _maxPower;
    public int Power => _power;
    public int MinDefence => _minDefence;
    public int MaxDefence => _maxDefence;
    public int Defence => _defence;
    public int MinAmountEnemy => _minAmountEnemy;
    public int MaxAmountEnemy => _maxAmountEnemy;
    public float ChanceToComplete => _chanceToCompleteQuest;
    public float ChanceSupplies => _chanceSupplies;
    public float ChanceCombat => _chanceCombat;
    public RegionQuest RegionQuest => _regionQuest;
    public RegionData RegionData => _regionData;
    public int AmountDaysToComplete => _amountDaysToComplete;
    public List<Enemy> EnemiesList => _enemiesList;
    public List<Affix> AffixesList => _affixesList;
    public int RandomAmountAffixes => _randomAmountAffixes;
    public Dictionary<int, Hero> VisibleHeroesSlots => _visibleHeroesSlots;
    public List<Hero> HeroesSlots => _heroesSlots;
    public float PowerHeroes => _powerHeroes;
    public float DefenceHeroes => _defenceHeroes;
    public List<Resistance> QuestResistance => _questResistance;
    public Dictionary<float, float> FoodAmount => _foodAmount;
    public Dictionary<float, float> LightAmount => _lightAmount;
    public Dictionary<float, float> HealAmount => _healAmount;
    public Dictionary<float, float> ManaAmount => _manaAmount;
    public float CoefLight => _coefLight;
    public float CoefHeal => _coefHeal;
    public float CoefMana => _coefMana;
    public List<int> QuestResistanceHollow => _questResistanceHollow;
    public List<RewardItems> RewardItems => _rewardItems;
    public int RewardGuildReputation => _rewardGuildReputation;
    public int MinRewardGuildReputation => _minRewardGuildReputation;
    public int MaxRewardGuildReputation => _maxRewardGuildReputation;
    public int RewardGold => _rewardGold;
    public int MinRewardGold => _minRewardGold;
    public int MaxRewardGold => _maxRewardGold;
    public int RewardHeroExp => _rewardHeroExp;
    public int MinRewardHeroExp => _minRewardHeroExp;
    public int MaxRewardHeroExp => _maxRewardHeroExp;
    public int GuildCurrentRep => _guildCurrentRep;
    public int GuildRequiredRep => _guildRequiredRep;
    public bool UpLevelGuild => _upLevelGuild;
    public List<int> HeroesCurrentExp => _heroesCurrentExp;
    public List<int> HeroesRequiredExp => _heroesRequiredExp;
    public List<bool> UpLevelHeroes => _upLevelHeroes;

    public int RemainDaysToComplete;
    public bool IsQuestToSuccess = false;
    public bool IsQuestToChecked = false;
    public bool IsQuestSeen = false;
    public List<RewardItems> SaveRewardItems = new List<RewardItems>();
    public List<BlankSlot> SaveRewardBlanks = new List<BlankSlot>();

    public Quest(QuestData questData, RegionData regionData)
    {
        _questData = questData;
        _regionData = regionData;

        _level = questData.Level;
        _minPower = questData.MinPower;
        _maxPower = questData.MaxPower;
        _minDefence = questData.MinDefence;
        _maxDefence = questData.MaxDefence;

        _minRewardGuildReputation = questData.MinRewardGuildReputation;
        _maxRewardGuildReputation = questData.MaxRewardGuildReputation;

        _minRewardGold = questData.MinRewardGold;
        _maxRewardGold = questData.MaxRewardGold;

        _minRewardHeroExp = questData.MinRewardHeroExp;
        _maxRewardHeroExp = questData.MaxRewardHeroExp;

        _minAmountEnemy = questData.MinAmountEnemy;
        _maxAmountEnemy = questData.MaxAmountEnemy;

        _minAmountAffixes = questData.MinAmountAffixes;
        _maxAmountAffixes = questData.MaxAmountAffixes;

        _minAmountDaysToComplete = questData.MinAmountDays;
        _maxAmountDaysToComplete = questData.MaxAmountDays;

        _regionQuest = regionData.RegionType;

        //_rewardItems = DeepCopyRewardItems(regionData.RewardRegion[questData.Level - 1].RewardItems);
        CopyRewardItems(regionData);

        _enemiesList = new List<Enemy>();
        //SetEnemy(regionData.EnemyDataList);
        SetEnemy(regionData.EnemyDataListq[Level - 1].EnemyDatas);

        _affixesList = new List<Affix>();

        SetRandomParametres();

        RemainDaysToComplete = _amountDaysToComplete;

        SetResistance();
    }

    public Quest(Quest copyQuest, QuestParametresSystemFix questSystem) // Копия для Взятия Квеста
    {
        _questData = copyQuest.QuestData;
        _regionData = copyQuest.RegionData;

        _level = copyQuest.QuestData.Level;
        _power = copyQuest.Power;
        _defence = copyQuest.Defence;

        _rewardGold = copyQuest.RewardGold;
        _rewardGuildReputation = copyQuest.RewardGuildReputation;
        _rewardHeroExp = copyQuest.RewardHeroExp;

        _enemiesList = new List<Enemy>();
        foreach (Enemy enemy in copyQuest.EnemiesList)
            _enemiesList.Add(enemy);

        _affixesList = new List<Affix>();
        foreach (Affix affix in copyQuest.AffixesList)
            _affixesList.Add(affix);

        _amountDaysToComplete = copyQuest.AmountDaysToComplete;

        _regionQuest = copyQuest.RegionQuest;

        CopyRewardItems(copyQuest.RegionData);

        RemainDaysToComplete = _amountDaysToComplete;

        _questResistance = new List<Resistance>();
        foreach (Resistance res in copyQuest.QuestResistance)
            _questResistance.Add(res);

        _foodAmount.Add(questSystem.TotalAvailableFood, questSystem.TotalRequiredFood);
        _lightAmount.Add(questSystem.TotalAvailableLight, questSystem.TotalRequiredLight);
        _healAmount.Add(questSystem.TotalAvailableHeal, questSystem.TotalRequiredHeal);
        _manaAmount.Add(questSystem.TotalAvailableMana, questSystem.TotalRequiredMana);

        _coefLight = questSystem.LightCoef;
        _coefHeal = questSystem.HealCoef;
        _coefMana = questSystem.ManaCoef;

        _powerHeroes = questSystem.TotalHeroesPower;
        _defenceHeroes = questSystem.TotalHeroesDefence;

        _chanceSupplies = questSystem.ChanceSupplies;
        _chanceCombat = questSystem.ChanceCombat;
        _chanceToCompleteQuest = questSystem.FullChanceComplete;
    }

    public void SetSendQuestParametres(float chanceToComplete, float chanceSupplies, float chanceCombat)
    {
        _chanceToCompleteQuest = chanceToComplete;
        _chanceSupplies = chanceSupplies;
        _chanceCombat = chanceCombat;
    }

    protected void SetResistance()
    {
        foreach (Enemy enemy in _enemiesList)
        {
            foreach (Ability enemyAbil in enemy.ListAbilities)
            {
                foreach (Resistance enemyRes in enemyAbil.ResistancesForAttack)
                {
                    Resistance currentRes = _questResistance.Find(r => r.TypeDamage == enemyRes.TypeDamage);

                    if (currentRes == null)
                    {
                        int randomValueRes = Random.Range(_questData.MinRes, _questData.MaxRes + 1);

                        _questResistance.Add(new Resistance(enemyRes.TypeDamage, randomValueRes));
                    }
                }
            }
        }

        foreach (WorldEventData worldEventData in NotificationSystem.Instance.WorldEventSystem.ActiveyDataEventsList)
        {
            if (worldEventData is EventQuestResistData eventResData)
            {
                foreach (TypeDamage eventRes in eventResData.eventQuestResists)
                {
                    int randomValueRes = Random.Range(this._questData.MinRes, this._questData.MaxRes + 1);
                    Resistance newRes = new Resistance(eventRes, randomValueRes);

                    bool alreadyExists = _questResistance.Any(res => res.TypeDamage == newRes.TypeDamage);

                    if (!alreadyExists)
                        _questResistance.Add(newRes);
                }
            }
        }
    }

    protected virtual void SetRandomParametres()
    {
        //int randomPower = UnityEngine.Random.Range(_minPower, _maxPower);
        //int randomDefence = UnityEngine.Random.Range(_minDefence, _maxDefence);

        _power = SetRandomValue(_minPower, _maxPower);
        _defence = SetRandomValue(_minDefence, _maxDefence);
        _rewardGuildReputation = SetRandomValue(_minRewardGuildReputation, _maxRewardGuildReputation);
        _rewardGold = SetRandomValue(_minRewardGold, _maxRewardGold);
        _rewardHeroExp = SetRandomValue(_minRewardHeroExp, _maxRewardHeroExp);
        _randomAmountAffixes = SetRandomValue(_minAmountAffixes, _maxAmountAffixes + 1);
        _amountDaysToComplete = SetRandomValue(_minAmountDaysToComplete, _maxAmountDaysToComplete + 1);
    }

    protected void SetEnemy(List<EnemyData> enemyDataList)
    {
        List<Enemy> localEnemyList = new List<Enemy>();

        foreach (EnemyData enemyData in enemyDataList)
        {
            Enemy newLocalEnemy = new Enemy(enemyData);
            localEnemyList.Add(newLocalEnemy);
        }

        _randomAmountEnemy = UnityEngine.Random.Range(_minAmountEnemy, _maxAmountEnemy);

        while (_enemiesList.Count < _randomAmountEnemy && localEnemyList.Count > 0)
        {
            //int randomEnemyIndex = UnityEngine.Random.Range(0, localEnemyList.Count);
            int randomEnemyIndex = SetRandomValue(0, localEnemyList.Count);
            Enemy newEnemy = localEnemyList[randomEnemyIndex];

            // Проверяем, есть ли уже враг с такими же данными
            bool enemyExists = _enemiesList.Exists(e => e.EnemyData == newEnemy.EnemyData);

            if (!enemyExists)
            {
                _enemiesList.Add(newEnemy);
            }
            else
            {
                // Если враг уже есть, убираем его из списка доступных
                localEnemyList.RemoveAt(randomEnemyIndex);
            }
        }

        ApplyEnemyAbilities();
    }

    public void SetCopyValues(int level, int power, int defence, List<Enemy> enemyList, List<Affix> affixList, int rewardGuildReputation,
        int rewardGold, int rewardHeroExp, int amountDaysToComplete)
    {
        _enemiesList.Clear();
        _affixesList.Clear();

        _level = level;
        _power = power;
        _defence = defence;
        _rewardGuildReputation = rewardGuildReputation;
        _rewardGold = rewardGold;
        _rewardHeroExp = rewardHeroExp;
        _amountDaysToComplete = amountDaysToComplete;

        foreach (Enemy enemy in enemyList)
            _enemiesList.Add(enemy);

        foreach (Affix affix in affixList)
            _affixesList.Add(affix);
    }

    private void ApplyEnemyAbilities()
    {
        foreach (Enemy enemy in _enemiesList)
        {
            foreach (Ability enemyAbility in enemy.ListAbilities)
            {
                //ApplyResistanceAbility(enemy, enemyAbility);
                //ApplyDebuffImmunAbility(enemy, enemyAbility);
                //ApplyBuffAbility(enemy, enemyAbility);
            }
        }
    }

    private void ApplyBuffAbility(Enemy enemy, Ability enemyAbility)
    {
        if (enemyAbility.Buffs.Count != 0)
        {
            if (enemyAbility.AbilityData.TypeAbility == TypeAbilities.Buff)
            {
                foreach (BuffList buff in enemyAbility.Buffs)
                {
                    enemy.BuffList.Add(buff);
                }
            }

            else if (enemyAbility.AbilityData.TypeAbility == TypeAbilities.Aura)
            {
                foreach (Enemy enemyForBuffEffect in _enemiesList)
                {
                    foreach (BuffList buff in enemyAbility.Buffs)
                    {
                        enemyForBuffEffect.BuffList.Add(buff);
                    }
                }
            }
        }
    }

    private void ApplyResistanceAbility(Enemy enemy, Ability enemyAbility)
    {
        if (enemyAbility.ResistancesForDefence.Count != 0)
        {
            if (enemyAbility.AbilityData.TypeAbility == TypeAbilities.Buff)
            {
                foreach (Resistance newResist in enemyAbility.ResistancesForDefence)
                {
                    // Ищем существующий резист у Enemy
                    Resistance existingResist = enemy.Resistances.Find(r => r.TypeDamage == newResist.TypeDamage);

                    if (existingResist != null)
                    {
                        // Если резист уже есть, увеличиваем его значение
                        existingResist.ValueResistance += newResist.ValueResistance;
                    }
                    else
                    {
                        // Если резиста нет, создаем новый
                        enemy.Resistances.Add(new Resistance(newResist.TypeDamage, newResist.ValueResistance));
                    }
                }
            }

            else if (enemyAbility.AbilityData.TypeAbility == TypeAbilities.Aura)
            {
                foreach (Enemy enemyForBuffEffect in _enemiesList)
                {
                    foreach (Resistance newResist in enemyAbility.ResistancesForDefence)
                    {
                        // Ищем существующий резист у Enemy
                        Resistance existingResist = enemyForBuffEffect.Resistances.Find(r => r.TypeDamage == newResist.TypeDamage);

                        if (existingResist != null)
                        {
                            // Если резист уже есть, увеличиваем его значение
                            existingResist.ValueResistance += newResist.ValueResistance;
                        }
                        else
                        {
                            // Если резиста нет, создаем новый
                            enemyForBuffEffect.Resistances.Add(new Resistance(newResist.TypeDamage, newResist.ValueResistance));
                        }
                    }
                }
            }
        }
    }

    private void ApplyDebuffImmunAbility(Enemy enemy, Ability enemyAbility)
    {
        if (enemyAbility.DebuffsForDefence.Count != 0)
        {
            if (enemyAbility.AbilityData.TypeAbility == TypeAbilities.Buff)
            {
                foreach (Debuff newDebuff in enemyAbility.DebuffsForDefence)
                {
                    // Ищем существующий резист у Enemy
                    Debuff existingDebuff = enemy.DebuffImmun.Find(d => d.DebuffData.DebuffList == newDebuff.DebuffData.DebuffList);

                    if (existingDebuff == null)
                    {
                        // Если резист уже есть, увеличиваем его значение
                        enemy.DebuffImmun.Add(newDebuff);
                    }
                }
            }

            else if (enemyAbility.AbilityData.TypeAbility == TypeAbilities.Aura)
            {
                foreach (Enemy enemyForBuffEffect in _enemiesList)
                {
                    foreach (Debuff newDebuff in enemyAbility.DebuffsForDefence)
                    {
                        // Ищем существующий резист у Enemy
                        Debuff existingDebuff = enemyForBuffEffect.DebuffImmun.Find(d => d.DebuffData.DebuffList == newDebuff.DebuffData.DebuffList);

                        if (existingDebuff == null)
                        {
                            // Если резист уже есть, увеличиваем его значение
                            enemyForBuffEffect.DebuffImmun.Add(newDebuff);
                        }
                    }
                }
            }
        }
    }

    protected List<RewardItems> DeepCopyRewardItems(List<RewardItems> original)
    {
        List<RewardItems> copy = new List<RewardItems>();
        foreach (RewardItems reward in original)
        {
            RewardItems newItems = new RewardItems(reward.RewardItem, reward.MinAmountItems, reward.MaxAmountItems);
            copy.Add(newItems);
        }
        return copy;
    }

    protected void CopyRewardItems(RegionData regionData)
    {
        List<RewardItems> items = new List<RewardItems>();

        foreach (RegionLevelReward rewardRegion in regionData.RewardRegion[_level - 1].RewardItems)
        {
            foreach (RewardItems rewardItems in rewardRegion.RewardItems)
            {
                items.Add(rewardItems);
            }
        }

        _rewardItems = DeepCopyRewardItems(items);
    }

    public virtual void SaveRewardParametres(int guildCurrentRep, int guildRequiredRep, bool upLevelGuild, List<int> heroesCurrentExp,
        List<int> heroesRequiredExp, List<bool> upLevelHeroes, int currentExpeditionStage)
    {
        _guildCurrentRep = guildCurrentRep;
        _guildRequiredRep = guildRequiredRep;
        _upLevelGuild = upLevelGuild;
        _heroesCurrentExp = heroesCurrentExp;
        _heroesRequiredExp = heroesRequiredExp;
        _upLevelHeroes = upLevelHeroes;
    }

    protected int SetRandomValue(int minValue, int maxValue)
    {
        int randomValue = UnityEngine.Random.Range(minValue, maxValue);
        return randomValue;
    }
}
