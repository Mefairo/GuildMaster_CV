using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestEnemySlot_UI : MonoBehaviour
{
    [SerializeField] private Image _iconEnemy;
    [SerializeField] private Button _buttonSelf;
    [SerializeField] private Enemy _enemy;

    public Enemy Enemy => _enemy;

    public QuestKeeperDisplay QuestKeeperDisplay { get; private set; }
    public QuestResistanceSystem QuestResistanceSystem { get; private set; }
    public QuestKeeperController QuestKeeperController { get; private set; }

    private void Awake()
    {
        QuestKeeperDisplay = GetComponentInParent<QuestKeeperDisplay>();
        QuestResistanceSystem = GetComponentInParent<QuestResistanceSystem>();
        QuestKeeperController = GetComponentInParent<QuestKeeperController>();

        _buttonSelf?.onClick.AddListener(ClickedSlot);
    }

    public void Init(Enemy enemy)
    {
        _enemy = enemy;
        UpdateUISlot();
    }

    private void UpdateUISlot()
    {
        _iconEnemy.sprite = _enemy.EnemyData.Icon;
    }

    private void ClickedSlot()
    {
        if (QuestKeeperDisplay != null)
        {
            QuestKeeperDisplay.SelectEnemy(this);
            QuestKeeperDisplay.UpdateEnemyInfo(this);
        }

        if(QuestResistanceSystem != null)
        {
            List<Resistance> resistances = this._enemy.EnemyResistanceSystem.AllRes;
            QuestResistanceSystem.ShowResistance(resistances);
        }

        if (QuestKeeperController != null)
        {
            QuestKeeperController.UpdateEnemyInfo(this);
        }

    }
}
