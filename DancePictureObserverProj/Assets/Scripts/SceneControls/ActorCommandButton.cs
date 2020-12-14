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


    /// <summary>
    /// Записывает основные данные об этом элементе в строку
    /// </summary>
    /// <returns>Сохранённые данные</returns>
    public virtual string GetSaveString()
    {
        return ActorJSONHolder.GetSimpleActorJSONString(type, myTransform.position, myTransform.rotation);
    }

    /// <summary>
    /// На основе данных в формате строки выставляет настройки для этого элемента
    /// </summary>
    /// <param name="data">Настройки элемента в формате строки</param>
    public virtual void SetOptions(string data)
    {
        var options = ActorJSONHolder.GetOptionsForSimpleActor(data);

        transform.position = options.position;
        transform.rotation = options.rotation;
    }


    protected override void OnStartAction()
    {
        cam = Camera.main;
        myTransform = transform;
        actorMenuTransform = actorMenu.transform;
        if (cam.TryGetComponent<ClickMenuController>(out ClickMenuController clickMenuController))
        {
            clickRayDistance = clickMenuController.clickRayDistance;
            ignoreMask = clickMenuController.ignoreMask;
        }
        base.OnStartAction();
        CheckMenuOrientation();
    }

    protected virtual void CheckMenuOrientation()
    {
        actorMenuTransform.up = Vector2.up;
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

public static class ActorJSONHolder
{
    public static char[] separator = { '|' };

    public static string GetSimpleActorJSONString(InstanceType type, Vector3 position, Quaternion rotation)
    {
        return string.Format("{0}|{1}|{2}", type.ToString(), position.ToString(), rotation.ToString());
    }
    public static (Vector3 position, Quaternion rotation) GetOptionsForSimpleActor(string data)
    {
        string[] optionsStrings = data.Split(separator, StringSplitOptions.RemoveEmptyEntries);
        return (CreateVectorFromString(optionsStrings[1]), CreateQuaternionFromString(optionsStrings[2]));
    }

    public static string GetPairActorJSONString(InstanceType type, Vector3 position, Quaternion rotation, bool positionsStatus,
        bool armsStatus)
    {
        return string.Format("{0}|{1}|{2}|{3}|{4}", type.ToString(), position.ToString(), rotation.ToString(),
            positionsStatus.ToString(), armsStatus.ToString());
    }
    public static (Vector3 position, Quaternion rotation, bool positionsStatus, bool armsStatus) GetOptionsForPairActor(string data)
    {
        string[] optionsStrings = data.Split(separator, StringSplitOptions.RemoveEmptyEntries);
        return (CreateVectorFromString(optionsStrings[1]), CreateQuaternionFromString(optionsStrings[2]),
            bool.Parse(optionsStrings[3]), bool.Parse(optionsStrings[4]));
    }


    public static string GetLineActorJSONString(InstanceType type, Vector3 position, Quaternion rotation, string lineActors)
    {
        return string.Format("{0}|{1}|{2}|{3}", type.ToString(), position.ToString(), rotation.ToString(), lineActors);
    }
    public static (Vector3 position, Quaternion rotation, string lineActors) GetOptionsForLineActor(string data)
    {
        string[] optionsStrings = data.Split(separator, StringSplitOptions.RemoveEmptyEntries);
        return (CreateVectorFromString(optionsStrings[1]), CreateQuaternionFromString(optionsStrings[2]), optionsStrings[3]);
    }

    public static string GetDirectionJSONString(InstanceType type, Vector3 position, Quaternion rotation, bool usePoint, bool visiblePoint,
        Vector3 pos2, Quaternion rot2, Vector3 pos3, Quaternion rot3)
    {

        return string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}",
            type.ToString(), position.ToString(), rotation.ToString(), usePoint.ToString(),
            visiblePoint.ToString(), pos2.ToString(), rot2.ToString(), pos3.ToString(), rot3.ToString()) ;
    }
    public static (Vector3 position, Quaternion rotation, bool usePoint, bool visiblePoint,
        Vector3 pos2, Quaternion rot2, Vector3 pos3, Quaternion rot3) GetOptionsForDirection(string data)
    {
        string[] optionsStrings = data.Split(separator, StringSplitOptions.RemoveEmptyEntries);
        return (CreateVectorFromString(optionsStrings[1]), CreateQuaternionFromString(optionsStrings[2]),
            bool.Parse(optionsStrings[3]), bool.Parse(optionsStrings[4]),
            CreateVectorFromString(optionsStrings[5]), CreateQuaternionFromString(optionsStrings[6]),
            CreateVectorFromString(optionsStrings[7]), CreateQuaternionFromString(optionsStrings[8]));
    }

    public static InstanceType GetElementType(string data)
    {
        string bufer = data.Split(separator)[0];

        switch (bufer)
        {
            case "ManActor":
                return InstanceType.ManActor;
            case "GirlActor":
                return InstanceType.GirlActor;
            case "LineActor":
                return InstanceType.LineActor;
            case "PairActor":
                return InstanceType.PairActor;
            case "SimpleDirection":
                return InstanceType.SimpleDirection;
            case "DirectionWithPoint":
                return InstanceType.DirectionWithPoint;
            default:
                return InstanceType.ManActor;
        }
    }


    private static Vector3 CreateVectorFromString(string data)
    {
        char[] seporators = { '(', ')', ' ', ',' };
        string[] dataParts = data.Split(seporators, StringSplitOptions.RemoveEmptyEntries);
        return new Vector3(float.Parse(dataParts[0].Replace('.',',')), float.Parse(dataParts[1].Replace('.', ',')),
            float.Parse(dataParts[2].Replace('.', ',')));
    }
    private static Quaternion CreateQuaternionFromString(string data)
    {
        char[] seporators = { '(', ')', ' ', ',' };
        string[] dataParts = data.Split(seporators, StringSplitOptions.RemoveEmptyEntries);
        return new Quaternion(float.Parse(dataParts[0].Replace('.', ',')), float.Parse(dataParts[1].Replace('.', ',')),
            float.Parse(dataParts[2].Replace('.', ',')), float.Parse(dataParts[3].Replace('.', ',')));
    }
}
