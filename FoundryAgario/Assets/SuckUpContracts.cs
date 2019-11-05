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
            foreach (GameObject contract in checkForContracts.currentContracts)
            {
                if(contract.GetComponent<BoxCollider2D>().size.x > GetComponent<BoxCollider2D>().size.x)
                {
                    Debug.Log("Me me big boi");
                    contract.transform.position = Vector2.one * Mathf.Sin(Time.time * shakeSpeed) * shakeAmount;
                    continue;
                }

                contract.transform.position = Vector2.Lerp(contract.transform.position, lerpPoint.position, succSpeed * Time.deltaTime);
            }
        }

        succParticlesObj.SetActive(mousePos.mouseDown);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (checkForContracts.currentContracts.Contains(collision.gameObject))
        {
            FriendlyAI thisAI = collision.gameObject.GetComponent<FriendlyAI>();
            if (thisAI)
            {
                ShipResourceManagement.Instance.ImportContract(thisAI);
                GameObject.Destroy(collision.gameObject);
            }
        }
    }
}
