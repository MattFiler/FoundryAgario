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
    public GameObject particleSystem;
    public AudioSource coinaudio;

    private void Start()
    {
        coins = GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {
        particleSystem.SetActive(mousePos.mouseDown);

        if (mousePos.mouseDown && !ShipResourceManagement.Instance.ResourceIsEmpty(ContractAssignee.YELLOW))
        {
            ShipResourceManagement.Instance.UseResource(ContractAssignee.YELLOW);

            coins.emissionRate = coinEmmisionRate;
            sparkles.GetComponent<ParticleSystem>().emissionRate = sparkleEmmisionRate;
            light.SetActive(true);
            if (!coinaudio.isPlaying)
            {
                coinaudio.Play();
            }
        }
        else
        {
            coins.emissionRate = 0;
            sparkles.GetComponent<ParticleSystem>().emissionRate = 0;
            light.SetActive(false);
            coinaudio.Stop();
        }
    }
}
