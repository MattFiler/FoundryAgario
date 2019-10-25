using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireLazer : MonoBehaviour
{
    public Transform rayPoint;
    public LayerMask layerMask;
    public GetMousePos mousePos;
    LineRenderer lazerBeam;


    private void Start()
    {
        lazerBeam = GetComponent<LineRenderer>();
    }


    void Update()
    {
        if (mousePos.mouseDown)
        {
            lazerBeam.enabled = true;

            lazerBeam.SetPosition(0, transform.parent.position);
            lazerBeam.SetPosition(1, rayPoint.position);

            List<RaycastHit2D> hits = new List<RaycastHit2D>();
            ContactFilter2D cf = new ContactFilter2D();
            cf.useTriggers = true;
            Physics2D.Raycast(transform.parent.position, rayPoint.position - transform.position, cf, hits);

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null)
                {
                    Debug.Log(hit.collider.gameObject.name);

                    if (hit.transform.CompareTag("Friendly"))
                    {
                        hit.collider.GetComponent<FriendlyAI>().shrink = true;
                        lazerBeam.SetPosition(1, hit.point);
                        break;
                    }
                    else if(!hit.transform.CompareTag("Turret Outline"))
                    {
                        lazerBeam.SetPosition(1, hit.point);
                        StopShrinking();
                    }
                    else
                    {
                        StopShrinking();
                    }


                }
                else
                {
                    StopShrinking();
                }
            }

            if(hits == null)
            {
                Debug.Log("null");
                StopShrinking();
            }

        }
        else
        {
            StopShrinking();
            lazerBeam.enabled = false;
        }

    }

    private void StopShrinking()
    {
        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Friendly"))
        {
            gameObject.GetComponent<FriendlyAI>().shrink = false;
        }
    }
}

