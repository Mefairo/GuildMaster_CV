using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class QuestResistanceSystem : MonoBehaviour
{
    [SerializeField] private ImageKeeper _imageKeeper;
    [Header("Ability Resistance")]
    [SerializeField] private Transform _imageAbilityContainer;
    //[SerializeField] private Image _prefabImage;
    [SerializeField] private ResImage_UI _prefabImage;
    [Header("Enemy Resistance")]
    [SerializeField] private Transform _imageEnemyContainer;
    [Header("Debuffs")]
    [SerializeField] private Transform _imageDebuffsContainer;
    [Header("Quest Ability")]
    [SerializeField] private Transform _imageQuestAbilityContainer;

    private QuestKeeperDisplay _questKeeperDisplay;

    private void Awake()
    {
        _questKeeperDisplay = GetComponent<QuestKeeperDisplay>();
    }

    private void Start()
    {
        ClearAllIcons();
    }

    private void OnEnable()
    {
        if (_questKeeperDisplay != null)
            _questKeeperDisplay.OnClickOtherSlot += ClearIcons;
    }

    private void OnDisable()
    {
        if (_questKeeperDisplay != null)
            _questKeeperDisplay.OnClickOtherSlot -= ClearIcons;
    }

    public void ShowResistance(List<Resistance> resistances)
    {
        ClearResistanceIcons(_imageEnemyContainer);
        ClearResistanceIcons(_imageAbilityContainer);
        ClearResistanceIcons(_imageDebuffsContainer);

        foreach (Resistance res in resistances)
        {
            //if (res.ValueResistance == 0)
            //    continue;

            ResImage_UI imageRes = Instantiate(_prefabImage, _imageEnemyContainer.transform);

            if (res.ValueResistance != 0)
            {
                imageRes.SetTypeDamage(res.TypeDamage, true);
                imageRes.Percent.text = $"{res.ValueResistance.ToString()}%";
            }

            else
                imageRes.SetTypeDamage(res.TypeDamage);

            //_imageKeeper.SetIcon(res, imageRes);

            //imageRes.Percent.text = $"{res.ValueResistance.ToString()}%";
        }
    }

    public void ShowAbilityResistance(AbilitySlot_UI abilitySlot)
    {
        ShowDebuffs(abilitySlot.Ability);
        ClearResistanceIcons(_imageAbilityContainer);

        List<Resistance> attackResistances = abilitySlot.Ability.ResistancesForAttack;
        List<Resistance> defenceResistances = abilitySlot.Ability.ResistancesForDefence;

        if (attackResistances.Count != 0)
            SetIcons(attackResistances, false);

        if (defenceResistances.Count != 0)
            SetIcons(defenceResistances, true);

    }

    public void ShowDebuffs(Ability ability)
    {
        ClearResistanceIcons(_imageDebuffsContainer);

        List<Debuff> debuffs = new List<Debuff>();

        if (ability.DebuffsForAttack.Count != 0)
            debuffs = ability.DebuffsForAttack;

        else if (ability.DebuffsForDefence.Count != 0)
            debuffs = ability.DebuffsForDefence;

        foreach (Debuff debuff in debuffs)
        {
            ResImage_UI imageDebuff = Instantiate(_prefabImage, _imageDebuffsContainer.transform);

            imageDebuff.SetTypeDebuff(debuff);
            imageDebuff.ImageSelf.sprite = debuff.DebuffData.Icon;
        }
    }

    public void ShowResistance(List<Resistance> resList, Transform container)
    {
        ClearResistanceIcons(container);

        foreach (Resistance res in resList)
        {
            ResImage_UI imageRes = Instantiate(_prefabImage, container);

            if (res.ValueResistance != 0)
            {
                imageRes.SetTypeDamage(res.TypeDamage, true);
                imageRes.Percent.text = $"{res.ValueResistance.ToString()}%";
            }

            else
                imageRes.SetTypeDamage(res.TypeDamage);
        }
    }

    public void ShowDebuffs(List<Debuff> debuffList, Transform container)
    {
        ClearResistanceIcons(container);

        foreach (Debuff debuff in debuffList)
        {
            ResImage_UI imageDebuff = Instantiate(_prefabImage, container);

            imageDebuff.SetTypeDebuff(debuff);
            imageDebuff.ImageSelf.sprite = debuff.DebuffData.Icon;
        }
    }

    private void SetIcons(List<Resistance> resistances, bool percentEnable)
    {
        foreach (Resistance res in resistances)
        {
            ResImage_UI imageRes = Instantiate(_prefabImage, _imageAbilityContainer.transform);
            imageRes.SetTypeDamage(res.TypeDamage, percentEnable);
            imageRes.Percent.text = $"{res.ValueResistance.ToString()}%";
        }
    }

    private void ClearResistanceIcons(Transform container)
    {
        if (container != null)
        {
            foreach (Transform child in container)
            {
                Destroy(child.gameObject);
            }
        }
    }

    public void ClearIcons()
    {
        if (_imageEnemyContainer != null)
        {
            foreach (Transform child in _imageEnemyContainer)
            {
                Destroy(child.gameObject);
            }
        }

        if (_imageAbilityContainer != null)
        {
            foreach (Transform child in _imageAbilityContainer)
            {
                Destroy(child.gameObject);
            }
        }

        if (_imageDebuffsContainer != null)
        {
            foreach (Transform child in _imageDebuffsContainer)
            {
                Destroy(child.gameObject);
            }
        }
    }

    public void ClearAllIcons()
    {
        if (_imageEnemyContainer != null)
            ClearResistanceIcons(_imageEnemyContainer);

        if (_imageAbilityContainer != null)
            ClearResistanceIcons(_imageAbilityContainer);

        if (_imageDebuffsContainer != null)
            ClearResistanceIcons(_imageDebuffsContainer);
    }

    public void ClearIcon(Transform container)
    {
        ClearResistanceIcons(container);
    }
}
