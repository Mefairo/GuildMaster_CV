using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class HeroAbilitySystem
{
    [SerializeField] private int _amountAbility = 3;
    [SerializeField] private List<Ability> _abilityList;

    //public event UnityAction<HeroAbilitySlot> OnAbilitySlotChanged;
    public List<Ability> AbilityList => _abilityList;

    public HeroAbilitySystem(int size)
    {
        _abilityList = new List<Ability>();

        for (int i = 0; i < _amountAbility; i++)
        {
            _abilityList.Add(new Ability());
        }
    }

    public HeroAbilitySystem(HeroAbilitySystem copyAbilitySystem)
    {
        _abilityList = new List<Ability>(copyAbilitySystem._amountAbility);

        for (int i = 0; i < _amountAbility; i++)
        {
            _abilityList.Add(copyAbilitySystem.AbilityList[i]);
        }
    }

    public void AddAbility(Ability ability, int index)
    {
        Ability newAbility = SetAbility(ability);

        _abilityList[index] = newAbility;
    }

    public void RemoveAbility(int index)
    {
        _abilityList[index] = null;
        _abilityList[index] = new Ability();
    }

    private Ability SetAbility(Ability ability)
    {
        Ability newAbility;

        switch (ability.AbilityData)
        {
            // ÏÀËÀÄÈÍ

            case PaladinAbilityData_1_2_1:
                newAbility = new PaladinAbility_1_2_1(ability);
                break;

            case PaladinAbilityData_2_2_1:
                newAbility = new PaladinAbility_2_2_1(ability);
                break;

            case PaladinAbilityData_3_2_1:
                newAbility = new PaladinAbility_3_2_1(ability);
                break;

            case PaladinAbilityData_6_2_2:
                newAbility = new PaladinAbility_6_2_2(ability);
                break;

            // ÄÀÐÊ ÍÀÉÒ

            case DarkKnightAbilityData_1_2_2:
                newAbility = new DarkKnightAbility_1_2_2(ability);
                break;

            case DarkKnightAbilityData_2_2_2:
                newAbility = new DarkKnightAbility_2_2_2(ability);
                break;

            case DarkKnightAbilityData_4_2_1:
                newAbility = new DarkKnightAbility_4_2_1(ability);
                break;

            case DarkKnightAbilityData_5_2_2:
                newAbility = new DarkKnightAbility_5_2_2(ability);
                break;

            // ÄÀÐÊ ÍÀÉÒ

            case PrimalistAbilityData_1_2_1:
                newAbility = new PrimalistAbility_1_2_1(ability);
                break;

            case PrimalistAbilityData_3_2_2:
                newAbility = new PrimalistAbility_3_2_2(ability);
                break;


            // ÍÅÊÐÎÌÀÍÑÅÐ

            case NecromancerAbilityData_1_2_1:
                newAbility = new NecromancerAbility_1_2_1(ability);
                break;

            case NecromancerAbilityData_6:
                newAbility = new NecromancerAbility_6(ability);
                break;

            case NecromancerAbilityData_6_2_1:
                newAbility = new NecromancerAbility_6_2_1(ability);
                break;

            case NecromancerAbilityData_6_2_2:
                newAbility = new NecromancerAbility_6_2_2(ability);
                break;


            // ÎÁÙÈÅ

            case WeaponAbilityData:
                newAbility = new WeaponAbilityRealization(ability);
                break;

            case StatScaleAbilityData:
                newAbility = new StatScaleAbilityReaization(ability);
                break;

            case StatScaleByValueAbilityData:
                newAbility = new StatScaleByValueAbilityRealization(ability);
                break;

            case BuffOtherHeroesAbilityData:
                newAbility = new BuffOtherHeroesAbilityRealization(ability);
                break;

            //ÄÅÔÎËÒ

            default:
                newAbility = new Ability(ability);
                break;
        }

        return newAbility;
    }
}
