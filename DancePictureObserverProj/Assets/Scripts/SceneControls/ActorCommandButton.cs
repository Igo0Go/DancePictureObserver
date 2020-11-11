using UnityEngine;
using System;

public class ActorCommandButton : ClickCommandObject
{
    [SerializeField, Tooltip("UI-панель с элементами управления для выбора действия с исполнителем")]
    private GameObject actorMenu = null;

    public Action<ActorCommandButton> ButtonCliccked;

    /// <summary>
    /// Команда, которая будет выполнена при клике левой кнопкой мыши.
    /// В данном случае - открытие панели для выбора действия с исполнителем
    /// </summary>
    /// <param name="clickPosition">позиция клика, которую можно использовать в команде</param>
    public override void OnLeftCkickCommand(Vector2 clickPosition)
    {
        actorMenu.SetActive(true);
        ButtonCliccked?.Invoke(this);
    }

    /// <summary>
    /// Команда, которая будет выполнена при клике правой кнопкой мыши.
    /// Вывод (в консоль) справке по команде для этой кнопки
    /// </summary>
    /// <param name="clickPosition">позиция клика, которую можно использовать в команде</param>
    public override void OnRightClickCommand(Vector2 clickPosition)
    {
        Debug.Log("Эта кнопка позволит вам выполнять действие с этим исполнителем");
    }

    /// <summary>
    /// Вовзрат к состоянию до кликов - Закрытие меню выбора действия.
    /// </summary>
    public override void ReturnToDefaultState()
    {
        actorMenu.SetActive(false);
    }

    public override void ReturnToDefaultStateWithCheck(ClickCommandObject commandObject)
    {
        if(this != commandObject)
        {
            ReturnToDefaultState();
        }
    }

    private void OnDestroy()
    {
        menuController.UnsubscribingToAnEvent(this);
        ButtonCliccked += menuController.AllToDefaultExcludeThis;
    }
}
