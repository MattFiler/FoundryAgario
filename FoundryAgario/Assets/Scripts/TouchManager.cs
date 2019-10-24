﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    public static TouchManager instance = null;

    public LayerMask validTouchLayers;

    private Dictionary<GameObject, Touch> touchedObjects = new Dictionary<GameObject, Touch>();


    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        touchedObjects.Clear();
        Touch mouseTouch = new Touch();
        mouseTouch.position = Input.mousePosition;

        bool mouseClicked = false;

        if (Input.GetMouseButtonDown(0))
        {
            mouseTouch.phase = TouchPhase.Began;
            mouseClicked = true;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            mouseTouch.phase = TouchPhase.Ended;
            mouseClicked = true;
        }
        else if(Input.GetMouseButton(0))
        {
            mouseClicked = true;
            mouseTouch.phase = TouchPhase.Stationary;
        }

        List<Touch> allTouches = new List<Touch>(Input.touches);

        if (mouseClicked)
        {
            allTouches.Add(mouseTouch);
        }

        for(int i = 0; i < allTouches.Count; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenPointToRay(allTouches[i].position).origin, Vector3.forward, Mathf.Infinity, validTouchLayers);
            if (hit.collider != null)
            {
                if (!touchedObjects.ContainsKey(hit.collider.gameObject))
                {
                    Touch touchy = allTouches[i];
                    touchy.position = hit.point;
                    touchedObjects.Add(hit.collider.gameObject, touchy);
                }
            }
        }
    }

    public bool GetTouch(GameObject queryObject, ref Touch touch)
    {
        if(touchedObjects.ContainsKey(queryObject))
        {
            touch = touchedObjects[queryObject];
            return true;
        }
        else
        {
            return false;
        }
    }
}
