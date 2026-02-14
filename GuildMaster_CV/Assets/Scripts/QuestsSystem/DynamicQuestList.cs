using System;
using System.Collections.Generic;
using UnityEngine;

public class DynamicQuestList : MonoBehaviour
{
    [SerializeField] private QuestKeeper _questKeeper;
    [SerializeField] private GuildValutes _guildValutes;
    [SerializeField] private List<AffixData> _affixPrimaryList;
    [SerializeField] private List<RegionData> _regionDataList;
    [Header("Quest Amount")]
    [SerializeField] private int _minAmountQuest;
    [SerializeField] private int _maxAmountQuest;
    [SerializeField] private int _randomAmountQuest;
    [Header("Quest Level")]
    [SerializeField] private int _minLevelQuest;
    [SerializeField] private int _maxLevelQuest;
    [SerializeField] private int _randomLevelQuest;
    [Header("Quest List")]
    [SerializeField] private List<QuestData> _questDataList = new List<QuestData>();
    [SerializeField] private List<Quest> _questList;
    [Header("Expedition Amount")]
    [SerializeField] private int _minAmountExpedition;
    [SerializeField] private int _maxAmountExpedition;
    [SerializeField] private int _randomAmountExpedition;
    [Header("Expedition Level")]
    [SerializeField] private int _minLevelExpedition;
    [SerializeField] private int _maxLevelExpedition;
    [SerializeField] private int _randomLevelExpedition;
    [Header("Expedition List")]
    [SerializeField] private List<ExpeditionData> _expeditionDataList = new List<ExpeditionData>();
    [SerializeField] private List<Expedition> _expeditionList;
    [Header("Enemy Amount")]
    [SerializeField] private int _minAmountEnemy;
    [SerializeField] private int _maxAmountEnemy;
    [SerializeField] private int _randomAmountEnemy;
    [Header("Affix Amount")]
    [SerializeField] private int _minAmountAffix;
    [SerializeField] private int _maxAmountAffix;
    [SerializeField] private int _randomAmountAffix;
    [Header("Enemy List")]
    //[SerializeField] private List<EnemyData> _enemyDataList = new List<EnemyData>();
    [SerializeField] private List<Enemy> _enemyList;
    [Header("Affix List")]
    [SerializeField] private List<Affix> _affixList;

    private static readonly System.Random random = new System.Random();

    public List<Quest> QuestsList => _questList;
    public List<Expedition> ExpeditionList => _expeditionList;

    private void Awake()
    {
        //SetAmountQuests();
        //SetAmountExpeditiions();
    }

    private int SetRandomValue(int minValue, int maxValue)
    {
        int randomValue = UnityEngine.Random.Range(minValue, maxValue);
        return randomValue;
    }

    private RegionData SetRegion()
    {
        int randomRegion = SetRandomValue(0, _regionDataList.Count);

        RegionData newRegion = _regionDataList[randomRegion];

        return newRegion;
    }

    public void SetAmountQuests()
    {
        if (_minAmountQuest == 0 && _maxAmountQuest == 0)
            return;

        switch (_guildValutes.Level)
        {
            case 1:
                _minAmountQuest = 3;
                _maxAmountQuest = 5;

                _minLevelQuest = 1;
                _maxLevelQuest = 1;

                break;

            case 2:
                _minAmountQuest = 4;
                _maxAmountQuest = 7;

                _minLevelQuest = 1;
                _maxLevelQuest = 2;

                break;

            case 3:
                _minAmountQuest = 5;
                _maxAmountQuest = 10;

                _minLevelQuest = 1;
                _maxLevelQuest = 3;

                break;

            case 4:
                _minAmountQuest = 5;
                _maxAmountQuest = 15;

                _minLevelQuest = 1;
                _maxLevelQuest = 4;

                break;
        }

        _randomAmountQuest = SetRandomValue(_minAmountQuest, _maxAmountQuest);

        _questList = new List<Quest>(_randomAmountQuest);

        for (int i = 0; i < _randomAmountQuest; i++)
        {
            _randomLevelQuest = SetRandomValue(_minLevelQuest, _maxLevelQuest + 1);

            RegionData region = SetRegion();

            Quest newQuest = new Quest(_questDataList[_randomLevelQuest - 1], region);
            _questList.Add(newQuest);
            SetAffixesForQuest(newQuest);
        }
    }

    public void SetAffixesForQuest(Quest quest)
    {
        //_randomAmountAffix = SetRandomValue(_minAmountAffix, _maxAmountAffix);
        List<AffixData> availableAffixes = new List<AffixData>(_affixPrimaryList);
        _affixList = new List<Affix>();

        for (int i = 0; i < quest.RandomAmountAffixes; i++)
        {
            if (availableAffixes.Count == 0)
            {
                // Если нет доступных аффиксов, выходим из цикла
                break;
            }

            int randomIndex = SetRandomValue(0, availableAffixes.Count);

            // Получаем случайный аффикс из списка доступных
            AffixData selectedAffixData = availableAffixes[randomIndex];

            // Создаем новый объект Affix на основе выбранного AffixData
            Affix newAffix = new Affix(selectedAffixData);

            // Добавляем аффикс в список аффиксов квеста
            quest.AffixesList.Add(newAffix);

            // Удаляем выбранный аффикс из доступных, чтобы не допустить его повторного выбора
            availableAffixes.RemoveAt(randomIndex);
        }
    }

    public Expedition CreateNewExpedition(ExpeditionalRegionSlot_UI slot, int level)
    {
        Expedition newExpedition = new Expedition(_expeditionDataList[level - 1], slot.RegionData);
        //SetEnemiesForQuest(newExpedition, slot.RegionData);
        SetAffixesForQuest(newExpedition);
        //_expeditionList.Add(newExpedition);
        //_questKeeper.AddNewExpedition(newExpedition);

        return newExpedition;
    }

    public RegionQuest GetRandomRegionQuest()
    {
        // Получаем все значения enum
        Array values = Enum.GetValues(typeof(RegionQuest));

        // Выбираем случайное значение
        RegionQuest randomRegion = (RegionQuest)values.GetValue(random.Next(values.Length));

        return randomRegion;
    }
}