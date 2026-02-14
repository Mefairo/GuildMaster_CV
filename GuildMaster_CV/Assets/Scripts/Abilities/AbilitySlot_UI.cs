using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AbilitySlot_UI : MonoBehaviour
{
    [SerializeField] private Image _iconAbility;
    [SerializeField] private TextMeshProUGUI _tier;
    [SerializeField] private Button _buttonSelf;
    [SerializeField] private Ability _ability;

    public Ability Ability => _ability;

    public QuestKeeperDisplay QuestKeeperDisplay { get; private set; }
    public KeeperDisplay KeeperDisplay { get; private set; }
    public QuestResistanceSystem QuestResistanceSystem { get; private set; }
    public HeroAbilitiesInfo HeroAbilitiesInfo { get; private set; }
    public QuestKeeperController QuestKeeperController { get; private set; }
    public RecruitQuestKeeperController RecruitQuestKeeperController { get; private set; }
    public PrepareQuestUIController PrepareQuestUIController { get; private set; }

    private void Awake()
    {
        QuestKeeperDisplay = GetComponentInParent<QuestKeeperDisplay>();
        KeeperDisplay = GetComponentInParent<KeeperDisplay>();
        QuestResistanceSystem = GetComponentInParent<QuestResistanceSystem>();
        HeroAbilitiesInfo = GetComponentInParent<HeroAbilitiesInfo>();
        QuestKeeperController = GetComponentInParent<QuestKeeperController>();
        RecruitQuestKeeperController = GetComponentInParent<RecruitQuestKeeperController>();
        PrepareQuestUIController = GetComponentInParent<PrepareQuestUIController>();

        _buttonSelf?.onClick.AddListener(ClickedSlot);
    }

    public void Init(Ability ability)
    {
        _ability = ability;
        UpdateUISlot();
    }

    private void UpdateUISlot()
    {
        if(_ability != null && _ability.AbilityData != null)
        {
            _iconAbility.sprite = _ability.AbilityData.Icon;
            _tier.text = SetTierText();
        }
    }

    private string SetTierText()
    {
        string tierText = "";

        switch (_ability.AbilityData.Tier)
        {
            case 0:
                tierText = "";
                break;

            case 1:
                tierText = "I";
                break;

            case 2:
                tierText = "II";
                break;

            case 3:
                tierText = "III";
                break;

            case 4:
                tierText = "IV";
                break;
        }

        return tierText;
    }

    private void ClickedSlot()
    {
        if(QuestKeeperDisplay != null)
        {
            QuestKeeperDisplay.SelectAbility(this);
            QuestKeeperDisplay.UpdateAbilityInfo(this);
        }

        else if(KeeperDisplay != null)
        {
            KeeperDisplay.SelectAbility(this);
            KeeperDisplay.UpdateAbilityInfo(this);
        }

        if(QuestResistanceSystem != null)
        {
            QuestResistanceSystem.ShowAbilityResistance(this);
        }

        if (HeroAbilitiesInfo != null)
        {
            HeroAbilitiesInfo.ShowAbilityResistance(this);
        }

        if (QuestKeeperController != null)
        {
            QuestKeeperController.UpdateUIEntityInfo(this);
        }

        if (RecruitQuestKeeperController != null)
        {
            RecruitQuestKeeperController.UpdateAbilityInfo(this);
        }

        if(PrepareQuestUIController != null)
        {
            PrepareQuestUIController.UpdateAbilityInfo(this);
        }
    }

   
}
