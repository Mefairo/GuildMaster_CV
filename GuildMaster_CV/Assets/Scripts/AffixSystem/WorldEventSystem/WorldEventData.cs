using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "World Event System/World Event Data")]
public class WorldEventData : AffixData
{
    [Header("World Event Parametres")]
    public WorldEventsList WorldEventType;
    public int AmountDays;
    public int LeftDays;
    public float Value_1;
    public bool IsAffix = false;
}
