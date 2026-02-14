using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AffixApplyingEffect
{
    public virtual int CheckAffixes(List<Affix> affixes, List<Hero> heroes)
    {
        int affixesComplete = 0;
        List<Affix> unConsiderAffixes = new List<Affix>();

        foreach (Affix affix in affixes)
        {
            if (affix.AffixData.ConsiderCountAffix == false)
                unConsiderAffixes.Add(affix);

            foreach (Hero hero in heroes)
            {
                if (affix.AffixData.RequiredForAllHeroesNeutralize)
                {
                    if(hero.BuffSystem.AllNeutralizeAffix.Contains(affix.AffixData.ActiveDebuffAffix))
                    affixesComplete++;
                }

                else
                {
                    if (hero.BuffSystem.AllNeutralizeAffix.Contains(affix.AffixData.ActiveDebuffAffix))
                    {
                        affixesComplete += heroes.Count;
                        break;
                    }
                }
            }
        }

        int applyingAffixes = (affixes.Count - unConsiderAffixes.Count) * heroes.Count - affixesComplete;

        return applyingAffixes;
    }
}
