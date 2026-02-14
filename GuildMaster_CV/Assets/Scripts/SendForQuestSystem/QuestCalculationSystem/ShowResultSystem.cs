using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ShowResultSystem : MonoBehaviour
{
    [Header("Heroes Rewards")]
    [SerializeField] private List<Image> _heroesSlots;
    [SerializeField] private List<WoundSlot_UI> _heroesWounds;
    [SerializeField] private List<Slider> _heroesCurrentExp;
    [SerializeField] private List<Slider> _heroesAddExp;
    [SerializeField] private List<TextMeshProUGUI> _expHeroesText;
    [SerializeField] private List<TextMeshProUGUI> _levelHeroesText;
    [Header("Guild Rewards")]
    [SerializeField] private TextMeshProUGUI _goldReward;
    [SerializeField] private TextMeshProUGUI _reputationTextReward;
    [SerializeField] private TextMeshProUGUI _levelGuild;
    [SerializeField] private Slider _reputationSliderReward;
    [SerializeField] private Slider _reputationSliderAdd;
    [SerializeField] private TextMeshProUGUI _resultQuest;
    [SerializeField] private ExpeditionProgress _expeditionResultProgress;
    [Header("Content Lists")]
    [SerializeField] private GameObject _lostItems;
    [SerializeField] private GameObject _rewardItems;
    [Header("Other")]
    [SerializeField] private CalculationResultsQuest _calculationResultsQuest;
    [SerializeField] private GuildValutes _guild;
    [SerializeField] private Button _closeButton;
    [SerializeField] private SlotResult_UI _slotResultPrefab;
    [SerializeField] private Image _panel;
    [SerializeField] private Sprite _defaulImageHeroSlot;

    public CalculationResultsQuest CalculationResultsQuest => _calculationResultsQuest;

    private void Awake()
    {
        _panel.gameObject.SetActive(false);

        _closeButton?.onClick.AddListener(ClosePanel);

        ClearLostSlots();
        ClearRewardSlots();

        ClearInfo();
    }

    private void OnEnable()
    {
        _panel.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        _panel.gameObject.SetActive(false);

        ClearInfo();
        ClearLostSlots();
        ClearRewardSlots();
    }

    public void ShowResult(List<Hero> heroesSlots, Quest quest)
    {
        if (quest is Expedition)
        {
            if (!_expeditionResultProgress.gameObject.activeSelf)
                _expeditionResultProgress.gameObject.SetActive(true);
        }

        else
        {
            if (_expeditionResultProgress.gameObject.activeSelf)
                _expeditionResultProgress.gameObject.SetActive(false);
        }

        _panel.gameObject.SetActive(true);

        ClearRewardSlots();
        ClearLostSlots();
        ClearInfo();



        if (quest.IsQuestToSuccess)
            QuestSuccessed(heroesSlots, quest);

        else
            QuestFailed(heroesSlots, quest);

        for (int i = 0; i < _heroesSlots.Count; i++)
        {
            if (heroesSlots[i] != null && heroesSlots[i].HeroData != null)
                _heroesSlots[i].sprite = heroesSlots[i].HeroData.Icon;
        }

        for (int i = 0; i < _heroesWounds.Count; i++)
        {
            if (heroesSlots[i] != null && heroesSlots[i].HeroData != null)
                _heroesWounds[i].SetImage(heroesSlots[i].WoundType);
        }
    }

    private void QuestSuccessed(List<Hero> heroesSlots, Quest quest)
    {
        _resultQuest.text = "SUCCESS";

        if (quest.UpLevelGuild == true)
            _levelGuild.color = Color.green;

        else
            _levelGuild.color = Color.black;

        _goldReward.text = $"Gold: {quest.RewardGold}";
        _reputationTextReward.text = $"{quest.GuildCurrentRep} / {quest.GuildRequiredRep}";

        _reputationSliderAdd.maxValue = quest.GuildRequiredRep;
        _reputationSliderAdd.value = quest.GuildCurrentRep;

        _reputationSliderReward.maxValue = quest.GuildRequiredRep;
        _reputationSliderReward.value = quest.GuildCurrentRep - quest.RewardGuildReputation;

        Image fillImageRep = _reputationSliderAdd.fillRect.GetComponent<Image>();
        fillImageRep.color = Color.green;

        _levelGuild.text = $"{_guild.Level}";

        foreach (RewardItems item in quest.SaveRewardItems)
        {
            SlotResult_UI itemReward = Instantiate(_slotResultPrefab, _rewardItems.transform);

            itemReward.Init(item.RewardItem, item.AmountItems);
        }

        foreach (BlankSlot blankSlot in quest.SaveRewardBlanks)
        {
            SlotResult_UI itemReward = Instantiate(_slotResultPrefab, _rewardItems.transform);

            itemReward.Init(blankSlot.BlankSlotData, 1);
        }

        for (int i = 0; i < heroesSlots.Count; i++)
        {
            if (heroesSlots[i] != null &&  heroesSlots[i].HeroData != null)
            {
                _levelHeroesText[i].text = $"{heroesSlots[i].LevelSystem.Level}";

                if (heroesSlots[i].WoundType == WoundsType.Dead)
                {
                    _heroesAddExp[i].maxValue = quest.HeroesRequiredExp[i];
                    _heroesAddExp[i].value = quest.HeroesCurrentExp[i];

                    Image fillImage = _heroesAddExp[i].fillRect.GetComponent<Image>();
                    fillImage.color = Color.red;

                    _heroesCurrentExp[i].maxValue = 0;
                    _heroesCurrentExp[i].value = 0;

                    foreach (EquipSlot item in heroesSlots[i].EquipHolder.EquipSystem.Slots)
                    {
                        if (item != null && item.EquipItemData != null)
                        {
                            if (item.EquipItemData.EquipType == EquipType.Trinket || item.EquipItemData.EquipType == EquipType.Light)
                                Debug.Log("consumable items lost");

                            else
                            {
                                SlotResult_UI itemReward = Instantiate(_slotResultPrefab, _lostItems.transform);
                                itemReward.Init(item.EquipItemData, item.StackSize);
                            }
                        }
                    }
                }

                else
                {
                    if (quest.UpLevelHeroes[i] == true)
                        _levelHeroesText[i].color = Color.green;

                    else
                        _levelHeroesText[i].color = Color.black;

                    _heroesCurrentExp[i].maxValue = quest.HeroesRequiredExp[i];
                    _heroesCurrentExp[i].value = quest.HeroesCurrentExp[i] - quest.RewardHeroExp;

                    _heroesAddExp[i].maxValue = quest.HeroesRequiredExp[i];
                    _heroesAddExp[i].value = quest.HeroesCurrentExp[i];

                    Image fillImage = _heroesAddExp[i].fillRect.GetComponent<Image>();
                    fillImage.color = Color.green;
                }
            }
        }

        for (int i = 0; i < _expHeroesText.Count; i++)
        {
            if (heroesSlots[i] != null&& heroesSlots[i].HeroData != null)
                _expHeroesText[i].text = $"{quest.HeroesCurrentExp[i]} / {quest.HeroesRequiredExp[i]}";
        }

        if (quest is Expedition expedition)
            _expeditionResultProgress.ShowSuccessedResult(expedition);
    }

    private void QuestFailed(List<Hero> heroesSlots, Quest quest)
    {
        Image fillImageRep = _reputationSliderAdd.fillRect.GetComponent<Image>();
        fillImageRep.color = Color.red;

        _resultQuest.text = "FAIL";

        _reputationTextReward.text = $"{quest.GuildCurrentRep} / {quest.GuildRequiredRep}";
        _goldReward.text = $"Reputation lost: {quest.RewardGuildReputation}";

        _reputationSliderAdd.maxValue = quest.GuildRequiredRep;
        _reputationSliderReward.maxValue = quest.GuildRequiredRep;

        if (quest.GuildCurrentRep >= quest.RewardGuildReputation)
        {
            _reputationSliderAdd.value = quest.GuildCurrentRep;
            _reputationSliderReward.value = quest.GuildCurrentRep - Mathf.Abs(quest.RewardGuildReputation);
        }

        else
        {
            _reputationSliderAdd.value = quest.GuildCurrentRep;
            _reputationSliderReward.value = 0;
        }

        for (int i = 0; i < heroesSlots.Count; i++)
        {
            if (heroesSlots[i] != null && heroesSlots[i].HeroData != null)
            {
                _levelHeroesText[i].text = $"{heroesSlots[i].LevelSystem.Level}";

                _heroesAddExp[i].maxValue = quest.HeroesRequiredExp[i];
                _heroesAddExp[i].value = quest.HeroesCurrentExp[i];

                Image fillImage = _heroesAddExp[i].fillRect.GetComponent<Image>();
                fillImage.color = Color.red;

                _heroesCurrentExp[i].maxValue = 0;
                _heroesCurrentExp[i].value = 0;

                foreach (EquipSlot item in heroesSlots[i].EquipHolder.EquipSystem.Slots)
                {
                    if (item != null && item.EquipItemData != null)
                    {
                        if (item.EquipItemData.EquipType == EquipType.Trinket || item.EquipItemData.EquipType == EquipType.Light)
                            Debug.Log("consumable items lost");

                        else
                        {
                            SlotResult_UI itemReward = Instantiate(_slotResultPrefab, _lostItems.transform);
                            itemReward.Init(item.EquipItemData, item.StackSize);
                        }
                    }
                }
            }
        }

        for (int i = 0; i < _expHeroesText.Count; i++)
        {
            if (heroesSlots[i] != null  && heroesSlots[i].HeroData != null)
                _expHeroesText[i].text = $"{quest.HeroesCurrentExp[i]} / {quest.HeroesRequiredExp[i]}";
        }

        if (quest is Expedition expedition)
            _expeditionResultProgress.ShowProgress(expedition);
    }

    private void ClearInfo()
    {
        for (int i = 0; i < _expHeroesText.Count; i++)
        {
            _expHeroesText[i].text = "0 / 0";
        }

        foreach (Image heroImage in _heroesSlots)
        {
            if (heroImage != null)
                heroImage.sprite = _defaulImageHeroSlot;
        }

        foreach (WoundSlot_UI woundImage in _heroesWounds)
        {
            if (woundImage != null && woundImage.MainIcon != null)
                woundImage.MainIcon.sprite = null;
        }

        foreach (Slider sliderAddExp in _heroesAddExp)
        {
            sliderAddExp.maxValue = 0;
            sliderAddExp.value = 0;
        }

        foreach (Slider sliderCurrentExp in _heroesCurrentExp)
        {
            sliderCurrentExp.maxValue = 0;
            sliderCurrentExp.value = 0;
        }

        for (int i = 0; i < _levelHeroesText.Count; i++)
        {
            _levelHeroesText[i].text = "";
        }

        _reputationSliderReward.maxValue = 0;
        _reputationSliderReward.value = 0;

        _reputationSliderAdd.maxValue = 0;
        _reputationSliderAdd.value = 0;

        _resultQuest.text = "";
        _goldReward.text = "";
        _reputationTextReward.text = "";
        _levelGuild.text = "";
    }

    private void ClosePanel()
    {
        _panel.gameObject.SetActive(false);
    }

    private void ClearRewardSlots()
    {
        foreach (Transform item in _rewardItems.transform.Cast<Transform>())
        {
            Destroy(item.gameObject);
        }
    }

    private void ClearLostSlots()
    {
        foreach (Transform item in _lostItems.transform.Cast<Transform>())
        {
            Destroy(item.gameObject);
        }
    }
}
