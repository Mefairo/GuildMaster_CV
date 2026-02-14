using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class QuestSlot_UI : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI _levelText;
    [SerializeField] protected TextMeshProUGUI _powerText;
    [SerializeField] protected TextMeshProUGUI _defenceText;
    [SerializeField] protected TextMeshProUGUI _region;
    [SerializeField] protected TextMeshProUGUI _goldReward;
    [SerializeField] protected TextMeshProUGUI _guildExp;
    [SerializeField] protected TextMeshProUGUI _heroExp;
    [SerializeField] protected Button _buttonSelf;
    [SerializeField] protected Image _viewIcon;
    [Space]
    [SerializeField] protected QuestSlot _quest;
    public MainQuestKeeperDisplay MainQuestKeeperDisplay;
    public TakingQuestUIController TakingQuestUIController;

    public QuestKeeperDisplay QuestKeeperDisplay { get; private set; }
    public QuestResistanceSystem QuestResistanceSystem { get; private set; }
    //public MainQuestKeeperDisplay MainQuestKeeperDisplay { get; private set; }
    //public TakingQuestUIController TakingQuestUIController { get; private set; }
    public QuestParametresSystemFix QuestParametresSystemFix { get; private set; }

    public QuestSlot Quest => _quest;
    public Button ButtonSelf => _buttonSelf;
    public Image ViewIcon => _viewIcon;


    protected virtual void Awake()
    {
        _buttonSelf?.onClick.AddListener(SlotClicled);

        QuestKeeperDisplay = GetComponentInParent<QuestKeeperDisplay>();
        //MainQuestKeeperDisplay = GetComponentInParent<MainQuestKeeperDisplay>();
        TakingQuestUIController = GetComponentInParent<TakingQuestUIController>();

        QuestParametresSystemFix = GetComponentInParent<QuestParametresSystemFix>();

        _viewIcon.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (_quest != null)
            UpdateUISlot();
    }

    //private void OnDisable()
    //{
    //    ClearUISlot();
    //}

    public virtual void Init(QuestSlot slot, MainQuestKeeperDisplay mainQuestKeeperDisplay)
    {
        _quest = slot;
        MainQuestKeeperDisplay = mainQuestKeeperDisplay;

        UpdateUISlot();
        _region.text = $"Region: {_quest.Quest.RegionData.RegionName}"; // какой то баг, почему то выдает nullref в UpdateUISlot
    }

    protected virtual void UpdateUISlot()
    {
        if (MainQuestKeeperDisplay == null)
            return;

        if (_quest != null)
        {
            _levelText.text = $"Level: {_quest.Quest.Level}";
            _powerText.text = $"Power: {_quest.Quest.Power}";
            _defenceText.text = $"Defence: {_quest.Quest.Defence}";

            if (TakingQuestUIController == null)
            {
                int newGold = (int)(_quest.Quest.RewardGold * MainQuestKeeperDisplay.CoefGold);
                _goldReward.text = $"{newGold}";

                int newRep = (int)(_quest.Quest.RewardGuildReputation * MainQuestKeeperDisplay.CoefGuildRep);
                if (_guildExp != null) _guildExp.text = $"Guild Rep: {newRep}";

                int newHeroExp = (int)(_quest.Quest.RewardHeroExp * MainQuestKeeperDisplay.CoefExpHero);
                if (_heroExp != null) _heroExp.text = $"Hero Exp: {newHeroExp}";
            }

            else
            {
                _goldReward.text = $"{_quest.Quest.RewardGold}";
                if (_guildExp != null) _guildExp.text = $"Guild Rep: {_quest.Quest.RewardGuildReputation}";
                if (_heroExp != null) _heroExp.text = $"Hero Exp: {_quest.Quest.RewardHeroExp}";
            }
        }
    }

    public void ClearUISlot()
    {
        _levelText.text = "";
        _powerText.text = "";
        _defenceText.text = "";
        _region.text = "";

        _goldReward.text = "";
        if (_guildExp != null) _guildExp.text = "";
        if (_heroExp != null) _heroExp.text = "";
    }

    protected virtual void SlotClicled()
    {
        if (QuestKeeperDisplay != null)
        {
            QuestKeeperDisplay.SelectQuest(this);
            QuestKeeperDisplay.UpdateQuestPreview(this);
        }

        //if (QuestResistanceSystem != null)
        //    QuestResistanceSystem.ShowResistance(this);

        if (MainQuestKeeperDisplay != null)
        {
            MainQuestKeeperDisplay.SelectQuest(this);
        }

        if (TakingQuestUIController != null)
        {
            TakingQuestUIController.SelectTakingQuest(this);
        }

        if (QuestParametresSystemFix != null)
        {
            QuestParametresSystemFix.CheckRequiresQuest();
            //QuestParametresSystem.ApplyHeroAbilityEffect(this);
        }
    }
}
