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
        flames.emissionRate = mousePos.mouseDown ? flameEmmisionRate : 0;
    }
}
