using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(LineRenderer))]
public class DirectionRenderer : MonoBehaviour
{
    public bool checkCurve;

    [SerializeField, Tooltip("Все опорные точки")]
    private List<ControllPoint> allPoints = null;
    [SerializeField]
    private List<GameObject> supportMarkers;
    [SerializeField, Range(4, 20), Tooltip("Количество сегментов кривой между опорными точками (гладкость)")]
    private int segmentsCount = 7;


    private List<Transform> currentControlPoints = null;
    private LineRenderer line;
    private Transform myTransform;

    private Action renderMethod;

    private void Start()
    {
        checkCurve = false;
        myTransform = transform;
        Prepare();
    }

    public void Prepare()
    {
        line = GetComponent<LineRenderer>();
        currentControlPoints = new List<Transform>();
        SetActiveForPoint(0, true);
    }

    public void SetActiveForPoint(int number, bool activeValue)
    {
        allPoints[number].isActive = activeValue;

        currentControlPoints.Clear();

        allPoints[number].point.gameObject.SetActive(activeValue);

        foreach (var item in allPoints)
        {
            if(item.isActive)
            {
                currentControlPoints.Add(item.point);
            }
        }

        SetRenderMethod(currentControlPoints.Count);
    }

    public void ActivatePoint(int number) => SetActiveForPoint(number, true);
    public void DeactivatePoint(int number) => SetActiveForPoint(number, false);

    private void Update()
    {
        if(checkCurve)
        {
            renderMethod();
            SetDirectionForPointer();
        }
    }


    private void DrawSimpleLine()
    {
        for (int i = 0; i < currentControlPoints.Count; i++)
        {
            line.SetPosition(i, currentControlPoints[i].position);
        }
    }

    private void DrawCurveWith3Points()
    {
        line.SetPosition(0, currentControlPoints[0].position);

        Vector3 p0 = currentControlPoints[0].position;
        Vector3 p1 = myTransform.InverseTransformPoint(currentControlPoints[1].position);
        Vector3 p2 = currentControlPoints[2].position;

        for (int i = 1; i <= segmentsCount; i++)
        {
            Vector3 point = Bezier.GetPoint(p0, p1, p2, (float)i / segmentsCount);
            line.SetPosition(i, point);
        }
    }

    private void DrawCurveWith4Points()
    {
        line.SetPosition(0, currentControlPoints[0].position);

        Vector3 p0 = currentControlPoints[0].position;
        Vector3 p1 = myTransform.InverseTransformPoint(currentControlPoints[1].position);
        Vector3 p2 = myTransform.InverseTransformPoint(currentControlPoints[2].position);
        Vector3 p3 = currentControlPoints[3].position;

        for (int i = 1; i <= segmentsCount * 2; i++)
        {
            Vector3 point = Bezier.GetPoint(p0, p1, p2, p3, (float)i / segmentsCount);
            line.SetPosition(i, point);
        }
    }

    private void SetRenderMethod(int pointCount)
    {
        switch (pointCount)
        {
            case 2:
                line.positionCount = 2;
                renderMethod = DrawSimpleLine;
                break;
            case 3:
                line.positionCount = 1 + segmentsCount;
                renderMethod = DrawCurveWith3Points;
                break;
            case 4:
                line.positionCount = 1 + segmentsCount*2;
                renderMethod = DrawCurveWith4Points;
                break;
            default:
                line.positionCount = 2;
                renderMethod = DrawSimpleLine;
                break;
        }

        renderMethod();
        SetDirectionForPointer();
    }

    private void SetDirectionForPointer()
    {
        Vector3 dir = line.GetPosition(line.positionCount - 1) - line.GetPosition(line.positionCount - 2);
        allPoints[allPoints.Count - 1].point.up = dir.normalized;
        dir = line.GetPosition(1) - line.GetPosition(0);
        allPoints[0].point.up = dir.normalized;
    }
}

[System.Serializable]
public class ControllPoint
{
    public Transform point;
    public bool isActive;
}
