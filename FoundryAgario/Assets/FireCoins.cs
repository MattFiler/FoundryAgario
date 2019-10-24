using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCoins : MonoBehaviour
{
    public GetMousePos mousePos;
    ParticleSystem coins;
    public float emmisionRate = 50;

    private void Start()
    {
        coins = GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {
        coins.emissionRate = mousePos.mouseDown? emmisionRate : 0;
    }
}
