using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCommandButton : ActorCommandButton
{
    [SerializeField]
    private Transform actorsContainerTransform = null;
    [SerializeField]
    private SpriteRenderer lineSprite = null;
    [SerializeField]
    private GameObject girlActor = null;
    [SerializeField]
    private GameObject manActor = null;
    [SerializeField]
    private GameObject removeActorButton = null;

    private string lineCode = string.Empty;

    public void AddActor(bool instanceGirl)
    {
        lineCode += instanceGirl ? "g" : "b";

        int childCount = actorsContainerTransform.childCount;

        Vector3 newPos = childCount == 0 ? actorsContainerTransform.position
            : actorsContainerTransform.GetChild(childCount - 1).position + actorsContainerTransform.right;

        Instantiate(instanceGirl ? girlActor : manActor,
            newPos,
            Quaternion.identity,
            actorsContainerTransform).transform.localRotation = Quaternion.Euler(Vector3.zero);
        if (childCount > 0)
        {
            rotateOrigins[1].position += rotateOrigins[1].right * 0.5f;
            rotateOrigins[2].position -= rotateOrigins[2].right * 0.5f;
            actorsContainerTransform.localPosition -= Vector3.right *0.5f;
        }
        lineSprite.size += Vector2.right;
        removeActorButton.SetActive(true);
    }

    public void RemoveActor()
    {
        lineCode = lineCode.Substring(0, lineCode.Length - 1);

        int childCount = actorsContainerTransform.childCount - 1;
        if(childCount == 0)
        {
            removeActorButton.SetActive(false);
            actorsContainerTransform.localPosition += Vector3.zero;
            rotateOrigins[1].localPosition = Vector3.right;
            rotateOrigins[2].localPosition = Vector3.left;
        }
        else
        {
            actorsContainerTransform.localPosition += Vector3.right * 0.5f;
            rotateOrigins[1].position -= rotateOrigins[1].right * 0.5f;
            rotateOrigins[2].position += rotateOrigins[2].right * 0.5f;
        }

        Destroy(actorsContainerTransform.GetChild(childCount).gameObject);
        
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

    public override string GetSaveString()
    {
        return ActorJSONHolder.GetLineActorJSONString(type, myTransform.position, myTransform.rotation, lineCode);
    }

    public override void SetOptions(string data)
    {
        var options = ActorJSONHolder.GetOptionsForLineActor(data);

        transform.position = options.position;
        transform.rotation = options.rotation;

        char[] code = options.lineActors.ToCharArray();
        for (int i = 0; i < code.Length; i++)
        {
            AddActor(code[i] == 'g');
        }
    }
}
