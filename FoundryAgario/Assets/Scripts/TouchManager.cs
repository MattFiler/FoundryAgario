using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    public static TouchManager instance = null;

    public LayerMask validTouchLayers;

    private Dictionary<GameObject, Touch> touchedObjects = new Dictionary<GameObject, Touch>();
    private Dictionary<int, GameObject> lastFrameTouches = new Dictionary<int, GameObject>();

    private ContactFilter2D cf;


    // Start is called before the first frame update
    void Start()
    {
        cf = new ContactFilter2D();
        cf.layerMask = validTouchLayers;
        cf.useTriggers = true;

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
        Dictionary<int, GameObject> currentFrameTouches = new Dictionary<int, GameObject>();
        touchedObjects.Clear();
        Touch mouseTouch = new Touch();
        mouseTouch.position = Input.mousePosition;

        bool mouseClicked = false;

        if (Input.GetMouseButtonDown(0))
        {
            mouseTouch.phase = TouchPhase.Began;
            mouseClicked = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            mouseTouch.phase = TouchPhase.Ended;
            mouseClicked = true;
        }
        else if (Input.GetMouseButton(0))
        {
            mouseClicked = true;
            mouseTouch.phase = TouchPhase.Stationary;
        }

        List<Touch> allTouches = new List<Touch>(Input.touches);

        if (mouseClicked)
        {
            allTouches.Add(mouseTouch);
        }

        List<Touch> removeList = new List<Touch>();
        foreach (Touch t in allTouches)
        {
            if (lastFrameTouches.ContainsKey(t.fingerId))
            {
                RaycastHit2D[] hits = new RaycastHit2D[10];
                Physics2D.Raycast(Camera.main.ScreenPointToRay(t.position).origin, Vector3.forward, cf, hits);
                foreach (RaycastHit2D hit in hits)
                {
                    if (hit.collider != null && hit.transform.gameObject == lastFrameTouches[t.fingerId])
                    {
                        Touch touchy = t;
                        touchy.position = hit.point;
                        //Debug.Log("Dic 1");
                        touchedObjects.Add(hit.collider.gameObject, touchy);
                        removeList.Add(t);
                        currentFrameTouches[t.fingerId] = hit.transform.gameObject;
                        break;
                    }
                }
            }
        }

        foreach(Touch t in removeList)
        {
            allTouches.Remove(t);
        }

        foreach (Touch t in allTouches)
        {
            RaycastHit2D[] hits = new RaycastHit2D[10];
            Physics2D.Raycast(Camera.main.ScreenPointToRay(t.position).origin, Vector3.forward, cf, hits);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null && hit.collider.gameObject.layer == 8) // I've given up on life
                {
                    if (!touchedObjects.ContainsKey(hit.collider.gameObject))
                    {
                        Touch touchy = t;
                        touchy.position = hit.point;
                        Debug.Log("Dic 2");
                        touchedObjects.Add(hit.collider.gameObject, touchy);
                        currentFrameTouches[t.fingerId] = hit.transform.gameObject;
                    }
                }
            }
        }
        lastFrameTouches = currentFrameTouches;
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
