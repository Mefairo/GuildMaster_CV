using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReduceDayResearch_Talent : GuildTalentSlot_UI
{
    [Header("Talent Attributes")]
    [SerializeField] private int _dayResearch;
    [SerializeField] private ExpeditionalCompany _expeditionalCompany;

    public override void RealizationTalent()
    {
        base.RealizationTalent();
        _expeditionalCompany.ChangeDayResearchByTalent(_dayResearch);
    }
}
