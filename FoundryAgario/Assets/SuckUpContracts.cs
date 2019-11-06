using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuckUpContracts : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform lerpPoint;
    CheckForContracts checkForContracts;
    public GetMousePos mousePos;
    public GameObject goodSuccParticlesObj;
    public GameObject badSuccParticlesObj;
    public AudioSource goodSuccSound;
    public AudioSource badSuccSound;

    [SerializeField] float shakeSpeed = 1.0f;
    [SerializeField] float shakeAmount = 1.0f;

    public float succSpeed = 1.0f;

    [SerializeField] bool goodParticles = true;

    public GameObject light;
    void Start()
    {
        checkForContracts = GetComponentInChildren<CheckForContracts>();
       
    }

    // Update is called once per frame
    void Update()
    {
        light.SetActive(mousePos.mouseDown && !ShipResourceManagement.Instance.ResourceIsEmpty(ContractAssignee.BLUE));

        if (mousePos.mouseDown && !ShipResourceManagement.Instance.ResourceIsEmpty(ContractAssignee.BLUE))
        {
            ShipResourceManagement.Instance.UseResource(ContractAssignee.BLUE);


            Debug.Log("my size " +  transform.localScale.x);

           

            foreach (GameObject contract in checkForContracts.currentContracts)
            {
                Debug.Log("contract size " + contract.GetComponent<FriendlyAI>().GetWidth());
                if (contract.GetComponent<FriendlyAI>().GetWidth() > transform.localScale.x)
                {
                    Debug.Log("Me me big boi");
                    goodParticles = false;

                    if(!badSuccSound.isPlaying)
                    {
                        badSuccSound.PlayOneShot(badSuccSound.clip);
                        goodSuccSound.Stop();
                    }

                    //contract.transform.position = Vector2.one * Mathf.Sin(Time.time * shakeSpeed) * shakeAmount;
                    continue;
                }

                goodParticles = true;
                contract.transform.position = Vector2.Lerp(contract.transform.position, lerpPoint.position, succSpeed * Time.deltaTime);
            }

            if (goodParticles && !goodSuccSound.isPlaying)
            {
                goodSuccSound.Play();
            }
        }
        else
        {
            goodSuccSound.Stop();
        }
          
       
        if(mousePos.mouseDown)
        {
            goodSuccParticlesObj.SetActive(goodParticles);
            badSuccParticlesObj.SetActive(!goodParticles);
        }
        else 
        {
            goodSuccParticlesObj.SetActive(false);
            badSuccParticlesObj.SetActive(false);
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (mousePos.mouseDown && checkForContracts.currentContracts.Contains(collision.gameObject)
            && collision.GetComponent<FriendlyAI>().GetWidth() <= transform.localScale.x)
        {
            FriendlyAI thisAI = collision.gameObject.GetComponent<FriendlyAI>();
            if (thisAI)
            {
                ShipResourceManagement.Instance.ImportContract(thisAI);
                GameObject.Destroy(collision.gameObject); 
                goodSuccSound.Stop();
            }
        }
    }
}
