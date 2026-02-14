using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Expedition : Quest
{
    [Header("Expefition Parametres")]
    [SerializeField] protected ExpeditionData _expeditionData;
    [SerializeField] private int _amountStages;
    [SerializeField] private int _currentStage;
    [SerializeField] private int _amountDaysToDisappear;
    [SerializeField] private int _leftDaysToDisappear;
    [Header("Amount Rewards Non-Main Stage")]
    [SerializeField] private int _minAmountStageRewards;
    [SerializeField] private int _maxAmountStageRewards;
    [SerializeField] private int _amountStageRewards;
    [SerializeField] private List<ExpeditionRewardList> _stageRewardItems = new List<ExpeditionRewardList>();
    [SerializeField] private List<DrawingItemData> _drawingRewards = new List<DrawingItemData>();
    [Header("Save Expedition Parametres")]
    [SerializeField] private int _currentSaveStage;

    public ExpeditionData ExpeditionData => _expeditionData;
    public int AmountStages => _amountStages;
    public int CurrentStage => _currentStage;
    public int AmountDaysToDisappear => _amountDaysToDisappear;
    public int LeftDays => _leftDaysToDisappear;
    public int CurrentSaveStage => _currentSaveStage;
    public int MinAmountStageRewards => _minAmountStageRewards;
    public int MaxAmountStageRewards => _maxAmountStageRewards;
    public int AmountStageRewards => _amountStageRewards;
    public List<ExpeditionRewardList> StageRewardItems => _stageRewardItems;
    public List<DrawingItemData> DrawingRewards => _drawingRewards;

    public Expedition(ExpeditionData expeditionData, RegionData regionData) : base(expeditionData, regionData) // Вызов конструктора базового класса
    {
        _expeditionData = expeditionData;

        _amountStages = SetRandomValue(2, 5);
        _currentStage = 0;

        _amountDaysToDisappear = SetRandomValue(expeditionData.MinAmountDaysToDisapper, expeditionData.MaxAmountDaysToDisapper);
        _leftDaysToDisappear = _amountDaysToDisappear;

        SetEnemy(regionData.EnemyDataListq[Level - 1].EnemyDatas);
        SetRandomParametres();

        DeepCopyDrawingRewards(expeditionData.DrawingRewards);
        _stageRewardItems = DeepCopyRewardStageItems(expeditionData.StageRewardItems);

        _currentSaveStage = 0;

        RemainDaysToComplete = _amountDaysToComplete;

        SetResistance();
    }

    public Expedition(Expedition expedition) : base(expedition.ExpeditionData, expedition.RegionData) // Вызов конструктора базового клас
    {
        _expeditionData = expedition.ExpeditionData;

        _amountStages = expedition.AmountStages;
        _currentStage = expedition.CurrentStage;

        _amountDaysToDisappear = expedition.AmountDaysToDisappear;
        _leftDaysToDisappear = expedition.LeftDays;
        _currentSaveStage = expedition.CurrentSaveStage;

        SetRandomParametres();

        SetEnemy(_regionData.EnemyDataListq[Level - 1].EnemyDatas);

        RemainDaysToComplete = _amountDaysToComplete;

        SetResistance();
    }

    public Expedition(Expedition copyExpedition, QuestParametresSystemFix questSystem) : base(copyExpedition, questSystem)
    {
        _expeditionData = copyExpedition.ExpeditionData;
        _regionData = copyExpedition.RegionData;

        _level = copyExpedition.ExpeditionData.Level;
        _power = copyExpedition.Power;
        _defence = copyExpedition.Defence;

        _rewardGold = copyExpedition.RewardGold;
        _rewardGuildReputation = copyExpedition.RewardGuildReputation;
        _rewardHeroExp = copyExpedition.RewardHeroExp;

        _enemiesList = new List<Enemy>();
        foreach (Enemy enemy in copyExpedition.EnemiesList)
            _enemiesList.Add(enemy);

        _affixesList = new List<Affix>();
        foreach (Affix affix in copyExpedition.AffixesList)
            _affixesList.Add(affix);

        _amountDaysToComplete = copyExpedition.AmountDaysToComplete;

        _regionQuest = copyExpedition.RegionQuest;

        CopyRewardItems(copyExpedition.RegionData);

        RemainDaysToComplete = _amountDaysToComplete;

        _questResistance = new List<Resistance>();
        foreach (Resistance res in copyExpedition.QuestResistance)
            _questResistance.Add(res);

        //_foodAmount.Add(questSystem.TotalAvailableFood, questSystem.TotalRequiredFood);
        //_lightAmount.Add(questSystem.TotalAvailableLight, questSystem.TotalRequiredLight);
        //_healAmount.Add(questSystem.TotalAvailableHeal, questSystem.TotalRequiredHeal);
        //_manaAmount.Add(questSystem.TotalAvailableMana, questSystem.TotalRequiredMana);

        _coefLight = questSystem.LightCoef;
        _coefHeal = questSystem.HealCoef;
        _coefMana = questSystem.ManaCoef;

        _powerHeroes = questSystem.TotalHeroesPower;
        _defenceHeroes = questSystem.TotalHeroesDefence;

        _chanceSupplies = questSystem.ChanceSupplies;
        _chanceCombat = questSystem.ChanceCombat;
        _chanceToCompleteQuest = questSystem.FullChanceComplete;

        _amountStages = copyExpedition.AmountStages;
        _currentStage = copyExpedition.CurrentStage;
        _amountDaysToDisappear = copyExpedition.AmountDaysToDisappear;
        _leftDaysToDisappear = copyExpedition.LeftDays;

    }

    private void ClearQuest()
    {
        _questData = null;
    }

    protected override void SetRandomParametres()
    {
        base.SetRandomParametres();

        _amountStageRewards = SetRandomValue(_minAmountStageRewards, _maxAmountStageRewards + 1);
    }

    public void UpdateCurrentStage()
    {
        _currentStage++;
    }

    public override void SaveRewardParametres(int guildCurrentRep, int guildRequiredRep, bool upLevelGuild,
        List<int> heroesCurrentExp, List<int> heroesRequiredExp, List<bool> upLevelHeroes, int currentExpeditionStage)
    {
        base.SaveRewardParametres(guildCurrentRep, guildRequiredRep, upLevelGuild, heroesCurrentExp, heroesRequiredExp, upLevelHeroes, currentExpeditionStage);
        _currentSaveStage = currentExpeditionStage;
    }

    private List<Enemy> DeepCopyEnemy(List<Enemy> original)
    {
        List<Enemy> copy = new List<Enemy>();
        foreach (Enemy enemy in original)
        {
            Enemy newEnemy = new Enemy(enemy.EnemyData);
            copy.Add(newEnemy);
        }
        return copy;
    }

    private List<Affix> DeepCopyAffix(List<Affix> original)
    {
        List<Affix> copy = new List<Affix>();
        foreach (Affix affix in original)
        {
            Affix newAffix = new Affix(affix.AffixData);
            copy.Add(newAffix);
        }
        return copy;
    }

    private void DeepCopyDrawingRewards(List<DrawingItemData> original)
    {
        foreach (DrawingItemData drawingReward in original)
            _drawingRewards.Add(drawingReward);
    }

    protected List<ExpeditionRewardList> DeepCopyRewardStageItems(List<ExpeditionRewardList> original)
    {
        List<ExpeditionRewardList> copy = new List<ExpeditionRewardList>();

        foreach (ExpeditionRewardList rewardList in original)
        {
            // Создаем новый список для копий RewardItems
            List<RewardItems> rewardItemsCopy = new List<RewardItems>();

            foreach (RewardItems rewardItem in rewardList.RewardItems)
            {
                // Создаем глубокую копию каждого RewardItems
                RewardItems newRewardItem = new RewardItems(rewardItem.RewardItem, rewardItem.MinAmountItems, rewardItem.MaxAmountItems);
                rewardItemsCopy.Add(newRewardItem);
            }

            // Создаем новый ExpeditionRewardList с копией RewardItems
            ExpeditionRewardList newRewardList = new ExpeditionRewardList(rewardItemsCopy);
            copy.Add(newRewardList);
        }

        return copy;
    }

    public void NextDayExpedition()
    {
        _leftDaysToDisappear--;
    }
}
