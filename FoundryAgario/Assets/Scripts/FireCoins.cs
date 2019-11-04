using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCoins : MonoBehaviour
{
    public GetMousePos mousePos;
     ParticleSystem coins;
    public GameObject sparkles;
    public float coinEmmisionRate = 50;
    public float sparkleEmmisionRate = 30;
    public GameObject light;
    public AudioSource coinnoise;

    private void Start()
    {
        coins = GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {
        coins.emissionRate = mousePos.mouseDown? coinEmmisionRate : 0;
        sparkles.GetComponent<ParticleSystem>().emissionRate = mousePos.mouseDown? sparkleEmmisionRate : 0;
        light.SetActive(mousePos.mouseDown);

        if (mousePos.mouseDown)
        {
            if (!coinnoise.isPlaying)
            {
                coinnoise.Play();
            }
        }

        else
        {
            coinnoise.Stop();
        }
    }
}
