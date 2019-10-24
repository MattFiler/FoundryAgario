using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireLazer : MonoBehaviour
{
    public Transform rayPoint;
    public LayerMask layerMask;
    public GetMousePos mousePos;

    void Update()
    {
        if (mousePos.mouseDown)
        {
            List<RaycastHit2D> hits = new List<RaycastHit2D>();
            ContactFilter2D cf = new ContactFilter2D();
            cf.useTriggers = true;
            Physics2D.Raycast(transform.parent.position, rayPoint.position - transform.position, cf, hits);

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null && hit.transform.CompareTag("Friendly"))
                {
                    Debug.Log(hit.collider.gameObject.name);
                    Debug.DrawRay(transform.parent.position, hit.point - (Vector2)transform.position, Color.red);
                }
            }

        }

    }
}

