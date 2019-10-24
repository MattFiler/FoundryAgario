using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetMousePos : MonoBehaviour
{

    public float rotation;
    public Transform centre;

    public bool mouseDown = false;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        Touch touch = new Touch();
        if (TouchManager.instance.GetTouch(this.gameObject, ref touch))
        {
            if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
            {
                mouseDown = true;
                rotation = Quaternion.FromToRotation(Vector2.up, Camera.main.ScreenPointToRay(Input.mousePosition).origin - centre.position).eulerAngles.z;
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
}
