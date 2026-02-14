using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoreGoldForQuest_Talent : GuildTalentSlot_UI
{
    [Header("Talent Attributes")]
    [SerializeField] private float _coefGold;
    [SerializeField] private MainQuestKeeperDisplay _mainQuestKeeperDisplay;
    [SerializeField] private CalculationResultsQuest _calculationResultsQuest;

    public override void RealizationTalent()
    {
        base.RealizationTalent();
        _calculationResultsQuest.ChangeGoldQuestByTalent(_coefGold / 100);
        _mainQuestKeeperDisplay.ChangeGoldQuestByTalent(_coefGold / 100);
    }
}
