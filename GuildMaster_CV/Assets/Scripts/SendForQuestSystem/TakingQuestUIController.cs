using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;

public class TakingQuestUIController : MainQuestKeeperDisplay
{
    [Header("Others")]
    [SerializeField] private Button _showResultsQuest;
    [SerializeField] private QuestKeeper _takingQuests;
    [SerializeField] private MainQuestKeeperDisplay _mainQuestKeeperDisplay;
    [SerializeField] private ResImage_UI _resImagePrefab;
    [SerializeField] private Transform _resImageContainer;
    [Header("Viewed Quests Parametres")]
    [SerializeField] private QuestKeeper _viewedQuests;
    [SerializeField] private bool _isViewQuests;
    [SerializeField] private Button _viewedOrTakingQuestsButton;
    [SerializeField] private TextMeshProUGUI _viewedOrTakingQuestsText;
    [Header("Chance Quest Parametres")]
    [SerializeField] private TextMeshProUGUI _chanceToComplete;
    [SerializeField] private TextMeshProUGUI _chanceSupplies;
    [SerializeField] private TextMeshProUGUI _chanceCombat;
    [Header("Main Quest Parametres")]
    [SerializeField] private TextMeshProUGUI _level;
    [SerializeField] private TextMeshProUGUI _power;
    [SerializeField] private TextMeshProUGUI _defence;
    [SerializeField] private TextMeshProUGUI _remainDays;
    [Header("Secondary Quest Parametres")]
    [SerializeField] private TextMeshProUGUI _food;
    [SerializeField] private TextMeshProUGUI _light;
    [SerializeField] private TextMeshProUGUI _heal;
    [SerializeField] private TextMeshProUGUI _mana;
    [Header("Resistance Quest Parametres")]
    [SerializeField] private TextMeshProUGUI _fireRes;
    [SerializeField] private TextMeshProUGUI _coldRes;
    [SerializeField] private TextMeshProUGUI _lightningRes;
    [SerializeField] private TextMeshProUGUI _necroticRes;
    [SerializeField] private TextMeshProUGUI _voidRes;

    public ShowResultSystem ShowResultSystem { get; private set; }

    public QuestKeeper ViewedQuests => _viewedQuests;

    public event UnityAction<float, int> OnUpdatePower;
    public event UnityAction<float, int> OnUpdateDefence;

    protected override void Awake()
    {
        base.Awake();

        ShowResultSystem = GetComponent<ShowResultSystem>();

        ClearInfo();

        _showResultsQuest?.onClick.AddListener(ShowResult);
    }

    protected override void Start()
    {
        ClearSlots();
        RefreshInfo();

        SetQuestBoardAfterDay();
    }

    protected override void Update()
    {
        // открывается на Q, если убрать
    }

    private void OnEnable()
    {
        ClearInfo();
    }

    private void OnDisable()
    {
        ClearInfo();
        _isViewQuests = false;
    }

    public override void DisplayQuestBoard(QuestBoardSystem boardSystem)
    {
        if (!_panelDisplay.gameObject.activeSelf)
            _panelDisplay.gameObject.SetActive(true);

        SetQuestBoardAfterDay();
    }

    public override void SetQuestBoardAfterDay()
    {
        _boardSystem = _takingQuests.BoardSystem;

        ClearSlots();
        RefreshInfo();

        foreach (Expedition expedition in _boardSystem.ExpeditionSlots)
        {
            if (expedition.ExpeditionData == null)
                continue;

            CreateExpeditionSlot(expedition);
        }

        for (int i = _boardSystem.QuestSlots.Count - 1; i >= 0; i--)
        {
            if (_boardSystem.QuestSlots[i].Quest.QuestData == null)
                continue;

            CreateQuestSlot(_boardSystem.QuestSlots[i]);
        }

        //foreach (QuestSlot quest in _boardSystem.QuestSlots)
        //{
        //    if (quest.Quest.QuestData == null)
        //        continue;

        //    CreateQuestSlot(quest);
        //}

        _enemySlots.gameObject.SetActive(true);
        _affixesSlots.gameObject.SetActive(true);

    }

    protected override void RefreshInfo()
    {
        _enemyInfo.SetActive(false);
        _heroInfo.SetActive(false);
        _enemySlots.SetActive(false);
        _affixesSlots.SetActive(false);
        _showResultsQuest.gameObject.SetActive(false);
    }

    protected override void TakeQuest() { }

    public override void SelectQuest(QuestSlot_UI questSlot)
    {
        base.SelectQuest(questSlot);

        //if (_selectedQuest != null && _selectedQuest.Quest != null && _selectedQuest.Quest.Quest != null)
        //{
        //    _lastSelectedQuest = _selectedQuest;
        //}

        //else
        //{
        //    _lastSelectedQuest = questSlot;
        //}

        //RefreshInfo();
        //_selectedQuest = questSlot;

        //if (PossibleRewardsSystem != null && PossibleRewardsSystem.Panel.gameObject.activeSelf)
        //    PossibleRewardsSystem.ShowWindow();

        //if (questSlot is ExpeditionSlot_UI expeditionSlot)
        //{
        //    if (!_expeditionProgress.gameObject.activeSelf)
        //    {
        //        _expeditionProgress.gameObject.SetActive(true);
        //    }


        //    _expeditionProgress.ShowProgress(expeditionSlot.Expedition);
        //}

        //else
        //{
        //    if (_expeditionProgress.gameObject.activeSelf)
        //    {
        //        _expeditionProgress.gameObject.SetActive(false);
        //        _expeditionDaysLeft.text = "";
        //    }
        //}

        //OnQuestClick?.Invoke();
    }

    public void SelectTakingQuest(QuestSlot_UI questSlot)
    {
        SelectQuest(questSlot);

        foreach (var slot in _heroesSlots.Slots)
        {
            slot.ClearSlot(); // Ваш метод сброса состояния
        }

        // Заполняем слоты из словаря
        foreach (var kvp in questSlot.Quest.Quest.VisibleHeroesSlots)
        {
            int slotIndex = kvp.Key;
            Hero hero = kvp.Value;

            // Проверяем, что индекс в допустимых пределах
            if (slotIndex >= 0 && slotIndex < _heroesSlots.Slots.Count)
            {
                _heroesSlots.Slots[slotIndex].InitSlot(hero);

                if (hero?.HeroData != null)
                {
                    _heroesSlots.Slots[slotIndex].UpdateUI(hero.HeroPowerPoints.AllPower * hero.HeroPowerPoints.AllCoefPower,
                        hero.HeroDefencePoints.AllDefence * hero.HeroDefencePoints.AllCoefDefence);
                }
            }
            else
            {
                Debug.LogWarning($"Invalid slot index {slotIndex} in HeroesSlots dictionary");
            }
        }

        UpdateUIInfo(questSlot, questSlot.Quest.Quest.PowerHeroes, questSlot.Quest.Quest.DefenceHeroes);

        if (questSlot.Quest.Quest.RemainDaysToComplete == 0)
            _showResultsQuest.gameObject.SetActive(true);
    }

    private void UpdateUIInfo(QuestSlot_UI questSlot, float powerHeroesSum, float defenceHeroesSum)
    {
        _chanceToComplete.text = $"Chance To Complete: {questSlot.Quest.Quest.ChanceToComplete.ToString("F2")}%";
        _chanceSupplies.text = $"Supplies: {questSlot.Quest.Quest.ChanceSupplies.ToString("F2")}%";
        _chanceCombat.text = $"Combat: {questSlot.Quest.Quest.ChanceCombat.ToString("F2")}%";

        _level.text = $"Level: {questSlot.Quest.Quest.Level}";
        _power.text = $"Power: {powerHeroesSum} / {questSlot.Quest.Quest.Power}";
        _defence.text = $"Defence: {defenceHeroesSum} / {questSlot.Quest.Quest.Defence}";
        _remainDays.text = $"Remain Days: {questSlot.Quest.Quest.RemainDaysToComplete}";

        _food.text = $"Food: {string.Join(", ", questSlot.Quest.Quest.FoodAmount.Keys)} / {string.Join(", ", questSlot.Quest.Quest.FoodAmount.Values)}";
        _light.text = $"Light: {string.Join(", ", questSlot.Quest.Quest.LightAmount.Keys)} / {string.Join(", ", questSlot.Quest.Quest.LightAmount.Values)}";
        _heal.text = $"Heal: {string.Join(", ", questSlot.Quest.Quest.HealAmount.Keys)} / {string.Join(", ", questSlot.Quest.Quest.HealAmount.Values)}";
        _mana.text = $"Mana: {string.Join(", ", questSlot.Quest.Quest.ManaAmount.Keys)} / {string.Join(", ", questSlot.Quest.Quest.ManaAmount.Values)}";

        UpdateResistanceInfoUI(questSlot.Quest.Quest.QuestResistance);

        //_fireRes.text = $"{questSlot.Quest.Quest.QuestResistanceHollow[0]}%";
        //_coldRes.text = $"{questSlot.Quest.Quest.QuestResistanceHollow[1]}%";
        //_lightningRes.text = $" {questSlot.Quest.Quest.QuestResistanceHollow[2]}%";
        //_necroticRes.text = $" {questSlot.Quest.Quest.QuestResistanceHollow[3]}%";
        //_voidRes.text = $"{questSlot.Quest.Quest.QuestResistanceHollow[4]}%";
    }

    public void UpdateResistanceInfoUI(List<Resistance> activeResistanceQuest)
    {
        ClearIcons();

        foreach (Resistance res in activeResistanceQuest)
        {
            ResImage_UI imageRes = Instantiate(_resImagePrefab, _resImageContainer.transform);

            imageRes.SetTypeDamage(res.TypeDamage, true);

            imageRes.Percent.text = $"{res.ValueResistance.ToString()}%";
        }
    }

    private void ClearInfo()
    {
        _chanceToComplete.text = "Chance To Complete: 0%";
        _chanceSupplies.text = "Supplies: 0%";
        _chanceCombat.text = "Combat: 0%";

        _level.text = "Level:";
        _power.text = "Power:";
        _defence.text = "Defence:";
        _remainDays.text = "Remain Days:";

        _food.text = "Food:";
        _light.text = "Light:";
        _heal.text = "Heal:";
        _heal.text = "Mana:";

        _fireRes.text = "0%";
        _coldRes.text = "0%";
        _lightningRes.text = "0%";
        _necroticRes.text = "0%";
        _voidRes.text = "0%";
    }

    public void ClearIcons()
    {
        if (_resImageContainer != null)
        {
            foreach (Transform child in _resImageContainer)
            {
                Destroy(child.gameObject);
            }
        }
    }

    private void ShowResult()
    {
        if (_selectedQuest.Quest.Quest is Expedition expedition)
        {
            if (_isViewQuests)
            {
                Expedition matchingExpedition = _viewedQuests.BoardSystem.ExpeditionSlots.FirstOrDefault(i => i == expedition);
                //ShowResultSystem.ShowResult(_heroesSlots, expedition);
                ShowResultSystem.ShowResult(_selectedQuest.Quest.Quest.HeroesSlots, expedition);
            }

            else
            {
                Expedition matchingExpedition = _takingQuests.BoardSystem.ExpeditionSlots.FirstOrDefault(i => i == expedition);
                //ShowResultSystem.ShowResult(_heroesSlots, expedition);
                ShowResultSystem.ShowResult(_selectedQuest.Quest.Quest.HeroesSlots, expedition);
            }
        }

        else
        {
            if (_isViewQuests)
            {
                QuestSlot matchingQuest = _viewedQuests.BoardSystem.QuestSlots.FirstOrDefault(i => i == _selectedQuest.Quest);
                //ShowResultSystem.ShowResult(_heroesSlots, _selectedQuest.Quest.Quest);
                ShowResultSystem.ShowResult(_selectedQuest.Quest.Quest.HeroesSlots, _selectedQuest.Quest.Quest);
            }

            else
            {
                QuestSlot matchingQuest = _takingQuests.BoardSystem.QuestSlots.FirstOrDefault(i => i == _selectedQuest.Quest);
                //ShowResultSystem.ShowResult(_heroesSlots, _selectedQuest.Quest.Quest);
                ShowResultSystem.ShowResult(_selectedQuest.Quest.Quest.HeroesSlots, _selectedQuest.Quest.Quest);
            }
        }

        _selectedQuest.Quest.Quest.IsQuestSeen = true;
        _selectedQuest.ViewIcon.gameObject.SetActive(false);

        SetQuestBoardAfterDay();
        RefreshInfo();
        RefreshHeroesSlots();

        NotificationSystem.Instance.CheckNotifications();
    }

    private void RefreshHeroesSlots()
    {
        foreach (HeroQuestSlot_UI heroSlot in _heroesSlots.Slots)
            heroSlot.ClearSlot();
    }

    protected override void CreateQuestSlot(QuestSlot questSlot)
    {
        QuestSlot_UI questSlot_UI = Instantiate(_questPrefab, _questList.transform);
        questSlot_UI.Init(questSlot, _mainQuestKeeperDisplay);
        ChangeColorQuest(questSlot_UI, questSlot.Quest);

        if (questSlot.Quest.RemainDaysToComplete == 0)
        {
            if (questSlot.Quest.IsQuestSeen)
                questSlot_UI.ViewIcon.gameObject.SetActive(false);

            else
                questSlot_UI.ViewIcon.gameObject.SetActive(true);
        }

        else
            questSlot_UI.ViewIcon.gameObject.SetActive(false);
    }

    protected override void CreateExpeditionSlot(Expedition expedition)
    {
        Debug.Log("create exp 2");
        ExpeditionSlot_UI expeditionSlot_UI = Instantiate(_expeditionPrefab, _questList.transform);
        expeditionSlot_UI.Init(expedition, _mainQuestKeeperDisplay);
        ChangeColorQuest(expeditionSlot_UI, expedition);

        if (expedition.RemainDaysToComplete == 0)
        {
            if (expeditionSlot_UI.Expedition.IsQuestSeen)
                expeditionSlot_UI.ViewIcon.gameObject.SetActive(false);

            else
                expeditionSlot_UI.ViewIcon.gameObject.SetActive(true);
        }

        else
            expeditionSlot_UI.ViewIcon.gameObject.SetActive(false);
    }

    private void ChangeColorQuest(QuestSlot_UI questSlot, Quest quest)
    {
        if (quest.RemainDaysToComplete == 0)
        {
            if (quest.IsQuestToSuccess)
                questSlot.ButtonSelf.image.color = new Color(0, 255, 0, 255);

            else
                questSlot.ButtonSelf.image.color = new Color(255, 43, 0, 255);
        }
    }

    private void ChangeTakingOrViewedQuests()
    {
        //if (!_isViewQuests)
        //{
        //    _isViewQuests = true;
        //    DisplayQuestBoard(_viewedQuests.BoardSystem);
        //    _viewedOrTakingQuestsText.text = "Taking Quests";
        //}

        //else
        //{
        //    _isViewQuests = false;
        //    DisplayQuestBoard(_takingQuests.BoardSystem);
        //    _viewedOrTakingQuestsText.text = "Viewed Quests";
        //}
    }

    public void ChangeViewFlag(bool flag)
    {
        _isViewQuests = flag;
    }

    protected override void OpenSwapQuestsPanel()
    {
        _panelDisplay.gameObject.SetActive(false);
        _mainQuestKeeperDisplay.DisplayQuestBoard(_mainQuestKeeper.BoardSystem);
    }
}
