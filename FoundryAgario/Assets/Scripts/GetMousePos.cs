using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetMousePos : MonoBehaviour
{

    public float rotation;
    public Transform centre;
    public DragAlongPath dragAlongPath;

    public bool mouseDown = false;

    void Update()
    {
        Touch touch = new Touch();
        if (TouchManager.instance.GetTouch(this.gameObject, ref touch))
        {
            //Debug.Log("touched");
            if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
            {
                mouseDown = true;
                rotation = Quaternion.FromToRotation(Vector2.up, Camera.main.ScreenPointToRay(Input.mousePosition).origin - centre.position).eulerAngles.z;
                dragAlongPath.forcePolyEnable = true;
            }
            else
            {
                dragAlongPath.forcePolyEnable = false;
                mouseDown = false;
            }
        }
        else
        {
            dragAlongPath.forcePolyEnable = false;
            mouseDown = false;
        }
    }
}
