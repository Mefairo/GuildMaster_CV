using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestKeeper : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] protected DynamicQuestList _questList;
    [SerializeField] protected QuestBoardSystem _boardSystem;
    [SerializeField] protected NextDayController _nextDayController;
    [SerializeField] protected WorldEventSystem _worldEventSystem;
    [Header("Flags")]
    [SerializeField] protected bool _isSendQuest = false;
    [SerializeField] protected bool _isTakingQuests = false;
    [SerializeField] protected bool _isViewedQuests = false;
    [Header("UI")]
    //[SerializeField] protected Button _activeButton;
    [SerializeField] private SpriteRenderer _glow;
    [SerializeField] private SpriteRenderer _namePanel;

    public QuestBoardSystem BoardSystem => _boardSystem;

    public static UnityAction OnQuestWindowRequested;
    public static UnityAction<QuestBoardSystem> OnSendQuestWindowRequested;
    public static UnityAction<QuestBoardSystem> OnTakingQuestWindowRequested;

    private void Start()
    {
        _boardSystem = new QuestBoardSystem();
        SetNewDailyQuests();

        if (_isTakingQuests)
        {

        }

        else
        {
            //foreach (Quest quest in _questList.QuestsList)
            //{
            //    _boardSystem.AddNewQuest(quest);
            //}

            //foreach (Expedition expedition in _questList.ExpeditionList)
            //{
            //    Expedition copyExpedition = new Expedition(expedition);
            //    _boardSystem.AddNewExpedition(copyExpedition);
            //}
        }

        _glow.gameObject.SetActive(false);
        _namePanel.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _nextDayController.OnNextDay += NextDayExpedition;
    }

    private void OnDisable()
    {
        _nextDayController.OnNextDay -= NextDayExpedition;
    }

    public void SetNewDailyQuests() // подписан на NextDayController через UnityEvent
    {
        if (!_isTakingQuests)
        {
            //_boardSystem = new QuestBoardSystem(false);
            _boardSystem.QuestSlots.Clear();
            _questList.SetAmountQuests();

            foreach (Quest quest in _questList.QuestsList)
            {
                _boardSystem.AddNewQuest(quest);
            }

            SetEventEnemy();
        }
    }

    public void AddNewExpedition(Expedition newExpedition)
    {
        //_boardSystem.AddNewExpedition(newExpedition);

        //foreach (Expedition quest in _questList.ExpeditionList)
        //{
        //    _boardSystem.AddNewExpedition(quest);
        //}

        //SetEventEnemy();
    }

    public void SetEventEnemy()
    {
        foreach (QuestSlot questSlot in _boardSystem.QuestSlots)
        {
            foreach (EnemyData enemyData in _worldEventSystem.EventEnemyList)
            {
                Enemy newEventEnemy = new Enemy(enemyData);
                questSlot.Quest.EnemiesList.Add(newEventEnemy);
            }
        }
    }

    public void OpenQuestWindow()
    {
        if (!_nextDayController.InTransition)
        {
            if (_isSendQuest)
                OnSendQuestWindowRequested?.Invoke(_boardSystem);

            //else
            //    OnQuestWindowRequested?.Invoke(_boardSystem);

            if (_isTakingQuests)
                OnTakingQuestWindowRequested?.Invoke(_boardSystem);

            OnQuestWindowRequested?.Invoke();
        }
    }

    private void NextDayExpedition()
    {
        if (!_isTakingQuests)
        {
            for (int i = _boardSystem.ExpeditionSlots.Count - 1; i >= 0; i--)
            {
                _boardSystem.ExpeditionSlots[i].NextDayExpedition();

                if (_boardSystem.ExpeditionSlots[i].LeftDays == 0)
                {
                    _boardSystem.RemoveExpedition(_boardSystem.ExpeditionSlots[i]);
                }
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _glow.gameObject.SetActive(true);
        _namePanel.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _glow.gameObject.SetActive(false);
        _namePanel.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OpenQuestWindow();
    }
}
