using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExpeditionSlot_UI : QuestSlot_UI
{
    [SerializeField] private TextMeshProUGUI _amountStage;
    [SerializeField] private TextMeshProUGUI _currentStage;
    [SerializeField] private Expedition _expedition;

    public Expedition Expedition => _expedition;

    protected override void Awake()
    {
        base.Awake();
    }

    public void Init(Expedition expedition, MainQuestKeeperDisplay mainQuestKeeperDisplay)
    {
        MainQuestKeeperDisplay = mainQuestKeeperDisplay;

        _quest = new QuestSlot(expedition);
        _quest.AssignQuest(expedition);
        _expedition = expedition;
        UpdateUISlot(expedition);
    }

    protected void UpdateUISlot(Expedition expedition)
    {
        if (expedition != null)
        {
            _levelText.text = $"Level: {expedition.Level}";
            _powerText.text = $"Power: {expedition.Power}";
            _defenceText.text = $"Defence: {expedition.Defence}";
            _currentStage.text = $"Stage: {expedition.CurrentStage}";
            _region.text = $"Region: {expedition.RegionData.RegionName}";
        }
    }

    protected override void SlotClicled()
    {
        base.SlotClicled();
    }
}
