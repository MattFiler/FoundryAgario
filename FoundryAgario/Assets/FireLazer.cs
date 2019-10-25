using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class FireLazer : MonoBehaviour
{
    public Transform rayPoint;
    public LayerMask layerMask;
    public GetMousePos mousePos;
    public int maxLazerCount = 3;
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
            lazerBeam.positionCount = 1;
            lazerBeam.SetPosition(0, transform.parent.position);
            //lazerBeam.SetPosition(1, rayPoint.position);
            CreateBeam(0, transform.parent.position, rayPoint.position - transform.position);

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

    private void CreateBeam (int lazerCount, Vector2 rayOrigin, Vector2 rayDirection)
    {
        rayOrigin += rayDirection.normalized;
        if(lazerCount >= maxLazerCount)
        {
            return;
        }

        List<RaycastHit2D> hits = new List<RaycastHit2D>();
        ContactFilter2D cf = new ContactFilter2D();
        cf.useTriggers = true;
        Physics2D.Raycast(rayOrigin, rayDirection, cf, hits);
        Debug.DrawRay(rayOrigin, rayDirection, Color.red);
        //Debug.Log("Count: " + lazerCount + " Origin: " + rayOrigin);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null)
            {
                //Debug.Log(hit.collider.gameObject.name);

                if (hit.transform.CompareTag("Friendly"))
                {
                    Debug.Log("Shrink");
                    hit.collider.GetComponent<FriendlyAI>().shrink = true;
                    ++lazerBeam.positionCount;
                    lazerBeam.SetPosition(lazerBeam.positionCount - 1, hit.point);
                    return;
                }
                else if (!hit.transform.CompareTag("Turret Outline"))
                {
                    ++lazerBeam.positionCount;
                    lazerBeam.SetPosition(lazerBeam.positionCount - 1, hit.point);
                    Vector2 reflectedBeam = Vector2.Reflect(rayDirection, hit.normal);
                    //lazerBeam.SetPosition(lazerBeam.positionCount - 1, hit.point + reflectedBeam);
                    CreateBeam(lazerCount + 1, hit.point, reflectedBeam);
                    //StopShrinking();
                    return;
                }
            }

        }

        //If the raycast hits nothing
        ++lazerBeam.positionCount;
        lazerBeam.SetPosition(lazerBeam.positionCount - 1, rayOrigin + rayDirection);
        StopShrinking();

    }


}

