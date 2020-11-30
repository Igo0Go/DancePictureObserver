using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCommandButton : ActorCommandButton
{
    [SerializeField]
    private Transform iconBuferTransform = null;
    [SerializeField]
    private SpriteRenderer lineSprite = null;
    [SerializeField]
    private GameObject girlActor = null;
    [SerializeField]
    private GameObject manActor = null;
    [SerializeField]
    private GameObject removeActorButton = null;

    public void AddActor(bool instanceGirl)
    {
        int childCount = iconBuferTransform.childCount;

        Vector3 newPos = childCount == 0 ? iconBuferTransform.position
            : iconBuferTransform.GetChild(childCount - 1).position + Vector3.right;

        Instantiate(instanceGirl ? girlActor : manActor,
            newPos,
            Quaternion.identity,
            iconBuferTransform);
        if (childCount > 0)
        {
            rotateOrigins[1].localPosition += rotateOrigins[1].right * 0.5f;
            rotateOrigins[2].localPosition -= rotateOrigins[2].right * 0.5f;
            iconBuferTransform.localPosition -= Vector3.right * 0.5f;
        }
        lineSprite.size += Vector2.right;
        removeActorButton.SetActive(true);
    }

    public void RemoveActor()
    {
        int childCount = iconBuferTransform.childCount - 1;
        if(childCount == 0)
        {
            removeActorButton.SetActive(false);
        }
        rotateOrigins[1].localPosition -= rotateOrigins[1].right * 0.5f;
        rotateOrigins[2].localPosition += rotateOrigins[2].right * 0.5f;
        Destroy(iconBuferTransform.GetChild(childCount).gameObject);
        iconBuferTransform.localPosition += Vector3.right * 0.5f;
        lineSprite.size -= Vector2.right;
    }

    protected override void CheckMenuOrientation()
    {
        int childCount = actorMenu.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            actorMenu.transform.GetChild(i).up = Vector2.up;
        }
    }
}
