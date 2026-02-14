using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class DebuffApplyingEffect
{
    public virtual int CheckDebuffs(List<Debuff> debuffs, List<Hero> heroes)
    {
        List<bool> bools = new List<bool>();

        foreach (Debuff debuff in debuffs)
        {
            foreach (Hero hero in heroes)
            {
                foreach (DebuffList neutralizeDebuff in hero.NeutralizeDebuffs)
                {
                    if (neutralizeDebuff == debuff.DebuffData.DebuffList)
                    {
                        bools.Add(true);
                        break;
                    }
                }
            }
        }

        int applyingDebuffs = debuffs.Count * heroes.Count - bools.Count;
        return applyingDebuffs;
    }

    public int CheckBuffs(List<BuffList> buffs, List<Hero> heroes)
    {
        List<BuffList> localBuffList = new List<BuffList>(buffs); // Создаем копию списка

        localBuffList.RemoveAll(enemyBuff =>
            heroes.Any(hero =>
                hero.AbilityHolder.HeroAbilitySystem.AbilityList.Any(ability =>
                    ability.NeutralizeBuffs.Contains(enemyBuff)
                )
            )
        );

        int applyingEnemyBuffs = localBuffList.Count;
        return applyingEnemyBuffs;
    }

    protected void ApplyDebuff()
    {

    }
    protected void NonEffectDebuff()
    {

    }
}
