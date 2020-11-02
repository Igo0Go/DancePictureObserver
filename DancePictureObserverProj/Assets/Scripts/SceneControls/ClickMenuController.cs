using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickMenuController : MonoBehaviour
{
    [SerializeField] private GameObject menuObj = null;
    [SerializeField] private Camera cam = null;
    [SerializeField] private LayerMask ignoreMask = 0;

    private const float clickRayDistance = 50;

    private void Start()
    {
        menuObj.SetActive(false);
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

        if(buttonNumber != 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(cam.ScreenToWorldPoint(Input.mousePosition),
               Vector2.zero,
               clickRayDistance,
               ~ignoreMask
               );

            Debug.Log(hit.collider);

            if (hit.collider != null)
            {
                switch (hit.collider.tag)
                {
                    case "Background":
                        if(buttonNumber > 0)
                        {
                            Vector2 clickPoint = hit.point;
                            menuObj.transform.position = clickPoint;
                            menuObj.SetActive(true);
                            return;
                        }
                        break;
                    case "Interactive":
                        break;
                    case "UI":
                        if(buttonNumber < 0)
                        {
                            Debug.Log("Command!");
                            return;
                        }
                        break;
                    default:
                        break;
                }
            }

            menuObj.SetActive(false);
        }
    }
}
