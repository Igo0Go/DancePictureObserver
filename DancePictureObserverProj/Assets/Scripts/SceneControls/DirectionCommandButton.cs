using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionCommandButton : ActorCommandButton
{
    [SerializeField]
    protected DirectionRenderer directionRenderer = null;

    /// <summary>
    /// Команда, которая будет выполнена при клике левой кнопкой мыши.
    /// В данном случае - открытие панели для выбора действия с исполнителем
    /// </summary>
    /// <param name="clickPosition">позиция клика, которую можно использовать в команде</param>
    public override void OnLeftCkickCommand(Vector2 clickPosition)
    {
        base.OnLeftCkickCommand(clickPosition);
        directionRenderer.checkCurve = moveKey;
    }

    /// <summary>
    /// Вовзрат к состоянию до кликов - Закрытие меню выбора действия.
    /// </summary>
    public override void ReturnToDefaultState()
    {
        moveKey = rotateKey = inMenu = false;
        actorMenu.SetActive(false);
        foreach (var item in rotateOrigins)
        {
            item.SetParent(myTransform);
        }
        directionRenderer.checkCurve = false;
    }

    public override void StartRotateAroundOrigin(int rotateOriginNumber)
    {
        base.StartRotateAroundOrigin(rotateOriginNumber);
        directionRenderer.checkCurve = rotateKey;
    }
}
