using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoreGuildPlaces_Talent : GuildTalentSlot_UI
{
    [Header("Talent Attributes")]
    [SerializeField] private int _extraPlaces;
    [SerializeField] private GuildValutes _guildValutes;

    public override void RealizationTalent()
    {
        base.RealizationTalent();
        _guildValutes.ExtendMaxPlaces(_extraPlaces);
    }
}
