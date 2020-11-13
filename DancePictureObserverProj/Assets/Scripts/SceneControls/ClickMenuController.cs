using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Данный класс используется для отслеживания кликов по интерактивным объектам и мимо них.
/// </summary>
public class ClickMenuController : MonoBehaviour, IEmptyClickEventSender
{
    [SerializeField] private Camera cam = null;
    public LayerMask ignoreMask = 0;

    /// <summary>
    /// Данное событие вызывается, когда пользователь кликнул мимо танцевальной площадки любой кнопокй
    /// </summary>
    public event Action EmptyClickEvent;
    /// <summary>
    /// Данное событие вызывается, когда нужно перевести в изначальное состояние все объекты, кроме указанного
    /// </summary>
    public event Action<ClickCommandObject> EmptyClickEventExcludeObject;

    private List<ClickCommandObject> interactiveObjectsOnScene;
    [HideInInspector] public float clickRayDistance = 50;

    private void Awake()
    {
        interactiveObjectsOnScene = new List<ClickCommandObject>();
    }

    private void Update()
    {
        CheckClick();
    }

    private void CheckClick()
    {
        int buttonNumber = 0;

        if (Input.GetMouseButtonDown(0))
        {
            buttonNumber = -1;
        }
        else if (Input.GetMouseButtonDown(1))
        {
            buttonNumber = 1;
        }

        if (buttonNumber != 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(cam.ScreenToWorldPoint(Input.mousePosition),
               Vector2.zero,
               clickRayDistance,
               ~ignoreMask
               );

            Debug.Log(hit.collider);

            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent<ClickCommandObject>(out ClickCommandObject commandObject))
                {
                    if (buttonNumber > 0)
                    {
                        commandObject.OnRightClickCommand(hit.point);
                        return;
                    }
                    else
                    {
                        commandObject.OnLeftCkickCommand(hit.point);
                        return;
                    }
                }
            }
            EmptyClickEvent?.Invoke();
        }
    }

    /// <summary>
    /// Подписать передаваемый объект на события EmptyClick, которое вызывается, когда пользователь кликнул
    /// мимо танцевальной площадки
    /// </summary>
    /// <param name="commandObject">Объект, который будет подписан на событие (если ещё не подписан). У этого объекта будет
    /// вызываться метод ReturnToDefaultState</param>
    public void SubscribingToAnEvent(ClickCommandObject commandObject)
    {
        if (!interactiveObjectsOnScene.Contains(commandObject))
        {
            interactiveObjectsOnScene.Add(commandObject);
            EmptyClickEvent += commandObject.ReturnToDefaultState;
            EmptyClickEventExcludeObject += commandObject.ReturnToDefaultStateWithCheck;
        }
    }
    /// <summary>
    /// Отписать передаваемый объект от события EmptyClick, которое вызывается, когда пользователь кликнул
    /// мимо танцевальной площадки
    /// </summary>
    /// <param name="commandObject">Объект, который будет отписан от событие (если был подписан, иначе будет будет выдан Exception).</param>
    public void UnsubscribingToAnEvent(ClickCommandObject commandObject)
    {
        if (interactiveObjectsOnScene.Contains(commandObject))
        {
            interactiveObjectsOnScene.Remove(commandObject);
            EmptyClickEvent -= commandObject.ReturnToDefaultState;
            EmptyClickEventExcludeObject -= commandObject.ReturnToDefaultStateWithCheck;
        }
        else
        {
            throw new Exception(string.Format("Невозможно провести отписку! Объект {0} не найден " +
                "в коллекции подписчиков на событие EmptyClick.",
                commandObject.name));
        }
    }
/// <summary>
/// Перевести в изначальное состояние все объекты кроме указанного
/// </summary>
/// <param name="commandObject">исключаемый объект</param>
    public void AllToDefaultExcludeThis(ClickCommandObject commandObject)
    {
        EmptyClickEventExcludeObject?.Invoke(commandObject);
    }
    private void OnDestroy()
    {
        EmptyClickEvent = null;
    }
}
