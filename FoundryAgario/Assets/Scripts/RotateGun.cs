using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGun : MonoBehaviour
{
    public GetMousePos mousePos;
    public bool canFire = true;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (mousePos.mouseDown && canFire)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, mousePos.rotation);
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, transform.parent.rotation, 0.1f);
        }
    }
}
