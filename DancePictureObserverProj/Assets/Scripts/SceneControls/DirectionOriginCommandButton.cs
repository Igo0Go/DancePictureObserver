using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionOriginCommandButton : DirectionCommandButton
{
    public List<DirectionCommandButton> pointers;

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
}
