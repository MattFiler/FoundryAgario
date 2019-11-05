using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class FireLazer : MonoBehaviour
{
    public Transform startRayPoint;
    public Transform endRayPoint;
    public LayerMask layerMask;
    public GetMousePos mousePos;
    public int maxLazerCount = 3;
    LineRenderer laserBeam;
    public float laserLength = 10.0f;

    public float laserZPos = -1;
    public GameObject light;

    public GameObject particleSystem;

    public List<GameObject> ignoreObjects;
    private void Start()
    {
        laserBeam = GetComponent<LineRenderer>();
    }


    void Update()
    {
        light.SetActive(mousePos.mouseDown);
        particleSystem.SetActive(mousePos.mouseDown);

        if (mousePos.mouseDown)
        {
            if (ShipResourceManagement.Instance.ResourceIsEmpty(ContractAssignee.GREEN))
            {
                light.SetActive(false);
                particleSystem.SetActive(false);
                laserBeam.positionCount = 0;
                return;
            }
            ShipResourceManagement.Instance.UseResource(ContractAssignee.GREEN);


            laserBeam.enabled = true;
            laserBeam.positionCount = 1;
            laserBeam.SetPosition(0, startRayPoint.position);
            //lazerBeam.SetPosition(1, rayPoint.position);
            CreateBeam(0, startRayPoint.position, endRayPoint.position - transform.position, laserLength);

            var positions = new Vector3[laserBeam.positionCount];
            for (int i = 0; i < laserBeam.positionCount; i++)
            {
                positions[i] = new Vector3(laserBeam.GetPosition(i).x, laserBeam.GetPosition(i).y, laserZPos);
            }

            laserBeam.SetPositions(positions);

        }
        else
        {
            StopShrinking();
            laserBeam.enabled = false;
        }

    }

    private void StopShrinking()
    {
        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Friendly"))
        {
            gameObject.GetComponent<FriendlyAI>().shrink = false;
        }
    }

    private void CreateBeam (int lazerCount, Vector2 rayOrigin, Vector2 rayDirection, float currentLaserLength)
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
            if(hit.distance > currentLaserLength)
            {
                continue;
            }

            if (hit.collider != null)
            {
                if(ignoreObjects.Contains(hit.collider.gameObject))
                {
                    continue;
                }

                if (hit.transform.CompareTag("Friendly"))
                {
                    Debug.Log("Shrink");
                    hit.collider.GetComponent<FriendlyAI>().shrink = true;
                    ++laserBeam.positionCount;
                    laserBeam.SetPosition(laserBeam.positionCount - 1, hit.point);
                    return;
                }
                else if (hit.transform.CompareTag("Enemy") || hit.transform.CompareTag("Ship"))
                {
                    Debug.Log(hit.collider.gameObject.name);

                    ++laserBeam.positionCount;
                    laserBeam.SetPosition(laserBeam.positionCount - 1, hit.point);
                    Vector2 reflectedBeam = Vector2.Reflect(rayDirection, hit.normal);
                    //lazerBeam.SetPosition(lazerBeam.positionCount - 1, hit.point + reflectedBeam);
                    CreateBeam(lazerCount + 1, hit.point, reflectedBeam, currentLaserLength - hit.distance);
                    //StopShrinking();
                    return;
                }
            }

        }

        //If the raycast hits nothing
        ++laserBeam.positionCount;
        laserBeam.SetPosition(laserBeam.positionCount - 1, rayOrigin + (rayDirection.normalized * currentLaserLength));
        StopShrinking();

    }
}

