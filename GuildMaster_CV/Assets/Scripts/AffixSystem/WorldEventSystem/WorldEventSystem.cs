using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WorldEventSystem : MonoBehaviour
{
    [SerializeField] private NextDayController _nextDayController;
    [SerializeField] private QuestKeeper _questKeeper;
    [Header("Parametres")]
    [SerializeField] private int _amountAvailableActiveEvents;
    [SerializeField] private int _amountActiveEvents;
    [SerializeField] private int _chanceNewEvent;
    [Header("UI")]
    [SerializeField] private Image _panel;
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _eventText;
    [SerializeField] private WorldEventSlot_UI _slotPrefab;
    [SerializeField] private Transform _contentTransform;
    [Header("Event Lists")]
    [SerializeField] private List<WorldEventData> _primaryDataEventsList;
    [SerializeField] private List<WorldEventData> _activeyDataEventsList;
    [SerializeField] private List<WorldEvent> _primaryEventsList = new List<WorldEvent>();
    [SerializeField] private List<WorldEventSlot_UI> _activeEventSlotsList = new List<WorldEventSlot_UI>();
    [Header("Event Content")]
    [SerializeField] private List<EnemyData> _eventEnemyList = new List<EnemyData>();
    [SerializeField] private List<TypeDamage> _eventResList = new List<TypeDamage>();

    public WorldEventApplySystem WorldEventApplySystem { get; private set; }

    public List<EnemyData> EventEnemyList => _eventEnemyList;
    public List<WorldEventData> ActiveyDataEventsList => _activeyDataEventsList;
    public List<TypeDamage> EventResList => _eventResList;

    private void Awake()
    {
        WorldEventApplySystem = GetComponent<WorldEventApplySystem>();

        _panel.gameObject.SetActive(false);

        foreach (WorldEventData eventData in _primaryDataEventsList)
        {
            WorldEvent newEvent = CreateEventByType(eventData);
            _primaryEventsList.Add(newEvent);
        }

        ClearWorldEventSlots();
    }

    private WorldEvent CreateEventByType(WorldEventData data)
    {
        switch (data.WorldEventType)
        {
            case WorldEventsList.War:
                return new EventWar(data, WorldEventApplySystem);

            case WorldEventsList.Alchem_Crisis:
                return new EventCrisis(data, WorldEventApplySystem, WorldEventApplySystem.AlchemistShop);

            case WorldEventsList.Food_Crisis:
                return new EventCrisis(data, WorldEventApplySystem, WorldEventApplySystem.CookShop);

            case WorldEventsList.Jeweler_Crisis:
                return new EventCrisis(data, WorldEventApplySystem, WorldEventApplySystem.JewelerShop);

            case WorldEventsList.WorkShop_Crisis:
                return new EventCrisis(data, WorldEventApplySystem, WorldEventApplySystem.WorkShop);

            case WorldEventsList.BlackSmith_Crisis:
                return new EventCrisis(data, WorldEventApplySystem, WorldEventApplySystem.BlacksmithShop);

            case WorldEventsList.Tanner_Crisis:
                return new EventCrisis(data, WorldEventApplySystem, WorldEventApplySystem.TannerShop);

            case WorldEventsList.Tailor_Crisis:
                return new EventCrisis(data, WorldEventApplySystem, WorldEventApplySystem.TailorShop);

            case WorldEventsList.MagicShop_Crisis:
                return new EventCrisis(data, WorldEventApplySystem, WorldEventApplySystem.MagicShop);

            case WorldEventsList.Alchem_Wealth:
                return new EventWealth(data, WorldEventApplySystem, WorldEventApplySystem.AlchemistShop);

            case WorldEventsList.Food_Wealth:
                return new EventWealth(data, WorldEventApplySystem, WorldEventApplySystem.CookShop);

            case WorldEventsList.Jeweler_Wealth:
                return new EventWealth(data, WorldEventApplySystem, WorldEventApplySystem.JewelerShop);

            case WorldEventsList.WorkShop_Wealth:
                return new EventWealth(data, WorldEventApplySystem, WorldEventApplySystem.WorkShop);

            case WorldEventsList.BlackSmith_Wealth:
                return new EventWealth(data, WorldEventApplySystem, WorldEventApplySystem.BlacksmithShop);

            case WorldEventsList.Tanner_Wealth:
                return new EventWealth(data, WorldEventApplySystem, WorldEventApplySystem.TannerShop);

            case WorldEventsList.Tailor_Wealth:
                return new EventWealth(data, WorldEventApplySystem, WorldEventApplySystem.TailorShop);

            case WorldEventsList.MagicShop_Wealth:
                return new EventWealth(data, WorldEventApplySystem, WorldEventApplySystem.MagicShop);

            case WorldEventsList.Dead_Invasion:
                return new EventInvasion(data, WorldEventApplySystem);

            case WorldEventsList.New_Invasion:
                return new EventInvasion(data, WorldEventApplySystem);

            case WorldEventsList.TestResEvent1:
                return new EventQuestResist(data, WorldEventApplySystem);

            case WorldEventsList.TestResEvent2:
                return new EventQuestResist(data, WorldEventApplySystem);

            default: return null;
        }
    }

    public void ChangeEvents()
    {
        if (_activeEventSlotsList.Count != 0)
        {
            for (int i = _activeEventSlotsList.Count - 1; i >= 0; i--)
            {
                WorldEventSlot_UI activeEvent = _activeEventSlotsList[i];
                activeEvent.WorldEvent.NextDay();
                activeEvent.NextDay();

                if (activeEvent.WorldEvent.LeftDays == 0)
                {
                    if (activeEvent.WorldEvent.IsAffix)
                    {
                        foreach (QuestSlot questSlot in _questKeeper.BoardSystem.QuestSlots)
                        {
                            for (int a = questSlot.Quest.AffixesList.Count - 1; a >= 0; a--)
                            {
                                if (questSlot.Quest.AffixesList[a].AffixData is WorldEventData affixEventData &&
                                    activeEvent.WorldEvent.EventData == affixEventData)
                                {
                                    questSlot.Quest.AffixesList.RemoveAt(a);
                                }
                            }
                        }
                    }

                    // —охран€ем ссылки перед удалением
                    GameObject slotGameObject = activeEvent.gameObject;
                    WorldEventData eventData = activeEvent.WorldEvent.EventData;

                    // ”дал€ем из списков
                    _activeEventSlotsList.RemoveAt(i);
                    _activeyDataEventsList.RemoveAt(i);

                    // ”ничтожаем GameObject и отмен€ем событие
                    Destroy(slotGameObject);
                    activeEvent.WorldEvent.CancelEvent(eventData);
                }
            }
        }

        if (_amountAvailableActiveEvents > _activeEventSlotsList.Count)
        {
            CreateNewEvent();
        }
    }

    //public void ChangeEvents1() // подписан на NextDayController через UnityEvent
    //{
    //    if (_activeEventsList.Count != 0)
    //    {
    //        for (int i = _activeEventsList.Count - 1; i >= 0; i--)
    //        {
    //            WorldEvent activeEvent = _activeEventsList[i];
    //            activeEvent.NextDay();

    //            if (activeEvent.LeftDays == 0)
    //            {
    //                if (activeEvent.IsAffix)
    //                {
    //                    foreach (QuestSlot questSlot in _questKeeper.BoardSystem.QuestSlots)
    //                    {
    //                        for (int a = questSlot.Quest.AffixesList.Count - 1; a >= 0; a--)
    //                        {
    //                            if (questSlot.Quest.AffixesList[a].AffixData is WorldEventData affixEventData && activeEvent.EventData == affixEventData)
    //                            {
    //                                questSlot.Quest.AffixesList.Remove(questSlot.Quest.AffixesList[a]);
    //                            }
    //                        }
    //                    }
    //                }

    //                _activeEventsList.RemoveAt(i);
    //                _activeyDataEventsList.RemoveAt(i);

    //                activeEvent.CancelEvent(activeEvent.EventData);
    //            }
    //        }
    //    }

    //    if (_amountAvailableActiveEvents > _activeEventsList.Count)
    //    {
    //        CreateNewEvent();
    //    }
    //}

    private void CreateNewEvent()
    {
        int randomValue = UnityEngine.Random.Range(0, 101);

        if (_chanceNewEvent >= randomValue)
        {
            // ѕопробуем найти событие, которое еще не активно
            List<WorldEvent> availableEvents = new List<WorldEvent>();

            foreach (WorldEvent activeEvent in _primaryEventsList)
            {
                // ѕровер€ем, существует ли событие такого типа в списке активных событий
                bool isAlreadyActive = _activeEventSlotsList.Exists(e => e.WorldEvent.EventData.WorldEventType == activeEvent.EventData.WorldEventType);

                if (!isAlreadyActive)
                {
                    availableEvents.Add(activeEvent);
                }
            }

            // ≈сли есть доступные событи€, выбираем случайное из них
            if (availableEvents.Count > 0)
            {
                int randomEventIndex = UnityEngine.Random.Range(0, availableEvents.Count);
                WorldEvent copyEvent = CreateEventByType(availableEvents[randomEventIndex].EventData);
                //_activeEventSlotsList.Add(copyEvent);
                _activeyDataEventsList.Add(copyEvent.EventData);
                copyEvent.ApplyEvent(copyEvent.EventData);
                ShowEventUI(copyEvent);
                CreateEventSlot(copyEvent);

                if (copyEvent.IsAffix)
                {
                    Affix copyAffix = new Affix(copyEvent.EventData);

                    foreach (QuestSlot questSlot in _questKeeper.BoardSystem.QuestSlots)
                    {
                        questSlot.Quest.AffixesList.Add(copyAffix);
                    }
                }
            }
            else
            {
                Debug.Log("Ќет доступных уникальных событий дл€ добавлени€.");
            }
        }
    }

    private void CreateEventSlot(WorldEvent worldEvent)
    {
        WorldEventSlot_UI newWorldEventSlot_UI = Instantiate(_slotPrefab, _contentTransform.transform);
        newWorldEventSlot_UI.Init(worldEvent);

        _activeEventSlotsList.Add(newWorldEventSlot_UI);
    }

    private void ClearWorldEventSlots()
    {
        foreach (Transform worldEvent in _contentTransform.transform.Cast<Transform>())
        {
            Destroy(worldEvent.gameObject);
        }
    }

    private void ShowEventUI(WorldEvent newEvent)
    {
        _panel.gameObject.SetActive(true);

        _titleText.text = $"{newEvent.EventData.Name}";
        _eventText.text = $"{newEvent.EventData.Description}";
    }

    public void AddEventEnemy(EnemyData enemyData)
    {
        _eventEnemyList.Add(enemyData);
    }

    public void RemoveEventEnemy(EnemyData enemyData)
    {
        _eventEnemyList.Remove(enemyData);
    }

    public void SetEventEnemy()
    {
        _questKeeper.SetEventEnemy();
    }
}
