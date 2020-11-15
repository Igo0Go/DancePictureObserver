﻿using UnityEngine;
using System;
using System.Collections.Generic;

public class ActorCommandButton : ClickCommandObject
{
    [SerializeField, Tooltip("UI-панель с элементами управления для выбора действия с исполнителем")]
    private GameObject actorMenu = null;
    [SerializeField, Range(1, 300)]
    private float rotateSpeed = 100;

    [SerializeField]
    private List<Transform> rotateOrigins = null;

    public Action<ActorCommandButton> ButtonCliccked;

    private Transform actorMenuTransform;
    private LayerMask ignoreMask;
    private Camera cam;
    private Transform myTransform;
    private float clickRayDistance;
    private bool inMenu;
    private bool moveKey;
    private bool rotateKey;
    private int currentRotateOrigin;

    private void FixedUpdate()
    {
        Move();
    }

    private void Update()
    {
        Rotate();
    }

    /// <summary>
    /// Команда, которая будет выполнена при клике левой кнопкой мыши.
    /// В данном случае - открытие панели для выбора действия с исполнителем
    /// </summary>
    /// <param name="clickPosition">позиция клика, которую можно использовать в команде</param>
    public override void OnLeftCkickCommand(Vector2 clickPosition)
    {
        if(!inMenu)
        {
            moveKey = !moveKey;
            ButtonCliccked?.Invoke(this);
        }
    }

    /// <summary>
    /// Команда, которая будет выполнена при клике правой кнопкой мыши.
    /// Вывод (в консоль) справке по команде для этой кнопки
    /// </summary>
    /// <param name="clickPosition">позиция клика, которую можно использовать в команде</param>
    public override void OnRightClickCommand(Vector2 clickPosition)
    {
        if(!moveKey)
        {
            actorMenu.SetActive(true);
            ButtonCliccked?.Invoke(this);
        }
    }

    /// <summary>
    /// Вовзрат к состоянию до кликов - Закрытие меню выбора действия.
    /// </summary>
    public override void ReturnToDefaultState()
    {
        moveKey = rotateKey = inMenu = false;
        actorMenu.SetActive(false);
    }

    public override void ReturnToDefaultStateWithCheck(ClickCommandObject commandObject)
    {
        if(this != commandObject)
        {
            ReturnToDefaultState();
        }
    }

    public virtual void StartRotateAroundOrigin(int rotateOriginNumber)
    {
        currentRotateOrigin = rotateOriginNumber;
        rotateKey = true;
    }

    protected override void OnStartAction()
    {
        base.OnStartAction();
        cam = Camera.main;
        myTransform = transform;
        actorMenuTransform = actorMenu.transform;
        if(cam.TryGetComponent<ClickMenuController>(out ClickMenuController clickMenuController))
        {
            clickRayDistance = clickMenuController.clickRayDistance;
            ignoreMask = clickMenuController.ignoreMask;
        }
    }

    private void Rotate()
    {
        if(rotateKey)
        {
            float angle = -Mathf.Clamp(Input.GetAxis("Mouse X") + Input.GetAxis("Mouse Y"), -1,1) * rotateSpeed * Time.deltaTime;

            myTransform.RotateAround(rotateOrigins[currentRotateOrigin].position, Vector3.forward,angle);
            actorMenuTransform.Rotate(Vector3.forward, -angle);
        }
    }

    private void Move()
    {
        if (moveKey)
        {
            RaycastHit2D hit = Physics2D.Raycast(cam.ScreenToWorldPoint(Input.mousePosition),
                           Vector2.zero,
                           clickRayDistance,
                           ~ignoreMask
                           );

            myTransform.position = new Vector3(hit.point.x, hit.point.y, -1);
        }
    }

    private void OnDestroy()
    {
        menuController.UnsubscribingToAnEvent(this);
        ButtonCliccked += menuController.AllToDefaultExcludeThis;
    }
}
