using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Скрипт танцевальной площадки
/// </summary>
public class DanceField : ClickCommandObject
{
    [SerializeField] private GameObject menuObj = null;

    /// <summary>
    /// Команда, которая будет выполнена при клике левой кнопкой мыши.
    /// В данном случае - закрытие контекстного меню
    /// </summary>
    /// <param name="clickPosition">позиция клика, которую можно использовать в команде</param>
    public override void OnLeftCkickCommand(Vector2 clickPosition)
    {
        ReturnToDefaultState();
    }

    /// <summary>
    /// Команда, которая будет выполнена при клике правой кнопкой мыши.
    /// В данном случае - открытие контекстного меню
    /// </summary>
    /// <param name="clickPosition">позиция клика, которую можно использовать в команде</param>
    public override void OnRightClickCommand(Vector2 clickPosition)
    {
        menuObj.transform.position = clickPosition;
        menuObj.SetActive(true);
    }

    /// <summary>
    /// Возврат к состоянию до кликов - закрытие контекстного меню
    /// </summary>
    public override void ReturnToDefaultState()
    {
        menuObj.SetActive(false);
    }
}
