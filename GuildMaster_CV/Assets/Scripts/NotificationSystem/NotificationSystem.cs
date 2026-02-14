using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;

public class NotificationSystem : MonoBehaviour
{
    public static NotificationSystem Instance;

    [SerializeField] private NotificationSlot_UI _notifSlotPrefab;
    [SerializeField] private GameObject _notifSlotsContent;
    [SerializeField] private NextDayController _nextDayController;
    [Header("Keepers")]
    [SerializeField] private GuildKeeper _guildKeeper;
    [SerializeField] private GuildValutes _guildValutes;
    [SerializeField] private QuestKeeper _questKeeper;
    [SerializeField] private PayKeeper _payKeeper;
    [SerializeField] private QuestKeeper _viewedQuestKeeper;
    [SerializeField] private QuestKeeper _takingQuestKeeper;
    [SerializeField] private DrawingKeeperDisplay _drawingKeeperDisplay;
    [SerializeField] private GuildTalentSystem _guildTalentSystem;
    [SerializeField] private WorldEventSystem _worldEventSystem;
    [Header("Icons")]
    [SerializeField] private Sprite _statsPointsHeroIcon;
    [SerializeField] private Sprite _abilityPointsHeroIcon;
    [SerializeField] private Sprite _levelUpGuildIcon;
    [SerializeField] private Sprite _isExpeditionResearch;
    [SerializeField] private Sprite _isNotGoldForWeeklyPay;
    [SerializeField] private Sprite _isQuestShowResult;
    [SerializeField] private Sprite _isNewRecipesUnlock;

    public GuildKeeper GuildKeeper => _guildKeeper;
    public GuildValutes GuildValutes => _guildValutes;
    public QuestKeeper QuestKeeper => _questKeeper;
    public PayKeeper PayKeeper => _payKeeper;
    public QuestKeeper ViewedQuestKeeper => _viewedQuestKeeper;
    public QuestKeeper TakingQuestKeeper => _takingQuestKeeper;
    public DrawingKeeperDisplay DrawingKeeperDisplay => _drawingKeeperDisplay;
    public GuildTalentSystem GuildTalentSystem => _guildTalentSystem;
    public WorldEventSystem WorldEventSystem => _worldEventSystem;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            return;
        }

        else
            Destroy(gameObject);
    }

    private void Start()
    {
        ClearSlots();
        CheckNotifications();
    }

    private void OnEnable()
    {
        _nextDayController.OnNextDay += CheckNotifications;
    }

    private void OnDisable()
    {
        _nextDayController.OnNextDay -= CheckNotifications;
    }

    public void CheckNotifications()
    {
        ClearSlots();

        CheckStatsPointsHeroes();
        CheckAbilityPointsHeroes();
        CheckLevelUpGuild();
        CheckExpeditionResearch();
        CheckWeeklyPay();
        CheckResultQuest();
        CheckNewRecipes();
    }

    private void CreateNotification(Sprite icon, NotificationEnum notifType)
    {
        NotificationSlot_UI notificationSlot = Instantiate(_notifSlotPrefab, _notifSlotsContent.transform);
        notificationSlot.ButtonSelf.image.sprite = icon;
        notificationSlot.SetNotifType(notifType);
    }

    private void CheckStatsPointsHeroes()
    {
        foreach (Hero hero in _guildKeeper.RecruitSystem.HeroSlots)
        {
            if (hero.LevelSystem.StatPoints != 0)
            {
                CreateNotification(_statsPointsHeroIcon, NotificationEnum.HeroStatsPoints);
                break;
            }
        }
    }

    private void CheckAbilityPointsHeroes()
    {
        foreach (Hero hero in _guildKeeper.RecruitSystem.HeroSlots)
        {
            if (hero.LevelSystem.AbilityPoints != 0)
            {
                CreateNotification(_abilityPointsHeroIcon, NotificationEnum.HeroAbilityPoints);
                break;
            }
        }
    }

    private void CheckLevelUpGuild()
    {
        if (_guildValutes.LevelPoints != 0)
        {
            CreateNotification(_levelUpGuildIcon, NotificationEnum.GuildStatPoints);
        }
    }

    private void CheckExpeditionResearch()
    {
        if (_questKeeper == null)
            Debug.Log("null 1");

        if (_questKeeper.BoardSystem == null)
            Debug.Log("null 2");

        if (_questKeeper.BoardSystem.ExpeditionSlots == null)
            Debug.Log("null 3");

        if (_questKeeper.BoardSystem.ExpeditionSlots.Count != 0)
        {
            CreateNotification(_isExpeditionResearch, NotificationEnum.ExpeditionResearch);
        }
    }

    private void CheckWeeklyPay()
    {
        if (_guildValutes.IsNotPaid)
            CreateNotification(_isNotGoldForWeeklyPay, NotificationEnum.NotGoldForWeeklyPay);
    }

    private void CheckResultQuest()
    {
        for (int i = 0; i < _takingQuestKeeper.BoardSystem.QuestSlots.Count; i++)
        {
            if (!_takingQuestKeeper.BoardSystem.QuestSlots[i].Quest.IsQuestSeen && _takingQuestKeeper.BoardSystem.QuestSlots[i].Quest.RemainDaysToComplete == 0)
            {
                CreateNotification(_isQuestShowResult, NotificationEnum.QuestShowResult);
                break;
            }
        }

        for (int i = 0; i < _takingQuestKeeper.BoardSystem.ExpeditionSlots.Count; i++)
        {
            if (!_takingQuestKeeper.BoardSystem.ExpeditionSlots[i].IsQuestSeen && _takingQuestKeeper.BoardSystem.ExpeditionSlots[i].RemainDaysToComplete == 0)
            {
                CreateNotification(_isQuestShowResult, NotificationEnum.QuestShowResult);
                break;
            }
        }
    }

    private void CheckNewRecipes()
    {
        if (_drawingKeeperDisplay.IsWindowChecked == false)
        {
            CreateNotification(_isNewRecipesUnlock, NotificationEnum.NewRecipeUnlock);
        }
    }

    private void ClearSlots()
    {
        foreach (Transform slot in _notifSlotsContent.transform.Cast<Transform>())
        {
            Destroy(slot.gameObject);
        }
    }
}
