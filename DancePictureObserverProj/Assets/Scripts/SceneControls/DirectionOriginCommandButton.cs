﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionOriginCommandButton : DirectionCommandButton
{
    public List<DirectionCommandButton> pointers;

    [SerializeField]
    private List<GameObject> menuButtons;

    private bool usePoint;
    private bool useMarker;

    public void AddPoint()
    {
        usePoint = true;
        directionRenderer.ActivatePoint(1);
        menuButtons[0].SetActive(false);
        menuButtons[1].SetActive(true);
        menuButtons[2].SetActive(false);
        menuButtons[3].SetActive(true);
        pointers[0].gameObject.SetActive(true);
    }
    public void RemovePoint()
    {
        usePoint = false;
        menuButtons[0].SetActive(true);
        menuButtons[1].SetActive(false);
        menuButtons[2].SetActive(false);
        menuButtons[3].SetActive(false);
        directionRenderer.DeactivatePoint(1);

    }
    public void AddMarker()
    {
        useMarker = true;
        menuButtons[0].SetActive(false);
        menuButtons[1].SetActive(true);
        menuButtons[2].SetActive(false);
        menuButtons[3].SetActive(true);
        pointers[0].gameObject.SetActive(true);
    }
    public void RemoveMarker()
    {
        useMarker = false;
        menuButtons[0].SetActive(false);
        menuButtons[1].SetActive(true);
        menuButtons[2].SetActive(true);
        menuButtons[3].SetActive(false);
        pointers[0].gameObject.SetActive(false);
    }

    //protected override void OnStartAction()
    //{
    //    base.OnStartAction();
    //    Invoke("SetParentsForPoints", 1);
    //}

    private void SetParentsForPoints()
    {
        pointers[0].transform.parent = transform;
        pointers[1].transform.parent = pointers[2].transform;
    }

    public override void DeleteActor()
    {
        directionRenderer.checkCurve = false;

        foreach (var item in pointers)
        {
            item.DeleteActor();
        }

        base.DeleteActor();
    }


    public override string GetSaveString()
    {
        return ActorJSONHolder.GetDirectionJSONString(type, myTransform.position, myTransform.rotation, usePoint, useMarker,
            pointers[0].transform.position, pointers[0].transform.rotation,
            pointers[1].transform.position, pointers[1].transform.rotation);
    }

    public override void SetOptions(string data)
    {
        StartCoroutine(SetOptionCoroutine(data));
    }

    private IEnumerator SetOptionCoroutine(string data)
    {
        var options = ActorJSONHolder.GetOptionsForDirection(data);

        transform.position = options.position;
        transform.rotation = options.rotation;

        yield return new WaitForSeconds(0.05f);

        if (options.usePoint)
        {
            AddPoint();
        }
        if (options.visiblePoint)
        {
            AddMarker();
        }

        pointers[0].SetTransformSettings(options.pos2, options.rot2);
        pointers[1].SetTransformSettings(options.pos3, options.rot3);

        directionRenderer.RedrawLine();
    }
}
