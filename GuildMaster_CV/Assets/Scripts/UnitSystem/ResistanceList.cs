using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResistanceList 
{
    public List<Resistance> Resistances;

    public ResistanceList(ResistanceList resCopy)
    {
        Resistances = resCopy.Resistances;
    }

    public ResistanceList() { }

    public void FireChange(int value)
    {

    }

    public void ColdChange(int value)
    {

    }

    public void LightningChange(int value)
    {

    }

    public void NecroticChange(int value)
    {

    }

    public void VoidChange(int value)
    {

    }

    public void PoisonChange(int value)
    {

    }

    public void AcidChange(int value)
    {

    }

    public void ExplosiveChange(int value)
    {

    }
}
