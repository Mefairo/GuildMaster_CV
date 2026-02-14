using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraDefenceStats_Talent : GuildTalentSlot_UI
{
    [Header("Talent Attributes")]
    [SerializeField] private int _defenceStatValue;
    [SerializeField] private GuildKeeper _guildKeeper;
    [SerializeField] private RecruitKeeper _recruitKeeper;

    public int DefenceStatValue => _defenceStatValue;

    public override void RealizationTalent()
    {
        base.RealizationTalent();
        //_guildKeeper.ExtraDefenceStatsByTalent(_defenceStatValue);
        //_recruitKeeper.ChangeDefenceStatCoefTalent(_defenceStatValue);
    }
}
