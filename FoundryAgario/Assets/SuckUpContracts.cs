using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuckUpContracts : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform lerpPoint;
    CheckForContracts checkForContracts;
    public GetMousePos mousePos;
    public GameObject succParticlesObj;
    public AudioSource succsound;

    [SerializeField] float shakeSpeed = 1.0f;
    [SerializeField] float shakeAmount = 1.0f;

    public float succSpeed = 1.0f;
    void Start()
    {
        checkForContracts = GetComponentInChildren<CheckForContracts>();
       
    }

    // Update is called once per frame
    void Update()
    {
        if (mousePos.mouseDown)
        {
            Debug.Log("my size " +  transform.localScale.x);
            succsound.Play();
            foreach (GameObject contract in checkForContracts.currentContracts)
            {
                Debug.Log("contract size " + contract.GetComponent<FriendlyAI>().GetWidth());
                if (contract.GetComponent<FriendlyAI>().GetWidth() > transform.localScale.x)
                {
                   
                    Debug.Log("Me me big boi");
                    //contract.transform.position = Vector2.one * Mathf.Sin(Time.time * shakeSpeed) * shakeAmount;
                    continue;
                }

                contract.transform.position = Vector2.Lerp(contract.transform.position, lerpPoint.position, succSpeed * Time.deltaTime);
            }
        }
          
       
        succParticlesObj.SetActive(mousePos.mouseDown);

        
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
                succsound.Stop();
            }
        }
    }
}
