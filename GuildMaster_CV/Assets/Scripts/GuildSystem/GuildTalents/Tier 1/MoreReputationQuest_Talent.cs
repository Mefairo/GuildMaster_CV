using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoreReputationQuest_Talent : GuildTalentSlot_UI
{
    [Header("Talent Attributes")]
    [SerializeField] private float _coefReputation;
    [SerializeField] private GuildValutes _guildValutes;
    [SerializeField] private MainQuestKeeperDisplay _mainQuestKeeperDisplay;

    public override void RealizationTalent()
    {
        base.RealizationTalent();
        _guildValutes.ChangeCoefReputationByTalent(_coefReputation / 100);
        _mainQuestKeeperDisplay.ChangeGuildRepByTalent(_coefReputation / 100);
    }
}
