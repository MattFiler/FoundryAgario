using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootFlames : MonoBehaviour
{
    public GetMousePos mousePos;
    ParticleSystem[] flames;
    public float flameEmmisionRate = 50;

    private void Start()
    {
        flames = GetComponentsInChildren<ParticleSystem>();
    }

    void Update()
    {
        foreach (ParticleSystem ps in flames)
        {
            if (mousePos.mouseDown)
            {
                if (ShipResourceManagement.Instance.ResourceIsEmpty(ContractAssignee.RED))
                {
                    ps.Stop();
                }
                else
                {
                    ShipResourceManagement.Instance.UseResource(ContractAssignee.RED);
                    ps.Play();
                }
            }
            else
            {
                ps.Stop();
            }
        }
    }
}
