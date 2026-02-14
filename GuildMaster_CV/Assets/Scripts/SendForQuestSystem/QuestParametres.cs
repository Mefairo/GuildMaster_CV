using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class QuestParametres : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _level;
    [SerializeField] private TextMeshProUGUI _power;
    [SerializeField] private TextMeshProUGUI _defence;
    [SerializeField] private TextMeshProUGUI _amountDays;
    [SerializeField] private TextMeshProUGUI _expeditionDaysLeft;
    [Header("Resistance")]
    [SerializeField] private ResImage_UI _resImagePrefab;
    [SerializeField] private Transform _resImageContainer;
    [Header("Consumable")]
    [SerializeField] private TextMeshProUGUI _foodAmount;
    [SerializeField] private TextMeshProUGUI _lightAmount;
    [SerializeField] private TextMeshProUGUI _healAmount;
    [SerializeField] private TextMeshProUGUI _manaCostAmount;
    [Header("Chances")]
    [SerializeField] private TextMeshProUGUI _chanceComplete;
    [SerializeField] private TextMeshProUGUI _chanceSupplies;
    [SerializeField] private TextMeshProUGUI _chanceCombat;

    public MainQuestKeeperDisplay MainQuestKeeperDisplay { get; private set; }
    public QuestResistanceSystem QuestResistanceSystem { get; private set; }

    private void Awake()
    {
        MainQuestKeeperDisplay = GetComponentInParent<MainQuestKeeperDisplay>();
        QuestResistanceSystem = GetComponentInParent<QuestResistanceSystem>();

        ClearInfo();
    }

    private void OnEnable()
    {
        //MainQuestKeeperDisplay.OnQuestClick += UpdateQuestInfo;
        //MainQuestKeeperDisplay.OnTakeQuest += ClearInfo;
    }

    private void OnDisable()
    {
        //MainQuestKeeperDisplay.OnQuestClick -= UpdateQuestInfo;
        //MainQuestKeeperDisplay.OnTakeQuest -= ClearInfo;
    }

    public void ClearInfo()
    {
        _chanceComplete.text = "Chance To Complete: 0%";
        _chanceSupplies.text = "Supplies: 0%";
        _chanceCombat.text = "Combat: 0%";

        _level.text = "Quest Level:";
        _power.text = "Power:";
        _defence.text = "Defence:";
        _amountDays.text = "Amount Days:";

        _foodAmount.text = "Food: ";
        _lightAmount.text = "Light:";
        _healAmount.text = "Heal:";
        _manaCostAmount.text = "Mana:";

        ClearIcons();
    }

    //private void UpdateQuestInfo()
    //{
    //    QuestSlot_UI questSlot = MainQuestKeeperDisplay.SelectedQuest;

    //    OnCheckRequiresComplete?.Invoke();

    //    _level.text = $"Quest Level: {questSlot.Quest.Quest.Level}";
    //    _amountDays.text = $"Amount Days: {questSlot.Quest.Quest.AmountDaysToComplete}";

    //    if (questSlot.Quest.Quest is Expedition expedition)
    //        _expeditionDaysLeft.text = $"Days left: {expedition.LeftDays}";

    //    UpdatePowerInfoUI();
    //    UpdateDefenceInfoUI();
    //}

    //private void UpdatePowerInfoUI()
    //{
    //    QuestSlot_UI questSlot = MainQuestKeeperDisplay.SelectedQuest;

    //    if (questSlot != null)
    //        _power.text = $"Power: {QuestParametresSystem.PowerSum}/{questSlot.Quest.Quest.Power}";
    //}

    //private void UpdateDefenceInfoUI()
    //{
    //    QuestSlot_UI questSlot = MainQuestKeeperDisplay.SelectedQuest;

    //    if (questSlot != null)
    //        _defence.text = $"Defence: {QuestParametresSystem.DefenceSum}/{questSlot.Quest.Quest.Defence}";
    //}

    public void ClearIcons()
    {
        if (_resImageContainer != null)
        {
            foreach (Transform child in _resImageContainer)
            {
                Destroy(child.gameObject);
            }
        }
    }

    public void UpdateFoodInfoUI(float requiredFood, float activeFood)
    {
        _foodAmount.text = $"Food: {activeFood} / {requiredFood}";
    }

    public void UpdateLightInfoUI(float requiredLight, float activeLight)
    {
        _lightAmount.text = $"Light: {activeLight} / {requiredLight}";
    }

    public void UpdateHealInfoUI(float requiredHeal, float activeHeal)
    {
        _healAmount.text = $"Heal: {activeHeal} / {requiredHeal}";
    }

    public void UpdateManaInfoUI(float requiredHeal, float activeHeal)
    {
        _manaCostAmount.text = $"Mana: {activeHeal} / {requiredHeal}";
    }

    public void UpdateSupplyInfoUI(float chance)
    {
        _chanceSupplies.text = $"Supplies: {chance}%";
    }

    public void UpdateCombatInfoUI(float chance)
    {
        _chanceCombat.text = $"Combat: {chance}%";
    }

    public void UpdateCompleteInfoUI(float chance)
    {
        _chanceComplete.text = $"Chance To Complete: {chance}%";
    }

    public void UpdateResistanceInfoUI(List<Resistance> activeResistanceQuest)
    {
        ClearIcons();

        foreach (Resistance res in activeResistanceQuest)
        {
            ResImage_UI imageRes = Instantiate(_resImagePrefab, _resImageContainer.transform);

            imageRes.SetTypeDamage(res.TypeDamage, true);

            imageRes.Percent.text = $"{res.ValueResistance.ToString()}%";
        }
    }

    public void UpdateQuestInfoUI(float power, float defence)
    {
        _level.text = $"Quest Level: {MainQuestKeeperDisplay.SelectedQuest.Quest.Quest.Level}";
        _amountDays.text = $"Amount Days: {MainQuestKeeperDisplay.SelectedQuest.Quest.Quest.AmountDaysToComplete}";

        _power.text = $"Power: {power}/{MainQuestKeeperDisplay.SelectedQuest.Quest.Quest.Power}";
        _defence.text = $"Defence: {defence}/{MainQuestKeeperDisplay.SelectedQuest.Quest.Quest.Defence}";
    }
}
