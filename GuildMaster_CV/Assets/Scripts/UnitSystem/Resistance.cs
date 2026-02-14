using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Resistance
{
    [SerializeField] public TypeDamage TypeDamage;
    [SerializeField] public int ValueResistance;

    public Resistance(Resistance resCopy) 
    {
        TypeDamage = resCopy.TypeDamage;
        ValueResistance = resCopy.ValueResistance;
    }

    public Resistance(TypeDamage typeDamage, int valueResistance)
    {
        TypeDamage = typeDamage;
        ValueResistance = valueResistance;
    }

    public Resistance() { }
}
