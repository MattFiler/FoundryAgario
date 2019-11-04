using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGunAndStay : MonoBehaviour
{
    public GetMousePos mousePos;
    public GrabberMovement grabberMovement;
    public bool canFire = true;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (mousePos.mouseDown && canFire)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, mousePos.rotation);
        }
        else if(!grabberMovement.isExtending && !grabberMovement.hasExtended && !grabberMovement.hasStartedOpenClose)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, transform.parent.rotation, 0.1f);
        }
    }
}
