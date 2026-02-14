using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddGold_Talent : GuildTalentSlot_UI
{
    [Header("Talent Attributes")]
    [SerializeField] private GuildValutes _guildValutes;

    public override void RealizationTalent()
    {
        base.RealizationTalent();
        _guildValutes.AddGoldByTalent();
    }
}
