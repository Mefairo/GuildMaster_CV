using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraPowerStats_Talent : GuildTalentSlot_UI
{
    [Header("Talent Attributes")]
    [SerializeField] private int _powerStatValue;
    [SerializeField] private GuildKeeper _guildKeeper;
    [SerializeField] private RecruitKeeper _recruitKeeper;

    public int PowerStatValue => _powerStatValue;

    public override void RealizationTalent()
    {
        base.RealizationTalent();
        //_guildKeeper.ExtraPowerStatsByTalent(_powerStatValue);
        //_recruitKeeper.ChangePowerStatCoefTalent(_powerStatValue);
    }
}
