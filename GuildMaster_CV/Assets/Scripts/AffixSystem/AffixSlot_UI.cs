using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AffixSlot_UI : MonoBehaviour
{
    [SerializeField] private Button _buttonSelf;
    [SerializeField] private Image _icon;
    [SerializeField] private Affix _affix;

    public Affix Affix => _affix;

    public QuestKeeperController QuestKeeperController { get; private set; }
    public QuestResistanceSystem QuestResistanceSystem { get; private set; }

    private void Awake()
    {
        QuestKeeperController = GetComponentInParent<QuestKeeperController>();
        QuestResistanceSystem = GetComponentInParent<QuestResistanceSystem>();

        _buttonSelf?.onClick.AddListener(ClickedSlot);
    }

    public void Init(Affix affix)
    {
        ClearInfo();

        _affix = affix;

        UpdateUI(affix);
    }

    private void UpdateUI(Affix affix)
    {
        _icon.sprite = affix.AffixData.Icon;
    }

    private void ClearInfo()
    {
        _icon.sprite = null;
        _affix = null;
    }
    private void ClickedSlot()
    {
        if (QuestKeeperController != null)
        {
            QuestKeeperController.UpdateUIEntityInfo(this);
        }

        if(QuestResistanceSystem != null)
        {
            QuestResistanceSystem.ClearIcons();
        }
    }
}
