using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GuildHeroSlot_UI : HeroSlot_UI
{
    [SerializeField] protected TextMeshProUGUI _heroCondition;
    [SerializeField] protected WoundSlot_UI _wound;
    [SerializeField] protected Image _levelUpIcon;

    private void OnEnable()
    {
        EquipItem.OnChangeStat += UpdateUISlot;
    }

    private void OnDisable()
    {
        EquipItem.OnChangeStat -= UpdateUISlot;
    }

    public override void UpdateUISlot()
    {
        base.UpdateUISlot();

        _wound.SetImage(_hero.WoundType);

        if (_hero != null)
        {
            if (_hero.IsSentOnQuest)
                _heroCondition.text = $"Condition: On Quest";

            else if (_hero.IsHealing)
                _heroCondition.text = $"Condition: Healing";

            else if (!_hero.IsRested)
                _heroCondition.text = $"Condition: Resting";

            else
                _heroCondition.text = $"Condition: Ready";

            if (_hero.LevelSystem.StatPoints > 0 || _hero.LevelSystem.AbilityPoints > 0)
                _levelUpIcon.gameObject.SetActive(true);

            else
                _levelUpIcon.gameObject.SetActive(false);
        }

        else
        {
            _heroCondition.text = "";
        }
    }
}
