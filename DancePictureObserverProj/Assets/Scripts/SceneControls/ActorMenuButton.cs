using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActorMenuButton : ClickCommandObject
{
    [SerializeField, Tooltip("Действие при клике левой кнопокй")] private UnityEvent LeftButtonClicked;
    [SerializeField, Tooltip("Действие при клике правой кнопокй")] private UnityEvent RightButtonClicked;
    [SerializeField, Tooltip("Действие при возврате в состояние по уолчанию")] private UnityEvent ReturnToDefault;

    public override void OnLeftCkickCommand(Vector2 clickPosition)
    {
        LeftButtonClicked?.Invoke();
    }

    public override void OnRightClickCommand(Vector2 clickPosition)
    {
        RightButtonClicked?.Invoke();
    }

    public override void ReturnToDefaultState()
    {
        ReturnToDefault?.Invoke();
    }

    public override void ReturnToDefaultStateWithCheck(ClickCommandObject commandObject)
    {
        if(commandObject != this)
        {
            ReturnToDefault?.Invoke();
        }
    }
}
