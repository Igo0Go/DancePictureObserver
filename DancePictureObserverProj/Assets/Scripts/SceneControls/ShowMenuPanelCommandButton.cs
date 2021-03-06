﻿using UnityEngine;

/// <summary>
/// Скрипт кнопки, отвечающей за добавление исполниелей на танцевальную площадку
/// </summary>
public class ShowMenuPanelCommandButton : ClickCommandObject
{
    [SerializeField, Tooltip("UI-панель с элементами управления для выбора типа добавляемого объекта")]
    private GameObject menuPanel = null;
    [SerializeField, Tooltip("Ссылка на скрипт танцевальной площадки")]
    private DanceField danceField = null;
    [SerializeField, Tooltip("Подсказка, которая будет высвечиваться при правом клике")]
    private string tooltip;


    /// <summary>
    /// Команда, которая будет выполнена при клике левой кнопкой мыши.
    /// В данном случае - открытие панели для выбора типа добавляемого исполниеля
    /// </summary>
    /// <param name="clickPosition">позиция клика, которую можно использовать в команде</param>
    public override void OnLeftCkickCommand(Vector2 clickPosition)
    {
        menuPanel.SetActive(true);
    }

    /// <summary>
    /// Команда, которая будет выполнена при клике правой кнопкой мыши.
    /// Вывод (в консоль) справке по команде для этой кнопки
    /// </summary>
    /// <param name="clickPosition">позиция клика, которую можно использовать в команде</param>
    public override void OnRightClickCommand(Vector2 clickPosition)
    {
        Debug.Log(tooltip);
    }

    /// <summary>
    /// Вовзрат к состоянию до кликов - Закрытие панели для выбора типа добавляемого исполнителя
    /// </summary>
    public override void ReturnToDefaultState()
    {
        menuPanel.SetActive(false);
    }

    /// <summary>
    /// Добавить исполнител на площадку
    /// </summary>
    /// <param name="actor">GameObject префаба</param>
    public void InstanceActor(GameObject actor)
    {
        danceField.InstanceActor(actor, transform.parent.position - Vector3.forward);
        ReturnToDefaultState();
    }

    /// <summary>
    /// Добавить перемеение на площадку
    /// </summary>
    /// <param name="actor">GameObject префаба</param>
    public void InstanceDirection(GameObject directionPrefab)
    {
        danceField.InstanceDirection(directionPrefab);
        ReturnToDefaultState();
    }

    public override void ReturnToDefaultStateWithCheck(ClickCommandObject commandObject)
    {
        if(this != commandObject)
        {
            ReturnToDefaultState();
        }
    }
}
