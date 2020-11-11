using System;

public interface IEmptyClickEventSender
{
    /// <summary>
    /// Данное событие вызывается, когда пользователь кликнул мимо танцевальной площадки любой кнопокй
    /// </summary>
    event Action EmptyClickEvent;

    /// <summary>
    /// Подписать передаваемый объект на события EmptyClick, которое вызывается, когда пользователь кликнул
    /// мимо танцевальной площадки
    /// </summary>
    /// <param name="commandObject">Объект, который будет подписан на событие (если ещё не подписан). У этого объекта будет
    /// вызываться метод ReturnToDefaultState</param>
    void SubscribingToAnEvent(ClickCommandObject commandObject);
    /// <summary>
    /// Отписать передаваемый объект от события EmptyClick, которое вызывается, когда пользователь кликнул
    /// мимо танцевальной площадки
    /// </summary>
    /// <param name="commandObject">Объект, который будет отписан от событие (если был подписан, иначе будет будет выдан Exception).</param>
    void UnsubscribingToAnEvent(ClickCommandObject commandObject);
}
