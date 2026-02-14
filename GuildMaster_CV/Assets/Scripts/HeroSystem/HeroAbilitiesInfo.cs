using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroAbilitiesInfo : MonoBehaviour
{
    [SerializeField] private ImageKeeper _imageKeeper;
    [Header("Ability Resistance")]
    [SerializeField] private Transform _imageAbilityContainer;
    [SerializeField] private ResImage_UI _prefabImage;
    [Header("Debuffs")]
    [SerializeField] private Transform _imageDebuffsContainer;

    private KeeperDisplay _keeperDisplay;

    private void Awake()
    {
        _keeperDisplay = GetComponent<KeeperDisplay>();
    }

    public void ShowAbilityResistance(AbilitySlot_UI ability)
    {
        ShowDebuffs(ability);
        ClearResistanceIcons(_imageAbilityContainer);

        List<Resistance> attackResistances = ability.Ability.ResistancesForAttack;
        List<Resistance> defenceResistances = ability.Ability.ResistancesForDefence;

        if(attackResistances.Count != 0)
            CreateImageRes(attackResistances, false);

        if(defenceResistances.Count != 0)
            CreateImageRes(defenceResistances, true);
    }

    private void CreateImageRes(List<Resistance> resistance, bool percentEnable)
    {
        foreach (Resistance res in resistance)
        {
            ResImage_UI imageRes = Instantiate(_prefabImage, _imageAbilityContainer.transform);

            imageRes.SetTypeDamage(res.TypeDamage, percentEnable);
            imageRes.Percent.text = $"{res.ValueResistance.ToString()}%";
        }
    }

    private void ShowDebuffs(AbilitySlot_UI ability)
    {
        ClearResistanceIcons(_imageDebuffsContainer);

        List<Debuff> attackDebuffs = ability.Ability.DebuffsForAttack;
        List<Debuff> defenceDebuffs = ability.Ability.DebuffsForDefence;

        if (attackDebuffs.Count != 0)
            CreateImageDebuff(attackDebuffs);

        if (defenceDebuffs.Count != 0)
            CreateImageDebuff(defenceDebuffs);
    }

    private void CreateImageDebuff(List<Debuff> debuffs)
    {
        foreach (Debuff debuff in debuffs)
        {
            ResImage_UI imageDebuff = Instantiate(_prefabImage, _imageDebuffsContainer.transform);

            imageDebuff.SetTypeDebuff(debuff);
            imageDebuff.ImageSelf.sprite = debuff.DebuffData.Icon;
        }
    }

    public void ClearAllIcons()
    {
        foreach (Transform child in _imageAbilityContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in _imageDebuffsContainer)
        {
            Destroy(child.gameObject);
        }
    }

    private void ClearResistanceIcons(Transform container)
    {
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }
    }
}
