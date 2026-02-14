using UnityEngine;


[System.Serializable]
public class Debuff
{
  [SerializeField]  private DebuffData _debuffData;

    public DebuffData DebuffData => _debuffData;

    public Debuff(Debuff debuffCopy)
    {
        _debuffData = debuffCopy.DebuffData;
    }
    public virtual void ApplyDebuff()
    {
        Debug.Log($"Applying debuff");
    }
}