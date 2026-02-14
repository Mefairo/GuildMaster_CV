using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestSlot 
{
    [SerializeField] private Quest _quest;

    public Quest Quest => _quest;

    public QuestSlot(Quest data)
    {
        _quest = data;
    }

    public QuestSlot(Expedition expedition)
    {
        _quest = expedition;
    }

    public QuestSlot()
    {
        ClearQuest();
    }

    private void ClearQuest()
    {
        _quest = null;
    }

    public void AssignQuest(QuestSlot quest)
    {
        _quest = quest.Quest;
    }

    public void AssignQuest(Expedition expedition)
    {
        _quest = expedition;
    }
}
