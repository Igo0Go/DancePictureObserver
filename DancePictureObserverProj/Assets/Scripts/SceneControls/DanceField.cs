using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Скрипт танцевальной площадки
/// </summary>
public class DanceField : ClickCommandObject, IEmptyClickEventSender
{
    [SerializeField] private GameObject menuObj = null;

    [SerializeField] private List<GameObject> instancePrefabs;

    private List<ClickCommandObject> interactiveObjectsOnField;


    /// <summary>
    /// Данное событие вызывается, когда пользователь кликнул мимо танцевальной площадки любой кнопокй
    /// </summary>
    public event Action EmptyClickEvent;

    protected override void OnStartAction()
    {
        interactiveObjectsOnField = new List<ClickCommandObject>();
        ReturnToDefaultState();
    }

    /// <summary>
    /// Команда, которая будет выполнена при клике левой кнопкой мыши.
    /// В данном случае - закрытие контекстного меню
    /// </summary>
    /// <param name="clickPosition">позиция клика, которую можно использовать в команде</param>
    public override void OnLeftCkickCommand(Vector2 clickPosition)
    {
        ReturnToDefaultState();
        EmptyClickEvent?.Invoke();
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

    /// <summary>
    /// Подписать передаваемый объект на события EmptyClick, которое вызывается, когда пользователь кликнул
    /// мимо танцевальной площадки
    /// </summary>
    /// <param name="commandObject">Объект, который будет подписан на событие (если ещё не подписан). У этого объекта будет
    /// вызываться метод ReturnToDefaultState</param>
    public void SubscribingToAnEvent(ClickCommandObject commandObject)
    {
        if (!interactiveObjectsOnField.Contains(commandObject))
        {
            interactiveObjectsOnField.Add(commandObject);
            EmptyClickEvent += commandObject.ReturnToDefaultState;
        }
    }
    /// <summary>
    /// Отписать передаваемый объект от события EmptyClick, которое вызывается, когда пользователь кликнул
    /// мимо танцевальной площадки
    /// </summary>
    /// <param name="commandObject">Объект, который будет отписан от событие (если был подписан, иначе будет будет выдан Exception).</param>
    public void UnsubscribingToAnEvent(ClickCommandObject commandObject)
    {
        if (interactiveObjectsOnField.Contains(commandObject))
        {
            interactiveObjectsOnField.Remove(commandObject);
            EmptyClickEvent -= commandObject.ReturnToDefaultState;
            if(commandObject is ActorCommandButton actorCommandButton)
            {
                actorCommandButton.ObjectDeleted -= UnsubscribingToAnEvent;
            }
        }
        else
        {
            throw new Exception(string.Format("Невозможно провести отписку! Объект {0} не найден " +
                "в коллекции подписчиков на событие EmptyClick.",
                commandObject.name));
        }
    }

    /// <summary>
    /// Добавить исполнител на площадку
    /// </summary>
    /// <param name="actor">GameObject префаба</param>
    public void InstanceActor(GameObject actor, Vector3 pos)
    {
        ActorCommandButton actorCommandButton = Instantiate(actor,
            pos,
            Quaternion.Euler(0, 0, 180)).GetComponent<ActorCommandButton>();
        SubscribingToAnEvent(actorCommandButton);
        actorCommandButton.ButtonCliccked += menuController.AllToDefaultExcludeThis;
        actorCommandButton.ObjectDeleted += UnsubscribingToAnEvent;
        ReturnToDefaultState();
    }

    /// <summary>
    /// Добавить перемеение на площадку
    /// </summary>
    /// <param name="actor">GameObject префаба</param>
    public void InstanceDirection(GameObject directionPrefab)
    {
        DirectionOriginCommandButton directionCommandButton = Instantiate(directionPrefab,
            transform.parent.position - Vector3.forward,
            Quaternion.identity).GetComponent<DirectionOriginCommandButton>();

        SubscribingToAnEvent(directionCommandButton);
        directionCommandButton.ButtonCliccked += menuController.AllToDefaultExcludeThis;
        directionCommandButton.ObjectDeleted += UnsubscribingToAnEvent;

        foreach (var pointer in directionCommandButton.pointers)
        {

            SubscribingToAnEvent(pointer);
            pointer.ButtonCliccked += menuController.AllToDefaultExcludeThis;
            pointer.ObjectDeleted += UnsubscribingToAnEvent;
        }

        directionCommandButton.pointers[1].transform.parent = null;

        ReturnToDefaultState();
    }

    /// <summary>
    /// Добавить исполнител на площадку
    /// </summary>
    /// <param name="actor">GameObject префаба</param>
    private void InstanceActor(ActorJSONHolder holder)
    {
        ActorCommandButton actorCommandButton = Instantiate(instancePrefabs[(int)(holder.actorType)],
            Vector3.zero,
            Quaternion.Euler(0, 0, 180)).GetComponent<ActorCommandButton>();
        SubscribingToAnEvent(actorCommandButton);
        actorCommandButton.ButtonCliccked += menuController.AllToDefaultExcludeThis;
        actorCommandButton.ObjectDeleted += UnsubscribingToAnEvent;
        ReturnToDefaultState();
        actorCommandButton.SetOptions(holder);
    }


    private void OnDestroy()
    {
        EmptyClickEvent = null;
    }

    public override void ReturnToDefaultStateWithCheck(ClickCommandObject commandObject)
    {
        if(this != commandObject)
        {
            ReturnToDefaultState();
        }
    }

    public List<ActorJSONHolder> GetHoldersOnField()
    {
        List<ActorJSONHolder> result = new List<ActorJSONHolder>();

        foreach (var item in interactiveObjectsOnField)
        {
            if(item.TryGetComponent<ActorCommandButton>(out ActorCommandButton actor))
            {
                result.Add(actor.GetHolder());
            }
        }

        return result;
    }

    public void InstenceActorsWithSettings(List<ActorJSONHolder> holders)
    {
        for (int i = 0; i < interactiveObjectsOnField.Count; i++)
        {
            ActorCommandButton bufer;
            if(interactiveObjectsOnField[i].TryGetComponent<ActorCommandButton>(out bufer))
            {
                bufer.DeleteActor();
                i--;
            }
        }

        foreach (var item in holders)
        {
            InstanceActor(item);
        }
    }
}
