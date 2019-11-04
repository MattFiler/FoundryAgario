using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContractConsumption : MonoBehaviour
{
    // Start is called before the first frame update
    public GrabberMovement grabberMovement;
    private BoxCollider2D boxCollider;
    public int amountEaten = 0;
    public float timeToConsume = 0.25f;

    public float timer = 0.0f;
    void Start()
    {
        boxCollider = this.GetComponent<BoxCollider2D>();
        boxCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (!grabberMovement.hasExtended && !grabberMovement.isExtending)
        {
            if (timeToConsume < timer)
            {
                boxCollider.enabled = false;
            }
            else
            {
                boxCollider.enabled = true;
            }
        }
        else
        {
            boxCollider.enabled = false;
            timer = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Friendly")
        {
            amountEaten++;
            if (collision.gameObject.GetComponent<FriendlyAI>() != null)
            {
                ShipResourceManagement.Instance.ImportContract(collision.gameObject.GetComponent<FriendlyAI>());
            }
            Destroy(collision.gameObject);
        }
    }
}
