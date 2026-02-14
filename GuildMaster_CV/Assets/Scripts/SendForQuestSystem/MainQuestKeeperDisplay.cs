using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MainQuestKeeperDisplay : MonoBehaviour, ILearning
{
    [SerializeField] protected QuestSlot_UI _questPrefab;
    [SerializeField] protected ExpeditionSlot_UI _expeditionPrefab;
    [SerializeField] protected GameObject _questList;
    [SerializeField] protected ExpeditionProgress _expeditionProgress;
    [SerializeField] protected Image _panelDisplay;
    [SerializeField] private QuestKeeper _viewQuestKeeper;
    [SerializeField] protected QuestKeeper _mainQuestKeeper;
    [SerializeField] protected NextDayController _nextDayController;
    [Header("Data Info")]
    [SerializeField] protected GameObject _enemyInfo;
    [SerializeField] protected GameObject _heroInfo;
    [SerializeField] protected GameObject _enemySlots;
    [SerializeField] protected GameObject _affixesSlots;
    [SerializeField] private GameObject _sendHeroInfo;
    [SerializeField] protected SendHeroesSlots _heroesSlots;
    [Header("Take Quest")]
    [SerializeField] private Button _takeQuest;
    [SerializeField] private TakingQuestUIController _takingQuestUIController;
    [SerializeField] private DynamicQuestList _dynamicQuestList;
    [SerializeField] private QuestKeeper _takingQuestKeeper;
    [Header("Up Right Corner Buttons")]
    [SerializeField] private Button _swapQuestBoardButton;
    [Header("Notification Settings")]
    [SerializeField] protected TextMeshProUGUI _notificationText;
    [SerializeField] protected float _timeNotif;
    [SerializeField] protected string _selectQuestNotif;
    [Header("Other UI")]
    [SerializeField] protected TextMeshProUGUI _expeditionDaysLeft;
    [SerializeField] protected QuestSlot_UI _selectedQuest;
    [SerializeField] protected QuestSlot_UI _lastSelectedQuest;
    [Header("Talent Properties")]
    [SerializeField] private float _coefGuildRep;
    [SerializeField] private float _coefExpHero;
    [SerializeField] private float _coefGold;
    [Header("Learning Properties")]
    [SerializeField] private Button _learnButton;
    [SerializeField] private bool _learnCheck = false;
    [SerializeField] private List<StringListContainer> _questBoardAllHelp = new List<StringListContainer>();
    [SerializeField] private List<StringListContainer> _questBoardHelp = new List<StringListContainer>();

    protected QuestBoardSystem _boardSystem;
    [SerializeField] protected HeroQuestSlot_UI _selectedHero;
    private Coroutine _currentNotificationRoutine;

    public event UnityAction OnQuestClick;
    public event UnityAction OnTakeQuest;
    public event UnityAction OnPanelOpen;
    public static event UnityAction OnPanelClose;
    public event UnityAction<HeroQuestSlot_UI> OnEmptyHeroSlotClick;
    public event UnityAction<HeroQuestSlot_UI> OnFillHeroSlotClick;

    public SendHeroesSlots HeroesSlots => _heroesSlots;

    public QuestSlot_UI SelectedQuest => _selectedQuest;
    public QuestSlot_UI LastSelectedQuest => _lastSelectedQuest;
    public HeroQuestSlot_UI SelectedHero => _selectedHero;
    public float CoefGuildRep => _coefGuildRep;
    public float CoefExpHero => _coefExpHero;
    public float CoefGold => _coefGold;
    public string SelectQuestNotif => _selectQuestNotif;

    public QuestKeeperController QuestKeeperController { get; private set; }
    public QuestParametresSystemFix QuestParametresSystemFix { get; private set; }
    public PossibleRewardsSystem PossibleRewardsSystem { get; private set; }
    public QuestResistanceSystem QuestResistanceSystem { get; private set; }
    public RecruitQuestKeeperController RecruitQuestKeeperController { get; private set; }
    public PrepareQuestUIController PrepareQuestUIController { get; private set; }

    protected virtual void Awake()
    {
        this.gameObject.SetActive(true);

        ClearNotificationInfo();

        QuestKeeperController = GetComponent<QuestKeeperController>();
        QuestParametresSystemFix = GetComponent<QuestParametresSystemFix>();
        PossibleRewardsSystem = GetComponent<PossibleRewardsSystem>();
        QuestResistanceSystem = GetComponent<QuestResistanceSystem>();
        RecruitQuestKeeperController = GetComponent<RecruitQuestKeeperController>();
        PrepareQuestUIController = GetComponent<PrepareQuestUIController>();

        _takeQuest?.onClick.AddListener(TakeQuest);
        _swapQuestBoardButton?.onClick.AddListener(OpenSwapQuestsPanel);

        if (_learnButton != null)
            _learnButton?.onClick.AddListener(SetLearningTextByButton);

        _expeditionProgress.gameObject.SetActive(false);
        _expeditionDaysLeft.text = "";

        _panelDisplay.gameObject.SetActive(false);
    }

    protected virtual void Start()
    {
        SetQuestBoardAfterDay();
    }

    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (_panelDisplay.gameObject.activeSelf)
                _panelDisplay.gameObject.SetActive(false);

            else
            {
                DisplayQuestBoard(null);
            }
        }

        if (_panelDisplay.gameObject.activeSelf)
            SetSelectHeroByKey();
    }

    public virtual void DisplayQuestBoard(QuestBoardSystem boardSystem)
    {
        if (_nextDayController.InTransition)
            return;

        if (!_panelDisplay.gameObject.activeSelf)
        {
            _panelDisplay.gameObject.SetActive(true);
            PrepareQuestUIController.ClearHeroResImages();

            SetLearningTextFirst();

            if (_selectedHero != null && _selectedHero.Hero != null && _selectedHero.Hero.HeroData != null)
                PrepareQuestUIController.ShowHeroAbilities();

            if (_selectedQuest == null)
            {
                _enemySlots.gameObject.SetActive(false);
                _affixesSlots.gameObject.SetActive(false);
            }

            if (RecruitQuestKeeperController != null)
                RecruitQuestKeeperController.UpdateHeroList();

            QuestParametresSystemFix.CheckRequiresQuest();

            OnPanelOpen?.Invoke();
        }

        //SetQuestBoardAfterDay();
    }

    public virtual void SetQuestBoardAfterDay() // подписан на NExtDayController через событие в инспекторе
    {
        _boardSystem = _mainQuestKeeper.BoardSystem;

        ClearSlots();
        RefreshInfo();

        foreach (Expedition expedition in _boardSystem.ExpeditionSlots)
        {
            if (expedition.ExpeditionData == null)
                continue;

            CreateExpeditionSlot(expedition);
        }

        foreach (QuestSlot quest in _boardSystem.QuestSlots)
        {
            if (quest.Quest.QuestData == null)
                continue;

            CreateQuestSlot(quest);
        }

        _enemySlots.gameObject.SetActive(true);
        _affixesSlots.gameObject.SetActive(true);
    }

    protected void ClearSlots()
    {
        ClearQuestSlots();
    }

    protected virtual void CreateQuestSlot(QuestSlot questSlot)
    {
        QuestSlot_UI questSlot_UI = Instantiate(_questPrefab, _questList.transform);
        questSlot_UI.Init(questSlot, this);
    }

    protected virtual void CreateExpeditionSlot(Expedition expedition)
    {
        ExpeditionSlot_UI expeditionSlot_UI = Instantiate(_expeditionPrefab, _questList.transform);
        expeditionSlot_UI.Init(expedition, this);
    }

    private void ClearQuestSlots()
    {
        foreach (Transform quest in _questList.transform.Cast<Transform>())
        {
            Destroy(quest.gameObject);
        }
    }

    public virtual void SelectQuest(QuestSlot_UI questSlot)
    {
        if (_selectedQuest != null && _selectedQuest.Quest != null && _selectedQuest.Quest.Quest != null)
        {
            _lastSelectedQuest = _selectedQuest;
            HighlightSlot(_lastSelectedQuest, false);
        }

        else
        {
            _lastSelectedQuest = questSlot;
        }

        RefreshInfo();

        _selectedQuest = questSlot;
        HighlightSlot(_selectedQuest, true);

        if (PossibleRewardsSystem != null && PossibleRewardsSystem.Panel.gameObject.activeSelf)
            PossibleRewardsSystem.ShowWindow();

        if (questSlot is ExpeditionSlot_UI expeditionSlot)
        {
            if (!_expeditionProgress.gameObject.activeSelf)
            {
                _expeditionProgress.gameObject.SetActive(true);
            }


            _expeditionProgress.ShowProgress(expeditionSlot.Expedition);
        }

        else
        {
            if (_expeditionProgress.gameObject.activeSelf)
            {
                _expeditionProgress.gameObject.SetActive(false);
                _expeditionDaysLeft.text = "";
            }
        }

        OnQuestClick?.Invoke();
    }

    public void SelectHero(HeroQuestSlot_UI heroSlot)
    {
        RefreshInfo();
        _selectedHero = heroSlot;

        if (heroSlot.Hero == null || heroSlot.Hero.HeroData == null)
        {
            OnEmptyHeroSlotClick?.Invoke(heroSlot);
        }

        else
        {
            OnFillHeroSlotClick?.Invoke(heroSlot);
        }
    }

    private void HighlightSlot(QuestSlot_UI questSlot, bool highlight)
    {
        //questSlot.ButtonSelf.image.color = highlight ? Color.yellow : Color.black;
        questSlot.ButtonSelf.targetGraphic.color = highlight ? Color.white : new Color(144f / 255f, 144f / 255f, 144f / 255f);
    }

    protected virtual void RefreshInfo()
    {
        _enemyInfo.SetActive(false);
        _heroInfo.SetActive(false);
        _enemySlots.SetActive(false);
        _affixesSlots.SetActive(false);
        _sendHeroInfo.SetActive(false);

        QuestResistanceSystem.ClearAllIcons();
    }

    protected virtual void OpenSwapQuestsPanel()
    {
        _panelDisplay.gameObject.SetActive(false);
        _takingQuestUIController.ChangeViewFlag(false);
        _takingQuestUIController.DisplayQuestBoard(null);
    }

    protected virtual void TakeQuest()
    {
        if (_selectedQuest == null || _selectedQuest.Quest.Quest == null)
        {
            SetNotificationText(_selectQuestNotif);
            return;
        }

        if (_selectedQuest.Quest.Quest is Expedition expedition)
        {
            Expedition newTakingExpedition = new Expedition(expedition, QuestParametresSystemFix);

            _takingQuestKeeper.BoardSystem.AddNewExpedition(newTakingExpedition);
            _dynamicQuestList.ExpeditionList.Remove(newTakingExpedition);

            _boardSystem.RemoveExpedition(expedition);

            SetHeroesForTakingQuest(newTakingExpedition);
        }

        else
        {
            Quest newTakingQuest = new Quest(_selectedQuest.Quest.Quest, QuestParametresSystemFix);
            QuestSlot questSlotTaking = new QuestSlot(newTakingQuest);

            _takingQuestKeeper.BoardSystem.AddNewQuest(newTakingQuest);
            _dynamicQuestList.QuestsList.Remove(newTakingQuest);
            _boardSystem.RemoveQuest(_selectedQuest.Quest);

            SetHeroesForTakingQuest(newTakingQuest);
        }



        //if (newTakingQuest is Expedition newExpedition)
        //{
        //    Debug.Log("add new exp 1");
        //    _takingQuestKeeper.BoardSystem.AddNewExpedition(newExpedition);
        //    _dynamicQuestList.ExpeditionList.Remove(newExpedition);

        //    if(_selectedQuest.Quest.Quest is Expedition oldSlotExpedition)
        //    _boardSystem.RemoveExpedition(oldSlotExpedition);
        //}

        //else
        //{
        //    _takingQuestKeeper.BoardSystem.AddNewQuest(newTakingQuest);
        //    _dynamicQuestList.QuestsList.Remove(newTakingQuest);
        //    _boardSystem.RemoveQuest(_selectedQuest.Quest);
        //}

        ClearQuestSlots();

        foreach (Expedition expeditionExist in _boardSystem.ExpeditionSlots)
        {
            if (expeditionExist.ExpeditionData == null)
                continue;

            CreateExpeditionSlot(expeditionExist);
        }

        foreach (QuestSlot questExist in _boardSystem.QuestSlots)
        {
            if (questExist.Quest.QuestData == null)
                continue;

            CreateQuestSlot(questExist);
        }

        QuestKeeperController.ClearAllSlots();

        //for (int i = 0; i < _heroesSlots.Slots.Count; i++)
        //{
        //    if (_heroesSlots.Slots[i].Hero != null && _heroesSlots.Slots[i].Hero.HeroData != null)
        //    {
        //        newTakingQuest.HeroesSlots.Add(_heroesSlots.Slots[i].Hero);

        //        Hero newHero = new Hero(_heroesSlots.Slots[i].Hero);
        //        newTakingQuest.VisibleHeroesSlots.Add(i, newHero);
        //        newTakingQuest.TestHeroesSlots.Add(newHero);

        //        _heroesSlots.Slots[i].Hero.IsSentOnQuest = true;
        //        _heroesSlots.Slots[i].DeleteHero();
        //    }

        //    else
        //    {
        //        newTakingQuest.HeroesSlots.Add(null);
        //    }
        //}

        RefreshInfo();
        OnTakeQuest?.Invoke();

        NotificationSystem.Instance.CheckNotifications();
    }

    private void SetHeroesForTakingQuest(Quest newTakingQuest)
    {
        for (int i = 0; i < _heroesSlots.Slots.Count; i++)
        {
            if (_heroesSlots.Slots[i].Hero != null && _heroesSlots.Slots[i].Hero.HeroData != null)
            {
                newTakingQuest.HeroesSlots.Add(_heroesSlots.Slots[i].Hero);

                Hero newHero = new Hero(_heroesSlots.Slots[i].Hero);
                newTakingQuest.VisibleHeroesSlots.Add(i, newHero);
                newTakingQuest.TestHeroesSlots.Add(newHero);

                _heroesSlots.Slots[i].Hero.IsSentOnQuest = true;
                _heroesSlots.Slots[i].DeleteHero();
            }

            else
            {
                newTakingQuest.HeroesSlots.Add(null);
            }
        }
    }

    public void SetNotificationText(string text)
    {
        if (_currentNotificationRoutine != null)
            StopCoroutine(_currentNotificationRoutine);

        _notificationText.gameObject.SetActive(true);
        _notificationText.text = $"{text}";

        if (_notificationText.TryGetComponent<CanvasGroup>(out var canvasGroup))
            canvasGroup.alpha = 1f;

        _currentNotificationRoutine = StartCoroutine(HideNotificationAfterDelay(_timeNotif));
    }

    private IEnumerator HideNotificationAfterDelay(float displayTime)
    {
        yield return new WaitForSeconds(displayTime);

        float _fadeDuration = 0.5f;

        // Плавное исчезновение
        if (_notificationText.TryGetComponent<CanvasGroup>(out var canvasGroup))
        {
            float elapsed = 0f;
            while (elapsed < _fadeDuration)
            {
                canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / _fadeDuration);
                elapsed += Time.deltaTime;
                yield return null;
            }
        }

        _notificationText.gameObject.SetActive(false);
        _currentNotificationRoutine = null;
    }

    private void ClearNotificationInfo()
    {
        if (_notificationText != null)
        {
            _notificationText.text = "";
            _notificationText.gameObject.SetActive(false);
        }
    }

    private void SetSelectHeroByKey()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectHero(HeroesSlots.Slots[0]);
            _selectedHero.ClickedSlot();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectHero(HeroesSlots.Slots[1]);
            _selectedHero.ClickedSlot();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectHero(HeroesSlots.Slots[2]);
            _selectedHero.ClickedSlot();
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SelectHero(HeroesSlots.Slots[3]);
            _selectedHero.ClickedSlot();
        }
    }

    public void ChangeGuildRepByTalent(float coefGuildRep)
    {
        _coefGuildRep += coefGuildRep;
    }

    public void ChangeHeroExpByTalent(float coefExpHero)
    {
        _coefExpHero += coefExpHero;
    }

    public void ChangeGoldQuestByTalent(float coefGold)
    {
        _coefGold += coefGold;
    }

    public void SetLearningTextByButton()
    {
        LearningGameManager.Instance.SetText(_questBoardAllHelp);
    }

    public void SetLearningTextFirst()
    {
        if (!_learnCheck)
        {
            LearningGameManager.Instance.SetText(_questBoardHelp);
            _learnCheck = true;
        }
    }
}
