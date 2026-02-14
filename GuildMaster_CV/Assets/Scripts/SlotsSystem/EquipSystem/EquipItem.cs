using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EquipItem
{
    public static UnityAction OnChangeStat;
    public static UnityAction<Hero> OnApplyDebuffNeutralize;
    public static UnityAction<Hero> OnCancelDebuffNeutralize;

    public void Subscribe()
    {
        EquipDisplay.OnHeroEquip += Equip;
        EquipDisplay.OnHeroPotionsEquip += EquipSeveralPotions;
        EquipDisplay.OnHeroTakeOfEquip += UnEquip;
        EquipDisplay.OnHeroTakeOfEquip1 += UnEquip;
        //CalculationResultsQuest.OnRemoveTrinketsEffect += UnEquip;
    }

    public void Unsubscribe()
    {
        EquipDisplay.OnHeroEquip -= Equip;
        EquipDisplay.OnHeroPotionsEquip -= EquipSeveralPotions;
        EquipDisplay.OnHeroTakeOfEquip -= UnEquip;
        EquipDisplay.OnHeroTakeOfEquip1 -= UnEquip;
        //CalculationResultsQuest.OnRemoveTrinketsEffect -= UnEquip;
    }

    private void Equip(Hero hero, EquipSlot_UI equipSlot_UI)
    {
        CraftItemData itemData = (CraftItemData)equipSlot_UI.AssignedInventorySlot.ItemData;

        UseEquip(hero, true, equipSlot_UI.AssignedEquipSlot);

        //if (itemData.EquipType == EquipType.Weapon_Mod)
        //    Mod(hero, itemData);
    }

    private void EquipSeveralPotions(Hero hero, EquipSlot_UI equipSlot_UI)
    {
        CraftItemData itemData = (CraftItemData)equipSlot_UI.AssignedInventorySlot.ItemData;

        if (equipSlot_UI.AssignedEquipSlot.EquipItemData.ItemSecondaryType == ItemSecondaryType.Potion)
        {
            for (int i = 0; i < equipSlot_UI.AssignedEquipSlot.StackSize; i++)
                UseEquip(hero, true, equipSlot_UI.AssignedEquipSlot);
        }

        else
            UseEquip(hero, true, equipSlot_UI.AssignedEquipSlot);
    }

    private void Equip(Hero hero, EquipSlot equipSlot)
    {
        CraftItemData itemData = (CraftItemData)equipSlot.ItemData;

        UseEquip(hero, true, equipSlot);

        //if (itemData.EquipType == EquipType.Weapon_Mod)
        //    Mod(hero, itemData);
    }

    private void UnEquip(Hero hero, EquipSlot_UI equipSlot_UI)
    {
        CraftItemData itemData = (CraftItemData)equipSlot_UI.AssignedInventorySlot.ItemData;

        if (equipSlot_UI.AssignedEquipSlot is BlankSlot blankSlot)
            UseEquip(hero, false, equipSlot_UI.AssignedEquipSlot);

        else
        {
            if (equipSlot_UI.AssignedEquipSlot.EquipItemData.ItemSecondaryType == ItemSecondaryType.Potion)
            {
                for (int i = 0; i < equipSlot_UI.AssignedEquipSlot.StackSize; i++)
                    UseEquip(hero, false, equipSlot_UI.AssignedEquipSlot);
            }

            else
                UseEquip(hero, false, equipSlot_UI.AssignedEquipSlot);
        }



        //if (itemData.EquipType == EquipType.Weapon_Mod)
        //    UnMod(hero, itemData);
    }

    public void UnEquip(Hero hero, EquipSlot equipSlot)
    {
        CraftItemData itemData = (CraftItemData)equipSlot.ItemData;

        if (equipSlot is BlankSlot blankSlot)
            UseEquip(hero, false, equipSlot);

        else
        {
            if (equipSlot.EquipItemData.ItemSecondaryType == ItemSecondaryType.Potion)
            {
                for (int i = 0; i < equipSlot.StackSize; i++)
                    UseEquip(hero, false, equipSlot);
            }

            else
                UseEquip(hero, false, equipSlot);
        }


        //if (itemData.EquipType == EquipType.Weapon_Mod)
        //    UnMod(hero, itemData);
    }


    private void UseEquip(Hero hero, bool equip, EquipSlot equipSlot)
    {
        // ÑÈËÀ ÃÅÐÎß
        if (equipSlot == null && equipSlot.EquipItemData == null)
            Debug.Log("eqa nul");

        OnChangeStat?.Invoke();
        //if (equip == true)
        //{
        //    Debug.Log("eq item");
        //    if (equipSlot.ItemDefDebuffs.Count != 0)
        //    {
        //        foreach (DebuffList neutralizeDebuff in equipSlot.ItemDefDebuffs)
        //        {
        //            hero.NeutralizeDebuffs.Add(neutralizeDebuff);
        //            Debug.Log("eq deb");
        //        }

        //        foreach (AffixList neutralizeAffix in equipSlot.ItemDefAffix)
        //        {
        //            hero.NeutralizeAffixes.Add(neutralizeAffix);
        //        }
        //    }
        //}

        //else
        //{
        //    Debug.Log("uneq item");
        //    if (equipSlot.ItemDefDebuffs.Count != 0)
        //    {
        //        foreach (DebuffList neutralizeDebuff in equipSlot.ItemDefDebuffs)
        //        {
        //            hero.NeutralizeDebuffs.Remove(neutralizeDebuff);
        //            Debug.Log("remove deb");
        //        }

        //        foreach (AffixList neutralizeAffix in equipSlot.ItemDefAffix)
        //        {
        //            hero.NeutralizeAffixes.Remove(neutralizeAffix);
        //        }
        //    }
        //}


    }
}

