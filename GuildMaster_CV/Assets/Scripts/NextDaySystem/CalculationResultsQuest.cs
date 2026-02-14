using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public class CalculationResultsQuest : MonoBehaviour
{
    [SerializeField] private TakingQuestUIController _takingQuestUIController;
    [SerializeField] private QuestKeeper _takingQuestKeeper;
    [SerializeField] private QuestKeeper _mainQuestKeeper;
    [SerializeField] private PlayerInventoryHolder _playerInventoryHolder;
    [SerializeField] private GuildKeeper _guildKeeper;
    [SerializeField] private NextDayController _nextDayController;
    [SerializeField] private GuildValutes _guildValutes;
    [SerializeField] private DynamicQuestList _dynamicQuestList;
    [SerializeField] private NotificationSystem _notificationSystem;
    [SerializeField] private DrawingKeeperDisplay _drawingKeeperDisplay;
    [Space]
    [Header("Craft Keepers")]
    [SerializeField] private CraftKeeper _alchemistKeeper;
    [SerializeField] private CraftKeeper _cookKeeper;
    [SerializeField] private CraftKeeper _jewelerKeeper;
    [SerializeField] private CraftKeeper _workshopKeeper;
    [SerializeField] private CraftKeeper _blacksmithKeeper;
    [SerializeField] private CraftKeeper _tannerKeeper;
    [SerializeField] private CraftKeeper _tailorKeeper;
    [SerializeField] private CraftKeeper _magicShopKeeper;
    [Header("Talent Properties")]
    [SerializeField] private float _coefExpHero;
    [SerializeField] private float _coefGold;

    public GuildValutes GuildValutes => _guildValutes;

    public event UnityAction<int> OnCollectGoldForQuest;

    private void Awake()
    {
        _nextDayController.OnNextDay += CheckComletedQuests;
    }

    private void OnDestroy()
    {
        _nextDayController.OnNextDay -= CheckComletedQuests;
    }

    private void CheckComletedQuests()
    {
        List<Quest> completedQuests = new List<Quest>();
        List<Expedition> completedExpeditions = new List<Expedition>();

        foreach (QuestSlot quest in _takingQuestKeeper.BoardSystem.QuestSlots)
        {
            if (quest.Quest.RemainDaysToComplete != 0)
            {
                quest.Quest.RemainDaysToComplete -= 1;

                if (quest.Quest.RemainDaysToComplete == 0)
                {
                    if (quest.Quest.IsQuestToChecked)
                    {
                        continue;
                    }

                    else
                    {
                        completedQuests.Add(quest.Quest);
                        quest.Quest.IsQuestToChecked = true;
                    }
                }
            }
        }

        foreach (Expedition expedition in _takingQuestKeeper.BoardSystem.ExpeditionSlots)
        {
            if (expedition.RemainDaysToComplete != 0)
            {
                expedition.RemainDaysToComplete -= 1;

                if (expedition.RemainDaysToComplete == 0)
                {
                    if (expedition.IsQuestToChecked)
                    {
                        continue;
                    }

                    else
                    {
                        completedExpeditions.Add(expedition);
                        expedition.IsQuestToChecked = true;
                    }
                }
            }
        }

        CheckComletedQuests(completedQuests, completedExpeditions);
        _notificationSystem.CheckNotifications();
    }

    private void CheckComletedQuests(List<Quest> completedQuests, List<Expedition> completedExpeditions)
    {
        foreach (Quest quest in completedQuests)
        {
            bool successQuest = CheckResultsQuest(quest);
            quest.IsQuestToSuccess = successQuest;

            if (successQuest)
                QuestSuccessed(quest);

            else
                QuestFailed(quest);

            _takingQuestUIController.ViewedQuests.BoardSystem.AddNewQuest(quest);
        }

        foreach (Expedition expedition in completedExpeditions)
        {
            bool successExpedition = CheckResultsQuest(expedition);
            expedition.IsQuestToSuccess = successExpedition;

            if (successExpedition)
                QuestSuccessed(expedition);

            else
                QuestFailed(expedition);

            _takingQuestUIController.ViewedQuests.BoardSystem.AddNewExpedition(expedition);
        }
    }

    private bool CheckResultsQuest(Quest quest)
    {
        int randomValue = Random.Range(1, 101);
        Debug.Log(randomValue);
        Debug.Log(quest.ChanceToComplete);

        if (quest.ChanceToComplete >= randomValue)
            return true;

        else
        {
            if (quest.ChanceToComplete >= 80)  //80% - УСЛОВНЫЙ ШАНС ДЛЯ ПЕРЕРАСЧЕТА, ЧТОБЫ УВЕЛИЧИТЬ ШАНС ПОБЕДЫ ПРИ ВЫСОКОМ ШАНСЕ СБОРА НА ЗАДАНИЕ
            {
                int secondRandomValue = Random.Range(1, 101);

                if (quest.ChanceToComplete >= randomValue)
                    return true;

                else
                    return false;
            }

            else
                return false;
        }
    }

    private void QuestSuccessed(Quest quest)
    {
        quest.SaveRewardItems = GetRandomRewardItems(quest);

        foreach (RewardItems item in quest.SaveRewardItems)
        {
            if (item.RewardItem is DrawingItemData drawingItemData)
                CheckAvailableDrawing(drawingItemData, item);

            else
                _playerInventoryHolder.AddToInventory(item.RewardItem, item.AmountItems);
        }

        quest.SaveRewardBlanks = CalculateBlankRewardChances(quest.RegionData.RewardBlankss, quest.Level);

        foreach (BlankSlot blankSlot in quest.SaveRewardBlanks)
            _playerInventoryHolder.PrimaryInventorySystem.AddToInventory(blankSlot, 1);

        List<int> currentExpHeroes = new List<int> { 0, 0, 0, 0 };
        List<int> requiredExpHeroes = new List<int> { 0, 0, 0, 0 };
        List<bool> upLevelHeroes = new List<bool> { false, false, false, false };

        for (int i = 0; i < quest.HeroesSlots.Count; i++)
        {
            if (quest.HeroesSlots[i] != null && quest.HeroesSlots[i].HeroData != null)
            {
                quest.HeroesSlots[i].IsSentOnQuest = false;
                quest.HeroesSlots[i].WoundType = CheckWoundHero(quest, quest.HeroesSlots[i]);

                if (quest.HeroesSlots[i].WoundType == WoundsType.Dead)
                {
                    Hero matchingHero = _guildKeeper.RecruitSystem.HeroSlots.FirstOrDefault(k => k == quest.HeroesSlots[i]);
                    _guildKeeper.RecruitSystem.HeroSlots.Remove(matchingHero);
                    _guildValutes.KickHero(1);
                }

                else
                {
                    foreach (EquipSlot itemSlot in quest.HeroesSlots[i].EquipHolder.EquipSystem.Slots)
                    {
                        if (itemSlot != null && itemSlot.EquipItemData != null)
                        {
                            if (itemSlot.EquipItemData.EquipType == EquipType.Light)
                                itemSlot.ClearSlot();

                            else if (itemSlot.EquipItemData.EquipType == EquipType.Trinket)
                            {
                                EquipItem equipItem = new EquipItem();
                                equipItem.UnEquip(quest.HeroesSlots[i], itemSlot);
                                itemSlot.ClearSlot();
                            }
                        }
                    }

                    quest.HeroesSlots[i].ChangeSlotCondition(false);

                    //var matchingHero = _sendHeroesSlots.Slots.FirstOrDefault(k => k == quest.HeroesSlots[i]);

                    //foreach (Ability ability in quest.HeroesSlots[i].AbilityHolder.HeroAbilitySystem.AbilityList)
                    //{

                    //}

                    float rewardHeroExp = (quest.RewardHeroExp * _coefExpHero);
                    int randomRewardExp = Random.Range((int)(rewardHeroExp * 0.9), (int)(rewardHeroExp * 1.1)); // РАЗБРОС 10% ДЛЯ ОПЫТА ГЕРОЕВ ЗА ЗАДАНИЕ

                    (int newLevelHero, bool isLevelUpHero) = quest.HeroesSlots[i].ChangeExp(randomRewardExp);
                    upLevelHeroes[i] = isLevelUpHero;
                    quest.HeroesSlots[i].ChangeRested(false);

                    for (int ab = 0; ab < quest.HeroesSlots[i].AbilityHolder.HeroAbilitySystem.AbilityList.Count; ab++)
                        quest.HeroesSlots[i].AbilityHolder.HeroAbilitySystem.AbilityList[ab] = new Ability();

                    //quest.HeroesSlots[i].ChangeExp(quest.RewardHeroExp);
                }

                currentExpHeroes[i] = quest.HeroesSlots[i].LevelSystem.CurrentExp;
                requiredExpHeroes[i] = quest.HeroesSlots[i].LevelSystem.RequiredExp;
            }
        }

        (int newLevelGuild, bool isLevelUpGuild) = _guildValutes.GainGuildReputation(quest.RewardGuildReputation);

        int questGold = (int)(quest.RewardGold * _coefGold);
        OnCollectGoldForQuest?.Invoke(quest.RewardGold);

        if (quest is Expedition expedition)
        {
            expedition.UpdateCurrentStage();

            quest.SaveRewardParametres(_guildValutes.CurrentRep, _guildValutes.RequiredRep, isLevelUpGuild,
currentExpHeroes, requiredExpHeroes, upLevelHeroes, expedition.CurrentStage);

            if (expedition.CurrentStage != expedition.AmountStages)
            {
                Expedition copyExpedition = _mainQuestKeeper.BoardSystem.AddAgainExpedition(expedition);
                //copyExpedition.EnemiesList.Clear();
                copyExpedition.AffixesList.Clear();
                //_dynamicQuestList.SetEnemiesForQuest(copyExpedition, copyExpedition.RegionData);
                _dynamicQuestList.SetAffixesForQuest(copyExpedition);
            }
        }

        else
        {
            quest.SaveRewardParametres(_guildValutes.CurrentRep, _guildValutes.RequiredRep, isLevelUpGuild, currentExpHeroes, requiredExpHeroes, upLevelHeroes, -1);
        }

    }

    private void QuestFailed(Quest quest)
    {
        List<int> currentExpHeroes = new List<int> { 0, 0, 0, 0 };
        List<int> requiredExpHeroes = new List<int> { 0, 0, 0, 0 };
        List<bool> upLevelHeroes = new List<bool> { false, false, false, false };

        for (int i = 0; i < quest.HeroesSlots.Count; i++)
        {
            if (quest.HeroesSlots[i] != null && quest.HeroesSlots[i].HeroData != null)
            {
                Hero matchingHero = _guildKeeper.RecruitSystem.HeroSlots.FirstOrDefault(k => k == quest.HeroesSlots[i]);
                _guildKeeper.RecruitSystem.HeroSlots.Remove(matchingHero);
                _guildValutes.KickHero(1);

                quest.HeroesSlots[i].WoundType = WoundsType.Dead;

                currentExpHeroes[i] = quest.HeroesSlots[i].LevelSystem.CurrentExp;
                requiredExpHeroes[i] = quest.HeroesSlots[i].LevelSystem.RequiredExp;
            }
        }

        quest.SaveRewardParametres(_guildValutes.CurrentRep, _guildValutes.RequiredRep, false, currentExpHeroes, requiredExpHeroes, upLevelHeroes, -1);
        _guildValutes.GainGuildReputation(-quest.RewardGuildReputation);
        //OnCollectGoldForQuest?.Invoke(-quest.RewardGold);
        if (quest is Expedition expedition)
        {
            Expedition copyExpedition = _mainQuestKeeper.BoardSystem.AddAgainExpedition(expedition);
            copyExpedition.SetCopyValues(expedition.Level, expedition.Power, expedition.Defence, expedition.EnemiesList, expedition.AffixesList,
                expedition.RewardGuildReputation, expedition.RewardGold, expedition.RewardHeroExp, expedition.AmountDaysToComplete);
        }

    }

    private List<RewardItems> GetRandomRewardItems(Quest quest)
    {
        List<RewardItems> randomRewards = new List<RewardItems>();
        int indexReward = quest.Level - 1;

        List<RewardItems> mainItemsReward = CalculateRewardChancesToAdd(quest.RegionData.RewardRegion[indexReward].RewardItems);
        List<RewardItems> runesReward = CalculateRewardChancesToAdd(quest.RegionData.RewardRunes[indexReward].RewardItems);
        List<RewardItems> catalystsReward = CalculateRewardChancesToAdd(quest.RegionData.RewardCatalysts[indexReward].RewardItems);

        foreach (RewardItems item in mainItemsReward)
            randomRewards.Add(item);

        foreach (RewardItems item in runesReward)
            randomRewards.Add(item);

        foreach (RewardItems item in catalystsReward)
            randomRewards.Add(item);

        if (quest is Expedition expedition)
        {
            if (expedition.CurrentStage == expedition.AmountStages - 1)
            {
                // Создаем список всех RewardItems из всех RegionLevelReward
                List<DrawingItemData> availableDrawingRewards = new List<DrawingItemData>();

                // Проходим по каждому RegionLevelReward в _stageRewardItems
                foreach (DrawingItemData drawingCopy in expedition.ExpeditionData.DrawingRewards)
                {
                    // Добавляем все RewardItems из текущего RegionLevelReward в availableRewards
                    availableDrawingRewards.Add(drawingCopy);
                }

                // Флаг для отслеживания, найден ли подходящий предмет
                bool itemFound = false;

                // Пока есть доступные предметы для проверки
                while (availableDrawingRewards.Count > 0)
                {
                    // Выбираем случайный индекс из доступных предметов
                    int randomValue = UnityEngine.Random.Range(0, availableDrawingRewards.Count);

                    // Получаем случайный предмет
                    DrawingItemData selectedReward = availableDrawingRewards[randomValue];

                    // Проверяем, есть ли предмет в списках CheckedItemsList или NewItemsList
                    if (_drawingKeeperDisplay.CheckedItemsList.Contains(selectedReward) ||
                        _drawingKeeperDisplay.NewItemsList.Contains(selectedReward))
                    {
                        // Если предмет уже есть в списках, удаляем его из доступных для выбора
                        availableDrawingRewards.RemoveAt(randomValue);
                    }
                    else
                    {
                        CheckAvailableDrawing(availableDrawingRewards[randomValue], null);
                        //_drawingKeeperDisplay.NewItemsList.Add(selectedReward);
                        //_drawingKeeperDisplay.IsWindowChecked = false;
                        itemFound = true; // Устанавливаем флаг, что предмет найден
                        break; // Выходим из цикла
                    }
                }

                // Если подходящий предмет не найден
                if (!itemFound)
                {
                    Debug.Log("Нет доступных предметов для добавления.");
                }
            }

            else
            {
                foreach (ExpeditionRewardList expeditionalLevelReward in expedition.StageRewardItems)
                {
                    foreach (RewardItems rewardItem in expeditionalLevelReward.RewardItems)
                    {
                        int randomValue = UnityEngine.Random.Range(0, 101);

                        if (rewardItem.ChanceDrop >= randomValue)
                        {

                            RewardItems item = new RewardItems(rewardItem.RewardItem, 1, 1);
                            randomRewards.Add(item);

                            if (rewardItem.RewardItem is DrawingItemData drawingItemData)
                                CheckAvailableDrawing(drawingItemData, null);
                        }
                    }
                }
            }
        }

        return randomRewards;
    }

    private List<BlankSlot> CalculateBlankRewardChances(List<BlankRewards> blankRewardItems, int questLevel)
    {
        List<BlankSlot> blankRewards = new List<BlankSlot>();

        foreach (BlankRewards blankReward in blankRewardItems)
        {
            int randomValueMain = UnityEngine.Random.Range(0, 101);

            if (blankReward.ChanceDrop >= randomValueMain)
            {
                BlankSlot newBlankSlot = new BlankSlot(blankReward.BlankSlotData, questLevel);
                blankRewards.Add(newBlankSlot);
            }

            else
            {
                int randomValue_1 = UnityEngine.Random.Range(0, 101);
                int randomTier = UnityEngine.Random.Range(1, questLevel + 1);

                if (blankReward.ChanceDrop >= randomValue_1)
                {
                    BlankSlot newBlankSlot = new BlankSlot(blankReward.BlankSlotData, randomTier);
                    blankRewards.Add(newBlankSlot);
                }
            }
        }

        return blankRewards;
    }

    private List<RewardItems> CalculateRewardChancesToAdd(List<RegionLevelReward> rewardItems)
    {
        List<RewardItems> randomRewards = new List<RewardItems>();

        foreach (RegionLevelReward rewardRegion in rewardItems)
        {
            foreach (RewardItems rewardItem in rewardRegion.RewardItems)
            {
                int randomValue = UnityEngine.Random.Range(0, 101);

                if (rewardItem.ChanceDrop >= randomValue)
                {
                    RewardItems item = new RewardItems(rewardItem.RewardItem, rewardItem.MinAmountItems, rewardItem.MaxAmountItems);
                    randomRewards.Add(item);
                }
            }
        }

        return randomRewards;
    }

    private void CheckAvailableDrawing(DrawingItemData drawingItemData, RewardItems item)
    {
        CraftKeeper localCraftKeeper = null;

        switch (drawingItemData.CraftItem.ItemType)
        {
            //Alchemist
            case ItemType.Herbs:
                localCraftKeeper = _alchemistKeeper;
                break;

            case ItemType.Potions:
                localCraftKeeper = _alchemistKeeper;
                break;

            case ItemType.Liquids:
                localCraftKeeper = _alchemistKeeper;
                break;

            //Cook
            case ItemType.Ingredients:
                localCraftKeeper = _cookKeeper;
                break;

            case ItemType.Food:
                localCraftKeeper = _cookKeeper;
                break;

            //Jeweler
            case ItemType.Gem:
                localCraftKeeper = _jewelerKeeper;
                break;

            case ItemType.Amulet:
                localCraftKeeper = _jewelerKeeper;
                break;

            case ItemType.Ring:
                localCraftKeeper = _jewelerKeeper;
                break;

            //WorkShop
            case ItemType.Thread:
                localCraftKeeper = _workshopKeeper;
                break;

            case ItemType.Resin:
                localCraftKeeper = _workshopKeeper;
                break;

            case ItemType.Lining:
                localCraftKeeper = _workshopKeeper;
                break;

            case ItemType.Rivet:
                localCraftKeeper = _workshopKeeper;
                break;

            case ItemType.Coal:
                localCraftKeeper = _workshopKeeper;
                break;

            case ItemType.Bones:
                localCraftKeeper = _workshopKeeper;
                break;

            case ItemType.Ores:
                localCraftKeeper = _workshopKeeper;
                break;

            case ItemType.Leather:
                localCraftKeeper = _workshopKeeper;
                break;

            case ItemType.Clothes:
                localCraftKeeper = _workshopKeeper;
                break;

            case ItemType.WorkShop_Items:
                localCraftKeeper = _workshopKeeper;
                break;

            //Blacksmith
            case ItemType.Metal_Helmet:
                localCraftKeeper = _blacksmithKeeper;
                break;

            case ItemType.Metal_Chest:
                localCraftKeeper = _blacksmithKeeper;
                break;

            case ItemType.Metal_Gloves:
                localCraftKeeper = _blacksmithKeeper;
                break;

            case ItemType.Metal_Pants:
                localCraftKeeper = _blacksmithKeeper;
                break;

            case ItemType.Metal_Boots:
                localCraftKeeper = _blacksmithKeeper;
                break;

            case ItemType.OneHand_Sword:
                localCraftKeeper = _blacksmithKeeper;
                break;

            case ItemType.OneHand_Hammer:
                localCraftKeeper = _blacksmithKeeper;
                break;

            case ItemType.TwoHand_Sword:
                localCraftKeeper = _blacksmithKeeper;
                break;

            case ItemType.TwoHand_Hammer:
                localCraftKeeper = _blacksmithKeeper;
                break;

            case ItemType.Shield:
                localCraftKeeper = _blacksmithKeeper;
                break;

            case ItemType.Bow:
                localCraftKeeper = _blacksmithKeeper;
                break;

            case ItemType.CrossBow:
                localCraftKeeper = _blacksmithKeeper;
                break;

            case ItemType.Dagger:
                localCraftKeeper = _blacksmithKeeper;
                break;

            case ItemType.Staff:
                localCraftKeeper = _blacksmithKeeper;
                break;

            //Tanner
            case ItemType.Leather_Helmet:
                localCraftKeeper = _tannerKeeper;
                break;

            case ItemType.Leather_Chest:
                localCraftKeeper = _tannerKeeper;
                break;

            case ItemType.Leather_Gloves:
                localCraftKeeper = _tannerKeeper;
                break;

            case ItemType.Leather_Pants:
                localCraftKeeper = _tannerKeeper;
                break;

            case ItemType.Leather_Boots:
                localCraftKeeper = _tannerKeeper;
                break;

            //Tailor
            case ItemType.Clothes_Helmet:
                localCraftKeeper = _tailorKeeper;
                break;

            case ItemType.Clothes_Chest:
                localCraftKeeper = _tailorKeeper;
                break;

            case ItemType.Clothes_Gloves:
                localCraftKeeper = _tailorKeeper;
                break;

            case ItemType.Clothes_Pants:
                localCraftKeeper = _tailorKeeper;
                break;

            case ItemType.Clothes_Boots:
                localCraftKeeper = _tailorKeeper;
                break;

            //MagicShop
            case ItemType.Essence:
                localCraftKeeper = _magicShopKeeper;
                break;

            case ItemType.Pollen:
                localCraftKeeper = _magicShopKeeper;
                break;

            case ItemType.Foliant:
                localCraftKeeper = _magicShopKeeper;
                break;

            case ItemType.Magic_Item:
                localCraftKeeper = _magicShopKeeper;
                break;
        }

        CraftSlot matchingCraftItem = localCraftKeeper.CraftSystem.CraftList.FirstOrDefault(k => k.ItemData == drawingItemData.CraftItem);

        //if (matchingCraftItem != null)
        //    _playerInventoryHolder.AddToInventory(item.RewardItem, item.AmountItems);

        if (matchingCraftItem == null)
        {
            localCraftKeeper.CraftSystem.CraftList.Add(new CraftSlot(drawingItemData.CraftItem));
            _drawingKeeperDisplay.NewItemsList.Add(drawingItemData.CraftItem);
            _drawingKeeperDisplay.IsWindowChecked = false;
        }
    }

    private WoundsType CheckWoundHero(Quest quest, Hero hero)
    {
        int randomValue = Random.Range(1, 101);
        float deadChance = 100 - quest.ChanceToComplete;
        //float deadChance = 100;
        float heavyWoundChance = (100 - quest.ChanceToComplete) * 1.2f;
        float mediumWoundChance = (100 - quest.ChanceToComplete) * 1.5f;
        float lightWoundChance = 50;

        int coefSpreadShit = 5; // см. Таблицу расчета силы и ран
        int coefWound = hero.VisibleHeroStats.Health / hero.LevelSystem.Level / coefSpreadShit;

        if (deadChance >= randomValue + coefWound)
        {
            Debug.Log("dead");
            return WoundsType.Dead;
        }

        else if (heavyWoundChance >= randomValue + coefWound)
        {
            Debug.Log("heavy");
            return WoundsType.Heavy;
        }

        else if (mediumWoundChance >= randomValue + coefWound)
        {
            Debug.Log("medium");
            return WoundsType.Medium;
        }

        else if (lightWoundChance >= randomValue + coefWound)
        {
            Debug.Log("lught");
            return WoundsType.Light;
        }

        return WoundsType.Healthy;
    }

    public void ChangeHeroExpByTalent(float coefExpHero)
    {
        _coefExpHero += coefExpHero;
    }

    public void ChangeGoldQuestByTalent(float coefGold)
    {
        _coefGold += coefGold;
    }
}
