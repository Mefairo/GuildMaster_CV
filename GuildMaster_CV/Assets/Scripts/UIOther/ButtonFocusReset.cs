using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonFocusReset : Button
{
    public override void OnSubmit(UnityEngine.EventSystems.BaseEventData eventData)
    {
        // Убираем обработку стандартных клавиш
    }
}
