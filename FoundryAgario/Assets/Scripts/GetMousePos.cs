using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetMousePos : MonoBehaviour
{
    public GunTrigger outerRingTrigger;
    public GunTrigger innerRingTrigger;
    public GunTrigger boxTrigger;

    public float rotation;
    public Transform centre;

    // Start is called before the first frame update
    void Start()
    {
        outerRingTrigger = transform.Find("Outer Ring Trigger").GetComponent<GunTrigger>();
        innerRingTrigger = transform.Find("Inner Ring Trigger").GetComponent<GunTrigger>();
        boxTrigger = transform.Find("Box Trigger").GetComponent<GunTrigger>();
    }

    // Update is called once per frame
    void Update()
    {
        if(outerRingTrigger.mouseOver && boxTrigger.mouseOver && !innerRingTrigger.mouseOver)
        {
            if(Input.GetMouseButton(0))
            {
                rotation = Quaternion.FromToRotation(Vector2.up, Camera.main.ScreenPointToRay(Input.mousePosition).origin - centre.position).eulerAngles.z;
            }
        }
    }
}
