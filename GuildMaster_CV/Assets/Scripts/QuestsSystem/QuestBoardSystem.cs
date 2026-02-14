using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestBoardSystem
{
    [SerializeField] private List<QuestSlot> _questSlots;
    [SerializeField] private List<Expedition> _expeditionSlots;

    public List<QuestSlot> QuestSlots => _questSlots;
    public List<Expedition> ExpeditionSlots => _expeditionSlots;

    public QuestBoardSystem()
    {
        _questSlots = new List<QuestSlot>();
        _expeditionSlots = new List<Expedition>();
    }

    public void AddNewQuest(Quest quest)
    {
        QuestSlot newSlot = new QuestSlot(quest);
        _questSlots.Add(newSlot);
        newSlot.AssignQuest(newSlot);
    }

    public void AddNewExpedition(Expedition expedition)
    {
        Debug.Log("add exp");
        QuestSlot newSlot = new QuestSlot(expedition);
        _expeditionSlots.Add(expedition);
        newSlot.AssignQuest(expedition);
    }

    public Expedition AddAgainExpedition(Expedition expedition)
    {
        Expedition copyExpedition = new Expedition(expedition);

        copyExpedition.IsQuestToChecked = false;
        copyExpedition.IsQuestToSuccess = false;

        _expeditionSlots.Add(copyExpedition);

        return copyExpedition;
    }

    public void RemoveQuest(QuestSlot questSlot)
    {
        _questSlots.Remove(questSlot);
    }

    public void RemoveExpedition(Expedition expedition)
    {
        Debug.Log("remove");
        _expeditionSlots.Remove(expedition);
    }
}
