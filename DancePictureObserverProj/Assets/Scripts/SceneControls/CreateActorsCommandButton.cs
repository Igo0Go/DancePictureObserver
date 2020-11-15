using UnityEngine;

/// <summary>
/// Скрипт кнопки, отвечающей за добавление исполниелей на танцевальную площадку
/// </summary>
public class CreateActorsCommandButton : ClickCommandObject
{
    [SerializeField, Tooltip("UI-панель с элементами управления для выбора типа добавляемого исполнителя")]
    private GameObject actorsPanel = null;
    [SerializeField, Tooltip("Ссылка на скрипт танцевальной площадки")]
    private DanceField danceField = null;

    /// <summary>
    /// Команда, которая будет выполнена при клике левой кнопкой мыши.
    /// В данном случае - открытие панели для выбора типа добавляемого исполниеля
    /// </summary>
    /// <param name="clickPosition">позиция клика, которую можно использовать в команде</param>
    public override void OnLeftCkickCommand(Vector2 clickPosition)
    {
        actorsPanel.SetActive(true);
    }

    /// <summary>
    /// Команда, которая будет выполнена при клике правой кнопкой мыши.
    /// Вывод (в консоль) справке по команде для этой кнопки
    /// </summary>
    /// <param name="clickPosition">позиция клика, которую можно использовать в команде</param>
    public override void OnRightClickCommand(Vector2 clickPosition)
    {
        Debug.Log("Эта кнопка позволит вам добавить исполнителей на сцену");
    }

    /// <summary>
    /// Вовзрат к состоянию до кликов - Закрытие панели для выбора типа добавляемого исполнителя
    /// </summary>
    public override void ReturnToDefaultState()
    {
        actorsPanel.SetActive(false);
    }

    /// <summary>
    /// Добавить исполнител на площадку
    /// </summary>
    /// <param name="actor">GameObject префаба</param>
    public void InstanceActor(GameObject actor)
    {
        ActorCommandButton actorCommandButton = Instantiate(actor,
            transform.parent.position - Vector3.forward,
            Quaternion.identity).GetComponent<ActorCommandButton>();
        danceField.SubscribingToAnEvent(actorCommandButton);
        actorCommandButton.ButtonCliccked += menuController.AllToDefaultExcludeThis;
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
