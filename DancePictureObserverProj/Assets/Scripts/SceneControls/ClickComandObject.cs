using UnityEngine;

/// <summary>
/// Заготовка для скриптов объектов, которые должны реагировать на клики мышкой.
/// У наследников вместо использования метода Start() следует переопределять метод OnStartAction().
/// Это требуется для автоматической подписки на события и отписки от них
/// </summary>
[RequireComponent(typeof(Collider2D))]
public abstract class ClickCommandObject : MonoBehaviour
{
    [SerializeField] protected ClickMenuController menuController = null;

    protected virtual void OnStartAction() => ReturnToDefaultState();

    private void Start()
    {
        menuController.SubscribingToAnEvent(this);
        OnStartAction();
    }

    /// <summary>
    /// Команда, которая будет выполнена при клике правой кнопкой мыши
    /// </summary>
    /// <param name="clickPosition">Позиция клика, которую можно использовать в команде</param>
    public abstract void OnRightClickCommand(Vector2 clickPosition);
    /// <summary>
    /// Команда, которая будет выполнена при клике левой кнопкой мыши
    /// </summary>
    /// <param name="clickPosition">Позиция клика, которую можно использовать в команде</param>
    public abstract void OnLeftCkickCommand(Vector2 clickPosition);
    /// <summary>
    /// Возврат объекта к состоянию, когда на него ещё не кликнули
    /// </summary>
    public abstract void ReturnToDefaultState();

    private void OnDestroy()
    {
        menuController.UnsubscribingToAnEvent(this);
    }
}
