using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NotificationSlot_UI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Button _buttonSelf;
    [SerializeField] private NotificationEnum _notifType;

    public Button ButtonSelf => _buttonSelf;
    public NotificationEnum NotifType => _notifType;

    public NotificationSystem NotificationSystem { get; private set; }

    private void Awake()
    {
        NotificationSystem = GetComponentInParent<NotificationSystem>();

        _buttonSelf?.onClick.AddListener(ClickSlot);
    }

    public void SetNotifType(NotificationEnum notifType)
    {
       _notifType = notifType;
    }

    private void ClickSlot()
    {
        switch (_notifType)
        {
            case NotificationEnum.None:
                break;

            case NotificationEnum.HeroStatsPoints:
                NotificationSystem.GuildKeeper.OpenWindow();
                break;

            case NotificationEnum.HeroAbilityPoints:
                NotificationSystem.GuildKeeper.OpenWindow();
                break;

            case NotificationEnum.GuildStatPoints:
                NotificationSystem.GuildTalentSystem.OpenPanel();
                break;

            case NotificationEnum.ExpeditionResearch:
                NotificationSystem.QuestKeeper.OpenQuestWindow();
                break;

            case NotificationEnum.NotGoldForWeeklyPay:
                NotificationSystem.PayKeeper.OpenPayWindow();
                break;

            case NotificationEnum.QuestShowResult:
                NotificationSystem.TakingQuestKeeper.OpenQuestWindow();
                break;

            case NotificationEnum.NewRecipeUnlock:
                NotificationSystem.DrawingKeeperDisplay.OpenDrawingWindow();
                break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        InfoPanelUI.Instance.ShowNotificationInfo(_notifType);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InfoPanelUI.Instance.HideInfo();
    }
}
