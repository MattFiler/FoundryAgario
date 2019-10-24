using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePosDebug : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Touch touch = new Touch();
        if (TouchManager.instance.GetTouch(this.gameObject, ref touch))
        {
            transform.position = touch.position;
        }
    }
}
