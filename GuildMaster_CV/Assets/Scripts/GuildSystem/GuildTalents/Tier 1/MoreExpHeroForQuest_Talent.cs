using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoreExpHeroForQuest_Talent : GuildTalentSlot_UI
{
    [Header("Talent Attributes")]
    [SerializeField] private float _coefExpHero;
    [SerializeField] private CalculationResultsQuest _calculationResultsQuest;
    [SerializeField] private MainQuestKeeperDisplay _mainQuestKeeperDisplay;

    public override void RealizationTalent()
    {
        base.RealizationTalent();
        _calculationResultsQuest.ChangeHeroExpByTalent(_coefExpHero / 100);
        _mainQuestKeeperDisplay.ChangeHeroExpByTalent(_coefExpHero / 100);
    }
}
