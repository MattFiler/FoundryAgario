using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContractConsumption : MonoBehaviour
{
    // Start is called before the first frame update
    public GrabberMovement grabberMovement;
    private BoxCollider2D boxCollider;
    public int amountEaten = 0;
    void Start()
    {
        boxCollider = this.GetComponent<BoxCollider2D>();
        boxCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!grabberMovement.hasExtended && !grabberMovement.isExtending)
        {
            boxCollider.enabled = true;
        }
        else
        {
            boxCollider.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Friendly")
        {
            amountEaten++;
            Destroy(collision.gameObject);
        }
    }
}
