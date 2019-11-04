using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootFlames : MonoBehaviour
{
    public GetMousePos mousePos;
    ParticleSystem flames;
    public float flameEmmisionRate = 50;

    private void Start()
    {
        flames = GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {
        if (mousePos.mouseDown)
        {
            if (ShipResourceManagement.Instance.ResourceIsEmpty(ContractAssignee.RED))
            {
                flames.emissionRate = 0;
            }
            else
            {
                ShipResourceManagement.Instance.UseResource(ContractAssignee.RED);
                flames.emissionRate = flameEmmisionRate;
            }
        }
        else
        {
            flames.emissionRate = 0;
        }
    }
}
