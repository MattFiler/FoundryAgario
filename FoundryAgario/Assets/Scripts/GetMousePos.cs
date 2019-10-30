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
        Touch touch = NewMethod();
        if (TouchManager.instance.GetTouch(this.gameObject, ref touch))
        {
            Debug.Log(name+ " " + touch.position);
            if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
            {
                mouseDown = true;
                rotation = Quaternion.FromToRotation(Vector2.up,(Vector3) touch.position - centre.position).eulerAngles.z;
            }
            else
            {
                mouseDown = false;
            }
        }
        else
        {
            mouseDown = false;
        }
    }

    private static Touch NewMethod()
    {
        return new Touch();
    }
}
