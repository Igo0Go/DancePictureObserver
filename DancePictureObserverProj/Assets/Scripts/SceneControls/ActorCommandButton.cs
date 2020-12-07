using UnityEngine;
using System;
using System.Collections.Generic;

public class ActorCommandButton : ClickCommandObject
{
    [SerializeField, Tooltip("UI-панель с элементами управления для выбора действия с исполнителем")]
    protected GameObject actorMenu = null;
    [SerializeField]
    protected InstanceType type = InstanceType.ManActor;
    [SerializeField]
    protected List<Transform> rotateOrigins = null;


    public Action<ActorCommandButton> ButtonCliccked;
    public Action<ActorCommandButton> ObjectDeleted;

    private Transform actorMenuTransform;
    private LayerMask ignoreMask;
    private Camera cam;
    protected Transform myTransform;
    private float clickRayDistance;
    protected bool inMenu;
    protected bool moveKey;
    protected bool rotateKey;
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
        myTransform.parent = null;
        foreach (var item in rotateOrigins)
        {
            item.SetParent(myTransform);
        }
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
        var lastOrigin = rotateOrigins[currentRotateOrigin];
        currentRotateOrigin = rotateOriginNumber;
        rotateOrigins[currentRotateOrigin].parent = null;
        lastOrigin.SetParent(rotateOrigins[currentRotateOrigin]);
        myTransform.SetParent(rotateOrigins[currentRotateOrigin]);

        rotateKey = true;
    }

    public virtual void DeleteActor()
    {
        ObjectDeleted?.Invoke(this);
        Destroy(gameObject);
    }

    protected override void OnStartAction()
    {
        cam = Camera.main;
        myTransform = transform;
        actorMenuTransform = actorMenu.transform;
        if(cam.TryGetComponent<ClickMenuController>(out ClickMenuController clickMenuController))
        {
            clickRayDistance = clickMenuController.clickRayDistance;
            ignoreMask = clickMenuController.ignoreMask;
        }
        base.OnStartAction();
        CheckMenuOrientation();
    }

    public virtual ActorJSONHolder GetHolder()
    {
        return new ActorJSONHolder(type, myTransform.position, myTransform.rotation);
    }

    private void Rotate()
    {
        if(rotateKey)
        {
            RaycastHit2D hit = Physics2D.Raycast(cam.ScreenToWorldPoint(Input.mousePosition),
               Vector2.zero,
               clickRayDistance,
               ~ignoreMask
               );

            Vector2 dir = hit.point - new Vector2(rotateOrigins[currentRotateOrigin].position.x,
                rotateOrigins[currentRotateOrigin].position.y);
            rotateOrigins[currentRotateOrigin].up = dir;
            CheckMenuOrientation();
        }
    }

    protected virtual void CheckMenuOrientation()
    {
        actorMenuTransform.up = Vector2.up;
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

    public virtual void SetOptions(ActorJSONHolder holder)
    {
        transform.position = holder.actorPosition;
        transform.rotation = Quaternion.Euler(holder.actorRotation);
    }

    private void OnDestroy()
    {
            menuController.UnsubscribingToAnEvent(this);
            ButtonCliccked += menuController.AllToDefaultExcludeThis;
    }
}

[Serializable]
public class ActorJSONHolder
{
    public InstanceType actorType;
    public Vector3 actorPosition;
    public Vector3 actorRotation;

    public ActorJSONHolder(InstanceType type, Vector3 position, Quaternion rotation)
    {
        actorType = type;
        actorPosition = position;
        actorRotation = rotation.eulerAngles;
    }

    public bool actorsPostionsnChanged;
    public bool actorsArmsChanged;

    public ActorJSONHolder(InstanceType type, Vector3 position, Quaternion rotation, bool positionsStatus, bool armsStatus)
    {
        actorType = type;
        actorPosition = position;
        actorRotation = rotation.eulerAngles;
        actorsArmsChanged = armsStatus;
        actorsPostionsnChanged = positionsStatus;
    }

    public string actorsInLine;

    public ActorJSONHolder(InstanceType type, Vector3 position, Quaternion rotation, string lineActors)
    {
        actorType = type;
        actorPosition = position;
        actorRotation = rotation.eulerAngles;
        actorsInLine = lineActors;
    }


    public bool supportPoint;
    public bool supportPointVisible;
    public Vector3 point2Pos;
    public Vector3 point2Rot;
    public Vector3 point3Pos;
    public Vector3 point3Rot;

    public ActorJSONHolder(InstanceType type, Vector3 position, Quaternion rotation, bool usePoint, bool visiblePoint,
        Vector3 pos2, Quaternion rot2, Vector3 pos3, Quaternion rot3)
    {
        actorType = type;
        actorPosition = position;
        actorRotation = rotation.eulerAngles;
        supportPoint = usePoint;
        supportPointVisible = visiblePoint;
        point2Pos = pos2;
        point2Rot = rot2.eulerAngles;
        point3Pos = pos3;
        point3Rot = rot3.eulerAngles;
    }
}
