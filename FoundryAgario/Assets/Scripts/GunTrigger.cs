using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunTrigger : MonoBehaviour
{
    public bool mouseOver = false;

    //private void OnMouseEnter()
    //{
    //    Debug.Log("Enter " + gameObject.name);
    //    mouseOver = true;
    //}

    //private void OnMouseExit()
    //{
    //    Debug.Log("Exit " + gameObject.name);
    //    mouseOver = false;
    //}

    void GetMouseInfo()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition).origin, Vector3.forward, Mathf.Infinity,1 << gameObject.layer);
        if (hit.collider != null)
        {
            if (hit.collider.transform.name == name)
            {
                mouseOver = true;
                Debug.Log("Mouse is over " + name + ".");
            }
        }
        else
        {
            mouseOver = false;
        }

    }

    void Update()
    {
        GetMouseInfo();
    }
}
