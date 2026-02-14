using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy System/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public int ID = -1;
    public string Name;
    [TextArea(4, 4)]
    public string Description;
    public Sprite Icon;
    public List<Resistance> Resistances;
    public List<Debuff> DebuffImmun;
    public List<Ability> Abilities;
    public EnemyType EnemyType;
    public bool RequireAoeDamage = false;
}

public enum EnemyType
{
    None,
    Undead,
    Humanoid,
    Demon,
    Elemental,
    Spirit,
    Animal,
}
