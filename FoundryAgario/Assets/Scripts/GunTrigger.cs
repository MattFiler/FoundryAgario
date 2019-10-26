using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunTrigger : MonoBehaviour
{
    public bool mouseOver = false;

    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition).origin, Vector3.forward, Mathf.Infinity, 1 << gameObject.layer);
        if (hit.collider != null)
        {
            if (hit.collider.transform.name == name)
            {
                mouseOver = true;
            }
        }
        else
        {
            mouseOver = false;
        }
    }
}
