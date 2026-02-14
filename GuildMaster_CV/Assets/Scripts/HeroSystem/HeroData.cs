using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Hero Data/New Hero")]
public class HeroData : ScriptableObject
{
    public int ID = -1;
    public Sprite Icon;
    public string ClassName;
    public string Description;
    [Space]
    [Header("Properties")]
    public ClassHero Class;
    public int PowerPoints;
    public int DefencePoints;
    [Space]
    [Header("Resistance")]
    public List<Resistance> Resistance;
    [Space]
    [Header("Stats")]
    public HeroStats HeroStats;
    public HeroStats VisibleHeroStats;
    [Space]
    [Header("Abilities")]
    public List<Ability> Abilities;
}
